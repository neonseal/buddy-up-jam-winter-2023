using System;
using System.Collections.Generic;
using UnityEngine;
/* User-defined Namespaces */
using Scriptables.DamageInstructions;

/// <summary>
/// Mending Game Generator
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
    public class MendingGameGenerator : MonoBehaviour {
        [Header("High-level Status/Progress Elements")]
        private DamageInstructrionsScriptableObject damageInstructions;

        [Header("Game Component Rendering")]
        [SerializeField] private Material defaultMaterial;
        [SerializeField] private Node mendingTargetPrefab;
        [SerializeField] private Dash mendingDashPrefab;
        [SerializeField] private Sprite[] dashOptions;
        [Range(0.01f, 1f)]
        [SerializeField] private float dashSize;
        [Range(0.1f, 2f)]
        [SerializeField] private float delta;

        [Header("Magnifying Glass Lens Elements")]
        [SerializeField] private GameObject magnifyingGlassLens;
        private SpriteRenderer lensSpriteRenderer;

        [Header("Game Component Collections")]
        private List<Node> nodes;

        /* Mending Game Events */
        public static event Action OnMendingGameComplete;

        private void Awake() {
            nodes = new List<Node>();
            lensSpriteRenderer = magnifyingGlassLens.GetComponent<SpriteRenderer>();

            Node.OnTargetNodeTriggered += HandleTargetNodeTrigger;
        }

        /*                      COMMON FUNCTIONS                       */
        /* ----------------------------------------------------------- */
        // Primary function invoked by the Mending Game Manager, responsible for parsing the 
        // damage instructions and rendering the appropriate game to the lens
        public void GenerateMendingGame(DamageInstructrionsScriptableObject damageInstructions) {
            this.damageInstructions = damageInstructions;

            switch (damageInstructions.PlushieDamageType) {
                case PlushieDamageType.SmallRip:
                    GenerateSewingGame();
                    break;
                case PlushieDamageType.LargeRip:
                    break;
                case PlushieDamageType.WornStuffing:
                    break;
                case PlushieDamageType.MissingStuffing:
                    break;
            }
        }


        /*                 SEWING AND CUTTING GAMES                    */
        /* ----------------------------------------------------------- */
        private void HandleTargetNodeTrigger(Node node) {
            Node matchingNode = nodes.Find(n => node == n);
        }

        private void GenerateSewingGame() {
            Vector2[] targetLocations = this.damageInstructions.TargetLocations;
            lensSpriteRenderer.sprite = this.damageInstructions.DamageSprite;

            // Set updated rotation value
            this.transform.Rotate(0, 0, this.damageInstructions.RotationZValue);
            magnifyingGlassLens.transform.Rotate(0, 0, this.damageInstructions.RotationZValue);

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

            }
        }

        private void GenerateTargetNodes(Vector2[] targetLocations) {
            for (int i = 0; i < targetLocations.Length; i++) {
                Vector3 position = new Vector3(targetLocations[i].x, targetLocations[i].y);
                Node node = Instantiate(mendingTargetPrefab, this.transform, false);
                node.transform.localPosition = position;
                node.SetNodeProperties(damageInstructions.RequiredToolType, i == 0);
                this.nodes.Add(node);
            }
        }

        /* Calculate dash positions between pairs of nodes where we will generate dash objects */
        private List<Vector3> GenerateDashPositions(Vector3 start, Vector3 end) {
            List<Vector3> positions = new List<Vector3>();
            // Triangulate a straight line between both point
            Vector3 direction = (end - start).normalized;
            Vector3 dash = start += (direction * delta);


            // Incrementally calculate dash positions until we reach the end position
            while ((end - start).magnitude > (dash - start + (direction * delta * 0.65f)).magnitude) {
                // If within threshold of the ending position, add new position to list
                positions.Add(dash);
                dash += (direction * delta);
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
                dashObject.transform.localPosition = dashPosition;
                dashes.Add(dashObject);
            }

            return dashes;
        }

        //Instantiate a new Dash object and calculate rotation between given node posisitons
        private Dash GenerateDash(Vector3 startingPos, Vector3 endingPos) {
            Dash dash = Instantiate(mendingDashPrefab, this.transform, false);

            Vector2 diff = endingPos - startingPos;
            float sign = (endingPos.y < startingPos.y) ? -1.0f : 1.0f;
            float angle = Vector2.Angle(Vector2.right, diff) * sign;

            Quaternion rot = dash.transform.rotation;
            //dash.transform.localRotation = Quaternion.Euler(rot.x, rot.y, angle);
            dash.transform.Rotate(0, 0, angle);
            return dash;
        }


        /*                        STUFFING GAME                         */
        /* ----------------------------------------------------------- */



    }
}