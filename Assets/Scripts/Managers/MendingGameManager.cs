using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
/* User-defined Namespaces */
using Scriptables.DamageInstructions;

/// <summary>
/// Mending Game Manager
/// 
/// Responsible for handling selection of a plushie's damage, invoking the mending game
/// generator to create the appropriate mending game type.
/// </summary>
namespace MendingGames {
    public enum MendingGameType {
        Sewing, 
        Cutting, 
        Stuffing
    }

    public class MendingGameManager : MonoBehaviour {
        [Header("Mending Game Progress Elements")]
        [SerializeField] private MendingGameGenerator mendingGameGenerator;
        private bool gameInProgress; 

        [Header("Mending Game UI Elements")]
        [SerializeField] private GameObject magnifyingGlass;

        [Header("Magnifying Glass Animation Elements")]
        [SerializeField] private float duration;
        [SerializeField] private Ease easeType;
        private Vector3 startingLocation;
        private Vector3 centerLocation;


        private void Awake() {
            DOTween.Init();

            gameInProgress = false;
            startingLocation = magnifyingGlass.transform.localPosition;
            centerLocation = new Vector3(5.5f, -10, 0);

            PlushieDamageGO.OnPlushieDamageClicked += HandleDamageClick;
        }

        private void HandleDamageClick(DamageInstructrionsScriptableObject damageInstructions) {
            gameInProgress = true;
            mendingGameGenerator.GenerateMendingGame(damageInstructions);
            //magnifyingGlass.transform.DOMove(centerLocation, duration).SetEase(easeType);
        }


        /* Public Properties */
        public bool GameInProgress { get => gameInProgress; }
    }
}

