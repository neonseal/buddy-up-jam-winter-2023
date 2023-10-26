using DG.Tweening;
using GameState;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Workspace Controller
/// 
/// This class maintains control of the client plushie list, listening for requests to load
/// in a new client/plushie and requests to send the plushie off after repairs are complete.
/// </summary>
namespace PlayArea {
    public class Workspace : MonoBehaviour {
        /* Private Member Variables */
        // Serialized fields
        [Header("Plushie Loading Elements")]
        [SerializeField] private Plushie[] plushieList;

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

        // Private fields
        private int currentPlushieIndex = 0;
        private Plushie currentPlushie;
         // Name of the game finished/credit scene, used by LoadScene as the String parameter for scene name
        private const String _GAME_FINISH_SCENE_NAME = "TestEndScene";

        /* Public Event Actions */
        public static event Action<Plushie> OnClientPlushieloaded;

        private void Awake() {
            InitializeWorkspace();
        }

        public void InitializeWorkspace() {
            DOTween.Init();

            WorkspaceEmptyState.OnNextClientRequested += TryCallInNextClientPlushie;
        }

        // Handle the request to call in next plushie in PlushieList
        // If there are more plushies to be repaired, load in next pluishei
        // else, switch to game finish scene
        private void TryCallInNextClientPlushie() {
            /*
            if (currentPlushieIndex < plushieList.Length) {
                StartCoroutine(StartLoadPlushieRoutine());
            }
            else {
                SceneManager.LoadScene(_GAME_FINISH_SCENE_NAME, LoadSceneMode.Single);
            }
            */
            SceneManager.LoadScene(_GAME_FINISH_SCENE_NAME, LoadSceneMode.Single);
        }

        IEnumerator StartLoadPlushieRoutine() {

            Sequence loadPlushieSequence = DOTween.Sequence();

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

            currentPlushieIndex++;
        }

        /* Public Properties */
        public int CurrentPlushieIndex { get => currentPlushieIndex; }
        public Plushie CurrentPlushie { get => currentPlushie; }
    }
}
