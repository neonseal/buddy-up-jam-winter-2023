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
        [SerializeField] private GameObject mendingTargetPrefab;
        [SerializeField] private GameObject mendingDashPrefab;
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
            Debug.Log("CLICK");
            Node matchingNode = nodes.Find(n => node == n);
            Debug.Log("Node Triggered: " + matchingNode);
        }

        private void GenerateSewingGame() {
            Vector2[] targetLocations = this.damageInstructions.TargetLocations;
            lensSpriteRenderer.sprite = this.damageInstructions.DamageSprite;

            // Set updated rotation value
            Quaternion rotation = magnifyingGlassLens.transform.rotation;
            Vector3 newRotationVector = new Vector3(rotation.x, rotation.y, this.damageInstructions.RotationZValue);
            magnifyingGlassLens.transform.rotation = Quaternion.Euler(newRotationVector);

            // Ensure we are using the appropriate material
            if (lensSpriteRenderer.material != defaultMaterial) {
                lensSpriteRenderer.material = defaultMaterial;
            }

            // Generate sewing target nodes based on input target locations
            GenerateTargetNodes(targetLocations);

            // Generate dashes between each pair of target nodes
            for (int i = 0; i < nodes.Count - 1; i++) {
                Vector3 startingNodePos = nodes[i].transform.position;
                Vector3 endingNodePos = nodes[i + 1].transform.position;

                List<Vector3> dashPositions = GenerateDashPositions(startingNodePos, endingNodePos);

            }
        }

        private void GenerateTargetNodes(Vector2[] targetLocations) {
            for (int i = 0; i < targetLocations.Length; i++) {
                Vector3 position = new Vector3(targetLocations[i].x, targetLocations[i].y);
                GameObject gameObject = Instantiate(mendingTargetPrefab, this.transform, false);
                gameObject.transform.localPosition = position;
                Node node = gameObject.GetComponent<Node>();
                node.SetNodeProperties(damageInstructions.RequiredToolType, i == 0);
                this.nodes.Add(node);
            }
        }

        /*Calculate dash positions between pairs of nodes where we will generate dash objects */
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


        /*                        STUFFING GAME                         */
        /* ----------------------------------------------------------- */



    }
}
