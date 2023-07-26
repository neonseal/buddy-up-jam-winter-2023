using DG.Tweening;
using Dialogue;
using GameState;
using PlayArea;
using Scriptables.DamageInstructions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// Mending Game Manager
/// 
/// Once an individual damage is selected, this class will generate the appropriate 
/// repair mini-game, which may be one of the following depending on the damage type: 
/// - Sewing 
/// - Cutting  
/// - Stuffing
/// 
/// The generator keeps reference to the magnifying glass lens, where it populates the 
/// corresponding mending game, emitting an event when the repair is complete to transition
/// to the next step of the repair or complete the game.
/// </summary>
namespace MendingGames {
    public enum MendingGameType {
        Sewing,
        Cutting,
        Stuffing
    }

    public class MendingGameManager : MonoBehaviour {
        [Header("High-level Status/Progress Elements")]
        private DamageInstructrionsScriptableObject[] damageInstructions;
        private int damageRepairStepIndex;
        private bool mendingGameInProgress;
        private int activeNodeIndex;
        private ToolType requiredToolType;
        [SerializeField] private PlayAreaCanvasManager canvasManager;
        [Range(0f, 1f)]
        [SerializeField] private float lineCompleteThreshold;

        [Header("Sewing/Cutting Game Rendering")]
        [SerializeField] private Material defaultMaterial;
        [SerializeField] private Node mendingTargetPrefab;
        [SerializeField] private Dash mendingDashPrefab;
        [Range(0.01f, 1f)]
        [SerializeField] private float dashSize;
        [Range(0.1f, 2f)]
        [SerializeField] private float delta;

        [Header("Stuffing Game Rendering")]
        [SerializeField] private Texture2D mainTex;
        [SerializeField] private Texture2D stuffingBrush;
        [SerializeField] private Sprite stuffingSprite;
        [SerializeField] private Texture2D stuffingTexture;
        [SerializeField] private Material stuffingMaterial;
        [SerializeField] private GameObject stuffingForeground;

        [Header("Stuffing Game State Management")]
        private Texture2D stuffingMaskTexture;
        private Vector2Int lastPaintPixelPosition;
        private float unstuffedAreaTotal;
        private float unstuffedAreaCurrent;

        [Header("Magnifying Glass Lens Elements")]
        [SerializeField] private GameObject magnifyingGlass;
        [SerializeField] private GameObject magnifyingGlassLens;
        [SerializeField] private GameObject mendingGamePlayArea;
        private SpriteRenderer lensSpriteRenderer;

        [Header("Magnifying Glass Animation Elements")]
        [SerializeField] private float duration;
        [SerializeField] private Ease easeType;
        private Vector3 startingLocation;
        private Vector3 centerLocation;

        [Header("Game Component Collections")]
        private List<Node> nodes;
        private List<List<Dash>> dashSets;

        [Header("Tutorial/Dialogue Managers")]
        [SerializeField] private TutorialManager tutorialManager;

        /* Mending Game Events */
        public static event Action<DamageInstructrionsScriptableObject[]> OnMendingGameComplete;
        public static event Action<DamageInstructrionsScriptableObject> OnMendingStepComplete;

        private void Awake() {
            nodes = new List<Node>();
            dashSets = new List<List<Dash>>();
            lensSpriteRenderer = magnifyingGlassLens.GetComponent<SpriteRenderer>();

            damageRepairStepIndex = -1;
            startingLocation = magnifyingGlass.transform.localPosition;
            centerLocation = new Vector3(5.5f, -10, 0);

            PlushieActiveState.MendingGameInitiated += GenerateMendingGame;
        }

        private void Update() {
            // Update stuffing material if interacting with stuffing game
            if (mendingGameInProgress && requiredToolType == ToolType.Stuffing) {
                // Check for raycast hits on magnifying glass and update mask
            }
        }

        /*                      COMMON FUNCTIONS                       */
        /* ----------------------------------------------------------- */
        // Primary function invoked by the Mending Game Manager, responsible for parsing the 
        // damage instructions and rendering the appropriate game to the lens
        public void GenerateMendingGame(DamageInstructrionsScriptableObject[] damageInstructions) {
            mendingGameInProgress = true;
            this.damageInstructions = damageInstructions;
            this.damageRepairStepIndex++;

            // Check first step of damage instructions to determine starting damage type
            switch (damageInstructions[damageRepairStepIndex].PlushieDamageType) {
                // TODO: Change to switch of MendingGameType (May need to add to DamageInstruction Scriptable)
                case PlushieDamageType.SmallRip:
                    GenerateSewingOrCuttingGame();
                    break;
                case PlushieDamageType.LargeRip:
                    break;
                case PlushieDamageType.WornStuffing:
                    GenerateSewingOrCuttingGame();
                    break;
                case PlushieDamageType.MissingStuffing:
                    GenerateStuffingGame();
                    break;
            }

            magnifyingGlass.transform.DOLocalMove(centerLocation, duration).SetEase(easeType);

            // Check if there is a tutorial active that requires a continue action, and continue tutorial
            if (tutorialManager.GetRequiredContinueAction() == TutorialActionRequiredContinueType.SelectDamage) {
                tutorialManager.ContinueTutorialSequence();
            }
        }


        /*                 SEWING AND CUTTING GAMES                    */
        /* ----------------------------------------------------------- */

        private void GenerateSewingOrCuttingGame() {
            Node.OnNodeTriggered += HandleTargetNodeTrigger;
            Node.OnActiveNodeReleased += ResetCurrentLine;

            Vector2[] targetLocations = this.damageInstructions[damageRepairStepIndex].TargetLocations;
            lensSpriteRenderer.sprite = this.damageInstructions[damageRepairStepIndex].DamageSprite;

            // Set updated rotation value
            mendingGamePlayArea.transform.Rotate(0, 0, this.damageInstructions[damageRepairStepIndex].RotationZValue);
            magnifyingGlassLens.transform.Rotate(0, 0, this.damageInstructions[damageRepairStepIndex].RotationZValue);

            // Ensure we are using the appropriate material
            if (lensSpriteRenderer.material != defaultMaterial) {
                lensSpriteRenderer.material = defaultMaterial;
            }

            // Generate sewing target nodes based on input target locations
            GenerateTargetNodes(targetLocations);

            // Generate dashes between each pair of target nodes
            for (int i = 0; i < nodes.Count - 1; i++) {
                Vector3 startingNodePos = nodes[i].transform.localPosition;
                Vector3 endingNodePos = nodes[i + 1].transform.localPosition;

                List<Vector3> dashPositions = GenerateDashPositions(startingNodePos, endingNodePos);
                List<Dash> dashes = RenderLine(dashPositions, startingNodePos, endingNodePos);

                dashSets.Add(dashes);
            }
        }

        private void CompleteSewingOrCuttingGame() {
            Node.OnNodeTriggered -= HandleTargetNodeTrigger;
            Node.OnActiveNodeReleased -= ResetCurrentLine;
            // Check if there are more steps to the repair process
            if (this.damageRepairStepIndex < this.damageInstructions.Count() - 1) {
                // Fire step complete event 
                // TODO: Add listener for this event on the ChecklistManager
                OnMendingStepComplete?.Invoke(this.damageInstructions[this.damageRepairStepIndex]);
                // Start next mending game step
                GenerateMendingGame(this.damageInstructions);
            } else {
                // Complete mending game and reset
                magnifyingGlass.transform.DOLocalMove(startingLocation, duration).SetEase(easeType);
                this.mendingGameInProgress = false;
                OnMendingGameComplete?.Invoke(this.damageInstructions);
            }


            // Check if there is a tutorial active that requires a continue action, and continue tutorial
            if (tutorialManager.GetRequiredContinueAction() == TutorialActionRequiredContinueType.CompleteRepair) {
                tutorialManager.ContinueTutorialSequence();
            }
        }

        private void HandleTargetNodeTrigger(Node triggeredNode) {
            this.activeNodeIndex = nodes.FindIndex(n => n == triggeredNode);
            // If starting node triggered, enable first line
            if (triggeredNode == nodes.First()) {
                EnableNextLine(triggeredNode);
            } else {
                bool lineComplete = CheckLineCompletion(triggeredNode);
                if (lineComplete) {
                    // Check if the triggered node is the last in the set, then complete the game/step
                    if (this.activeNodeIndex == nodes.Count - 1) {
                        CompleteSewingOrCuttingGame();
                    } else {
                        // Else, continue on to the next line
                        EnableNextLine(triggeredNode);
                        triggeredNode.ActiveNode = true;
                        nodes[this.activeNodeIndex - 1].ActiveNode = false;

                    }
                } else {
                    ResetCurrentLine(triggeredNode);
                }
            }

            // If triggered node is the first in the repair, and there is a corresponding tutorial continue action, continue tutorial
            if (triggeredNode.StartingNode && tutorialManager.GetRequiredContinueAction() == TutorialActionRequiredContinueType.StartRepair) {
                tutorialManager.ContinueTutorialSequence();
            }
        }
        private bool CheckLineCompletion(Node node) {
            // Check if enough dashes have been triggered to enable next line
            int dashTriggeredCount = dashSets[this.activeNodeIndex - 1].Where(d => d.Triggered).Count();
            return dashTriggeredCount >= this.lineCompleteThreshold;

        }

        private void EnableNextLine(Node node) {
            // Activate corresponding line of dashes
            foreach (Dash dash in dashSets[this.activeNodeIndex]) {
                dash.EnableDash(this.damageInstructions[damageRepairStepIndex].RequiredToolType);
            }
            // Activate next node as target node
            nodes[this.activeNodeIndex + 1].TargetNode = true;
            nodes[this.activeNodeIndex + 1].SetColor(Color.blue);
        }

        private void ResetCurrentLine(Node node) {
            // Reset current line and triggered node to try again
            foreach (Dash dash in dashSets[this.activeNodeIndex]) {
                dash.ResetDash(true);
            }
        }

        private void GenerateTargetNodes(Vector2[] targetLocations) {
            for (int i = 0; i < targetLocations.Length; i++) {
                Vector3 position = new Vector3(targetLocations[i].x, targetLocations[i].y);

                Node node = Instantiate(mendingTargetPrefab, mendingGamePlayArea.transform, false);
                node.CanvasManager = this.canvasManager;

                node.transform.localPosition = position;
                if (i == 0) {
                    node.StartingNode = true;
                    node.TargetNode = node.ActiveNode = true;
                    node.SetColor(Color.blue);
                }
                node.SetToolType(damageInstructions[damageRepairStepIndex].RequiredToolType);

                this.nodes.Add(node);
            }
        }

        /* Calculate dash positions between pairs of nodes where we will generate dash objects */
        private List<Vector3> GenerateDashPositions(Vector3 start, Vector3 end) {
            List<Vector3> positions = new List<Vector3>();
            // Triangulate a straight line between both point
            Vector3 direction = (end - start).normalized;
            Vector3 dash = start += direction * delta;


            // Incrementally calculate dash positions until we reach the end position
            while ((end - start).magnitude > (dash - start + (direction * delta * 0.65f)).magnitude) {
                // If within threshold of the ending position, add new position to list
                positions.Add(dash);
                dash += direction * delta;
            }

            return positions;
        }

        // Given a set of postitions to place dashes, and the positions of the starting and ending node positions
        // Generate and orient a line of evenly spaced dash objects
        private List<Dash> RenderLine(List<Vector3> dashPositions, Vector3 start, Vector3 end) {
            List<Dash> dashes = new List<Dash>();

            // Instantiate line of dashes between nodes
            foreach (Vector3 dashPosition in dashPositions) {
                Dash dashObject = GenerateDash(start, end);
                dashObject.CanvasManager = this.canvasManager;
                dashObject.DashSetIndex = this.dashSets.Count;
                dashObject.ParentNode = this.nodes[this.dashSets.Count];
                dashObject.transform.localPosition = dashPosition;
                dashes.Add(dashObject);
            }

            return dashes;
        }

        //Instantiate a new Dash object and calculate rotation between given node posisitons
        private Dash GenerateDash(Vector3 startingPos, Vector3 endingPos) {
            Dash dash = Instantiate(mendingDashPrefab, mendingGamePlayArea.transform, false);

            Vector2 diff = endingPos - startingPos;
            float sign = (endingPos.y < startingPos.y) ? -1.0f : 1.0f;
            float angle = Vector2.Angle(Vector2.right, diff) * sign;

            Quaternion rot = dash.transform.rotation;
            dash.transform.Rotate(0, 0, angle);
            return dash;
        }


        /*                        STUFFING GAME                         */
        /* ----------------------------------------------------------- */
        private void GenerateStuffingGame() {
            Assert.IsTrue(this.damageInstructions[damageRepairStepIndex].MendingGameType == MendingGameType.Stuffing,
                $"Damage Instructions does not contain stuffing game info at index {damageRepairStepIndex}!");
            DamageInstructrionsScriptableObject instructions = this.damageInstructions[damageRepairStepIndex];
            SpriteRenderer lensSpriteRenderer = magnifyingGlassLens.GetComponent<SpriteRenderer>();

            this.requiredToolType = ToolType.Stuffing;

            // Create Masking Texture
            stuffingMaskTexture = new Texture2D(instructions.StuffingBackgroundTexture.width, instructions.StuffingBackgroundTexture.height);
            stuffingMaskTexture.SetPixels(instructions.StuffingBackgroundTexture.GetPixels());
            stuffingMaskTexture.Apply();

            // Set textures on stuffing material for current plushie
            stuffingMaterial.SetTexture("_MainTex", stuffingTexture);
            stuffingMaterial.SetTexture("_UnstuffedTex", instructions.StuffingBackgroundTexture);
            stuffingMaterial.SetTexture("_Mask", stuffingMaskTexture);

            // Render unstuffed sprite and material on lens background
            lensSpriteRenderer.material = stuffingMaterial;
            lensSpriteRenderer.sprite = instructions.StuffingBackgroundSprite;

            // Activate and set foreground sprite
            stuffingForeground.GetComponent<SpriteRenderer>().sprite = instructions.DamageSprite;
            stuffingForeground.SetActive(true);

            unstuffedAreaTotal = 0f;
            for (int x = 0; x < instructions.StuffingBackgroundTexture.width; x++) {
                for (int y = 0; y < instructions.StuffingBackgroundTexture.height; y++) {
                    unstuffedAreaTotal += instructions.StuffingBackgroundTexture.GetPixel(x, y).g;
                }
            }
            unstuffedAreaCurrent = unstuffedAreaTotal;
        }




        /* Public Properties */
        public bool MendingGameInProgress { get => mendingGameInProgress; }
    }
}

