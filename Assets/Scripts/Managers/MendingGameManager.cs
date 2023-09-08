using DG.Tweening;
using Dialogue;
using GameState;
using PlayArea;
using Scriptables.DamageInstructions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
        private PlushieDamageGO plushieDamage;
        private DamageInstructrionsScriptableObject[] damageInstructions;
        private DamageInstructrionsScriptableObject instructionStep;
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
        [SerializeField] private Texture2D uvMask;
        [SerializeField] private Texture2D stuffingBrush;
        [SerializeField] private Sprite stuffingSprite;
        [SerializeField] private Texture2D stuffingTexture;
        [SerializeField] private Material stuffingMaterial;
        [SerializeField] private GameObject stuffingForeground;
        [SerializeField] private float textureXPosMax;
        [SerializeField] private float textureYPosMax;
        private int textureWidth;
        private int textureHeight;

        [Header("Stuffing Game State Management")]
        private Texture2D stuffingMaskTexture;
        private Vector2Int lastPaintPixelPosition;
        private float unstuffedAreaTotal;
        private float stuffedAreaCurrent;
        private bool stepCompleteCalled;

        [Header("Magnifying Glass Lens Elements")]
        [SerializeField] private GameObject magnifyingGlass;
        [SerializeField] private GameObject magnifyingGlassLens;
        [SerializeField] private GameObject mendingGamePlayArea;
        private SpriteRenderer lensSpriteRenderer;
        private CircleCollider2D lensCircleCollider;

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
        public static event Action<PlushieDamageGO> OnMendingGameComplete;
        public static event Action<DamageInstructrionsScriptableObject> OnMendingStepComplete;

        private void Awake() {
            nodes = new List<Node>();
            dashSets = new List<List<Dash>>();
            lensSpriteRenderer = magnifyingGlassLens.GetComponent<SpriteRenderer>();
            lensCircleCollider = magnifyingGlassLens.GetComponent<CircleCollider2D>();

            damageRepairStepIndex = -1;
            startingLocation = magnifyingGlass.transform.localPosition;
            centerLocation = new Vector3(5.5f, -10, 0);
            stepCompleteCalled = false;

            PlushieActiveState.MendingGameInitiated += GenerateMendingGame;
        }

        private void Update() {
            // Update stuffing material if interacting with stuffing game
            if (mendingGameInProgress && requiredToolType == ToolType.Stuffing) {
                if (Input.GetMouseButton(0) && canvasManager.CurrentToolType == requiredToolType && !stepCompleteCalled) {
                    // Check for raycast hits on magnifying glass and update mask
                    Vector3 position = Input.mousePosition;
                    position.z = 0.0f;
                    position = Camera.main.ScreenToWorldPoint(position);
                    RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero);

                    if (hit.collider != null && hit.collider.name == "LensBackground") {
                        // Determine mouse position as percentage of collider extents 
                        if (GetStuffedAmount() < 0.9f) {
                            float percentX = (float)Math.Round((position.x + (textureXPosMax / 2f)) / textureXPosMax, 2);
                            float percentY = (float)Math.Round((position.y + (textureYPosMax / 2f)) / textureYPosMax, 2);

                            float clampedPercentX = Math.Clamp(percentX, 0.00f, 1.00f);
                            float clampedPercentY = Math.Clamp(percentY, 0.00f, 1.00f);

                            int pixelX = (int)(clampedPercentX * textureWidth);
                            int pixelY = (int)(clampedPercentY * textureHeight);

                            Vector2Int paintPixelPosition = new Vector2Int(pixelX, pixelY);

                            int paintPixelDistance = Mathf.Abs(paintPixelPosition.x - lastPaintPixelPosition.x) + Mathf.Abs(paintPixelPosition.y - lastPaintPixelPosition.y);
                            int maxPaintDistance = 7;
                            if (paintPixelDistance < maxPaintDistance) {
                                // Painting too close to last position
                                return;
                            }
                            lastPaintPixelPosition = paintPixelPosition;

                            int pixelXOffset = pixelX - (stuffingBrush.width / 2);
                            int pixelYOffset = pixelY - (stuffingBrush.height / 2);

                            for (int x = 0; x < stuffingBrush.width; x++) {
                                for (int y = 0; y < stuffingBrush.height; y++) {
                                    Color pixelDirt = stuffingBrush.GetPixel(x, y);
                                    Color pixelDirtMask = stuffingMaskTexture.GetPixel(pixelXOffset + x, pixelYOffset + y);

                                    float stuffedAmount = pixelDirtMask.g - (pixelDirtMask.g * pixelDirt.g);
                                    stuffedAreaCurrent += stuffedAmount;

                                    stuffingMaskTexture.SetPixel(
                                        pixelXOffset + x,
                                        pixelYOffset + y,
                                        new Color(0, pixelDirtMask.g * pixelDirt.g, 0)
                                    );
                                }
                            }

                            stuffingMaskTexture.Apply();
                        } else {
                            // Player has stuffed at least 90% of texture -> complete game and continue
                            stepCompleteCalled = true;
                            CompleteStuffingGame();
                        }
                    }

                }
            }
        }

        /*                      COMMON FUNCTIONS                       */
        /* ----------------------------------------------------------- */
        // Primary function invoked by the Mending Game Manager, responsible for parsing the 
        // damage instructions and rendering the appropriate game to the lens
        public void GenerateMendingGame(PlushieDamageGO plushieDamage) {
            mendingGameInProgress = true;
            this.plushieDamage = plushieDamage;
            this.damageInstructions = plushieDamage.GetDamageInstructrions();
            this.damageRepairStepIndex++;

            instructionStep = this.damageInstructions[damageRepairStepIndex];

            // Check first step of damage instructions to determine starting damage type
            switch (damageInstructions[damageRepairStepIndex].MendingGameType) {
                case MendingGameType.Sewing:
                case MendingGameType.Cutting:
                    GenerateSewingOrCuttingGame();
                    break;
                case MendingGameType.Stuffing:
                    this.requiredToolType = ToolType.Stuffing;
                    GenerateStuffingGame();
                    break;
            }

            magnifyingGlass.transform.DOLocalMove(centerLocation, duration).SetEase(easeType);

            // Check if there is a tutorial active that requires a continue action, and continue tutorial
            if (tutorialManager.GetRequiredContinueAction() == TutorialActionRequiredContinueType.SelectDamage) {
                tutorialManager.ContinueTutorialSequence();
            }
        }

        private void CompleteOrContinueMendingGames() {
            // Check if there are more steps to the repair process
            if (this.damageRepairStepIndex < this.damageInstructions.Count() - 1) {
                // Fire step complete event 
                OnMendingStepComplete?.Invoke(this.damageInstructions[this.damageRepairStepIndex]);
                // Start next mending game step
                GenerateMendingGame(this.plushieDamage);
            } else {
                // Complete mending game and reset
                magnifyingGlass.transform.DOLocalMove(startingLocation, duration).SetEase(easeType);
                this.mendingGameInProgress = false;
                OnMendingStepComplete?.Invoke(this.damageInstructions[this.damageRepairStepIndex]);
                OnMendingGameComplete?.Invoke(this.plushieDamage);
            }

            // Check if there is a tutorial active that requires a continue action, and continue tutorial
            if (tutorialManager.GetRequiredContinueAction() == TutorialActionRequiredContinueType.CompleteRepair) {
                tutorialManager.ContinueTutorialSequence();
            }
        }


        /*                 SEWING AND CUTTING GAMES                    */
        /* ----------------------------------------------------------- */

        private void GenerateSewingOrCuttingGame() {
            // If transitioning from cutting/sewing game to similar game, destroy previous game objects
            if (dashSets.Count > 0) {
                foreach (var dashSet in dashSets) {
                    foreach (Dash dash in dashSet) {
                        Destroy(dash);
                    }
                }
                foreach (Node node in nodes) {
                    Destroy(node);
                }
            }

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
            CompleteOrContinueMendingGames();
        }

        private void HandleTargetNodeTrigger(Node triggeredNode) {
            this.activeNodeIndex = nodes.FindIndex(n => n == triggeredNode);
            // If starting node triggered, enable first line
            if (triggeredNode == nodes.First()) {
                EnableNextLine(triggeredNode);
            } else {
                bool lineComplete = CheckLineCompletion(triggeredNode);
                if (lineComplete) {
                    // Complete corresponding line of dashes
                    foreach (Dash dash in dashSets[this.activeNodeIndex - 1].Where(d => !d.Triggered)) {
                        dash.TriggerDash();
                    }

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
                    this.activeNodeIndex--;


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
            List<Dash> dashSet = dashSets[this.activeNodeIndex - 1];
            float dashTriggeredCount = dashSet.Where(d => d.Triggered).Count();
            float percentComplete = dashTriggeredCount / dashSet.Count;
            return percentComplete >= this.lineCompleteThreshold;

        }

        private void EnableNextLine(Node node) {
            node.TargetNode = false;

            // Activate corresponding line of dashes
            foreach (Dash dash in dashSets[this.activeNodeIndex]) {
                dash.EnableDash(this.damageInstructions[damageRepairStepIndex].RequiredToolType);
            }
            // Activate next node as target node
            nodes[this.activeNodeIndex + 1].TargetNode = true;
            nodes[this.activeNodeIndex + 1].SetColor(Color.blue);
        }

        private void ResetCurrentLine(Node node) {
            node.TargetNode = true;
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
                node.gameObject.name = $"TargetNode_{i}";
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
            // Create Masking Texture
            stuffingMaskTexture = new Texture2D(uvMask.width, uvMask.height);
            stuffingMaskTexture.SetPixels(uvMask.GetPixels());
            stuffingMaskTexture.Apply();

            textureWidth = stuffingMaskTexture.width;
            textureHeight = stuffingMaskTexture.height;

            // Set textures on stuffing material for current plushie
            stuffingMaterial.SetTexture("_MainTex", stuffingTexture);
            stuffingMaterial.SetTexture("_UnstuffedTex", instructionStep.StuffingBackgroundTexture);
            stuffingMaterial.SetTexture("_Mask", stuffingMaskTexture);

            // Render unstuffed sprite and material on lens background
            lensSpriteRenderer.material = stuffingMaterial;
            lensSpriteRenderer.sprite = stuffingSprite;

            // Activate and set foreground sprite
            stuffingForeground.GetComponent<SpriteRenderer>().sprite = instructionStep.DamageSprite;
            stuffingForeground.SetActive(true);

            unstuffedAreaTotal = 0f;
            for (int x = 0; x < textureWidth; x++) {
                for (int y = 0; y < textureHeight; y++) {
                    unstuffedAreaTotal += stuffingMaskTexture.GetPixel(x, y).g;
                }
            }

            lensCircleCollider.enabled = true;
        }

        private float GetStuffedAmount() {
            return this.stuffedAreaCurrent / unstuffedAreaTotal;
        }

        private void CompleteStuffingGame() {
            Color32 stuffedColor = new Color32(0, 0, 0, 0);
            Color32[] colors = stuffingMaskTexture.GetPixels32();

            for (int i = 0; i < colors.Length; i++) {
                colors[i] = stuffedColor;
            }

            stuffingMaskTexture.SetPixels32(colors);
            stuffingMaskTexture.Apply();

            stuffingForeground.SetActive(false);
            lensCircleCollider.enabled = false;

            CompleteOrContinueMendingGames();
        }


        /* Public Properties */
        public bool MendingGameInProgress { get => mendingGameInProgress; }
    }
}


