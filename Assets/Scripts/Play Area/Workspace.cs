using DG.Tweening;
using GameState;
using MendingGames;
using System;
using System.Collections;
using UnityEngine;

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

        [Header("Game UI Elements")]
        [SerializeField] private Checklist checklist;

        /* Public Event Actions */
        public static event Action<Plushie> OnClientPlushieloaded;
         /* Public Event Actions */
        public static event Action<Plushie> OnClientPlushieRepairComplete;

        private void Awake() {
            InitializeWorkspace();
        }

        public void InitializeWorkspace() {
            DOTween.Init();

            WorkspaceEmptyState.OnNextClientRequested += LoadNextClientPlushie;
        }

        private void LoadNextClientPlushie() {
            StartCoroutine(StartLoadPlushieRoutine());
        }

        IEnumerator StartLoadPlushieRoutine() {

            Sequence loadPlushieSequence = DOTween.Sequence();
            currentPlushieIndex++;

            // Load next plushie prefab if there are any left
            if (currentPlushieIndex < plushieList.Length) {
                // Load plushie object into scene
                currentPlushie = Instantiate(plushieList[currentPlushieIndex], new Vector3(0, 20, 0), Quaternion.identity, this.transform);
                Vector3 punchVector = new Vector3(squashedX, squashedY, 0);

                // Update checklist with current plushie's repair steps
                PlushieDamageGO[] plushieDamages = currentPlushie.GetComponentsInChildren<PlushieDamageGO>();
                checklist.InitializeChecklistForPlushie(plushieDamages);

                // Set up tweening animation
                loadPlushieSequence.Append(currentPlushie.transform.DOLocalMoveY(0, moveDuration).SetEase(moveEaseType));
                loadPlushieSequence.Append(currentPlushie.transform.DOPunchScale(punchVector, animateDuration, punchVibrato, punchElasticity));

                // Pause briefly to allow animation to finish before sending event
                yield return new WaitForSeconds(1f);

                // Send task complete event
                OnClientPlushieloaded?.Invoke(currentPlushie);
            }
        }

        /* Public Properties */
        public int CurrentPlushieIndex { get => currentPlushieIndex; }
        public Plushie CurrentPlushie { get => currentPlushie; }
    }
}
