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
        private SpriteRenderer spriteRenderer;

        [Header("Game Component Collections")]
        private List<Node> nodes;

        /* Mending Game Events */
        public static event Action OnMendingGameComplete;

        private void Awake() {
            spriteRenderer = GetComponent<SpriteRenderer>();
            nodes = new List<Node>();
        }

        /*                      COMMON FUNCTIONS                       */
        /* ----------------------------------------------------------- */
        // Primary function invoked by the Mending Game Manager, responsible for parsing the 
        // damage instructions and rendering the appropriate game to the lens
        public void GenerateMendingGame(DamageInstructrionsScriptableObject damageInstructions) {
            this.damageInstructions = damageInstructions; 

            switch (damageInstructions.PlushieDamageType) {
                case PlushieDamageType.SmallRip:
                    GenerateSewingGame(damageInstructions.TargetLocations);
                    break;
                case PlushieDamageType.LargeRip:
                    break;
                case PlushieDamageType.WornStuffing:
                    break;
                case PlushieDamageType.MissingStuffing:
                    break;
            }
        }


        /*                        SEWING GAME                          */
        /* ----------------------------------------------------------- */
        private void GenerateSewingGame(Vector2[] targetLocations) {
            // Ensure we are using the appropriate material
            if (spriteRenderer.material != defaultMaterial) {
                spriteRenderer.material = defaultMaterial;
            }

            // Generate sewing target nodes based on input target locations
            for (int i = 0; i < targetLocations.Length; i++) {
                Vector3 position = new Vector3(targetLocations[i].x, targetLocations[i].y);
                GameObject gameObject = Instantiate(mendingTargetPrefab, this.transform, false);
                gameObject.transform.localPosition = position;
                Node node = gameObject.GetComponent<Node>();
                node.SetNodeProperties(damageInstructions.RequiredToolType, i == 0);
                nodes.Add(node);
            }

        }



        /*                        CUTTING GAME                         */
        /* ----------------------------------------------------------- */




        /*                        STUFFING GAME                         */
        /* ----------------------------------------------------------- */



    }
}
