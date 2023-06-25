using System;
using System.Collections.Generic;
using UnityEngine;

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
        private DamageInstructions damageInstructions;

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
        private Node[] nodes;

        /* Mending Game Events */
        public static event Action OnMendingGameComplete;

        private void Awake() {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        /*                      COMMON FUNCTIONS                       */
        /* ----------------------------------------------------------- */
        // Primary function invoked by the Mending Game Manager, responsible for parsing the 
        // damage instructions and rendering the appropriate game to the lens
        public void GenerateMendingGame(DamageInstructions damageInstructions) {
            switch (damageInstructions.PlushieDamageType) {
                case PlushieDamageType.SmallRip:                    
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

        }



        /*                        CUTTING GAME                         */
        /* ----------------------------------------------------------- */




        /*                        STUFFING GAME                         */
        /* ----------------------------------------------------------- */



    }
}
