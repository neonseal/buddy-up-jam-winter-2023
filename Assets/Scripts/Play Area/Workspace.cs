using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; 
/* User-defined Namespaces */
using GameState;

/// <summary>
/// Workspace Controller
/// 
/// This class maintains control of the client plushie list, listening for requests to load
/// in a new client/plushie and requests to send the plushie off after repairs are complete.
/// </summary>
namespace PlayArea {
    public class Workspace : MonoBehaviour {
        /* Private Member Variables */
        [Header("Plushie Loading Elements")]
        [SerializeField] private Plushie[] plushieList;
        private int currentPlushieIndex = -1;
        private Plushie currentPlushie;

        [Header("Plushie Animation Elements")]
        [SerializeField] private Ease moveEaseType;
        [SerializeField] private float moveDuration;
        [SerializeField] private float animateDuration;
        [SerializeField] private float squashedX;
        [SerializeField] private float squashedY;
        [SerializeField] private int punchVibrato;
        [SerializeField] private float punchElasticity;


        /* Public Event Actions */
        public static event Action OnClientloaded;

        private void Awake() {
            InitializeWorkspace();
        }

        public void InitializeWorkspace() {
            DOTween.Init();

            WorkspaceEmptyState.OnNextClientRequested += LoadNextClientPlushie;
        }

        private void LoadNextClientPlushie() {
            Sequence loadPlushieSequence = DOTween.Sequence();
            currentPlushieIndex++;

            // Load next plushie prefab if there are any left
            if (currentPlushieIndex < plushieList.Length) {
                currentPlushie = Instantiate(plushieList[currentPlushieIndex], new Vector3(0,20,0), Quaternion.identity, this.transform);
                Vector3 punchVector = new Vector3(squashedX, squashedY, 0);

                // Set up tweening animation
                loadPlushieSequence.Append(currentPlushie.transform.DOLocalMoveY(0, moveDuration).SetEase(moveEaseType));
                loadPlushieSequence.Append(currentPlushie.transform.DOPunchScale(punchVector, animateDuration, punchVibrato, punchElasticity));

                // Send task complete event
                OnClientloaded?.Invoke();
            } else {
                // No plushies left -> transition to end state
            }

        }

        /* Public Properties */
        public int CurrentPlushieIndex { get => currentPlushieIndex; }
        public Plushie CurrentPlushie { get => currentPlushie; }
    }
}
