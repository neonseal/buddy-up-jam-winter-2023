using PlayArea;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UserInterface;

/// <summary>
/// Dialogue Canvas Manager
/// 
/// High-level manager for the client dialogue and tutorial canvas elements, 
/// responsible for activating/deactivating UI elements and reacting to 
/// state events when they are thrown
/// </summary>
namespace Dialogue {
    public class DialogueCanvasManager : MonoBehaviour {
        private Plushie currentPlushie;
        [SerializeField] private ClientDialogueSet clientDialogue;
        [SerializeField] private TutorialSequenceScriptableObject welcomeTutorialSequence;

        /* Private Member Variables */
        [Header("Dialogue/Tutorial Manager Elements")]
        [SerializeField] private ClientDialogueManager clientDialogueManager;
        [SerializeField] private TutorialManager tutorialManager;

        private void Awake() {
            SetupDialogueCanvasManager();
        }

        public void SetupDialogueCanvasManager() {
            MainMenuCanvasManager.OnStartButtonPressed += StartWelcomeTutorial;
            Workspace.OnClientPlushieloaded += HandlePlushieLoadEvent;
            ClientDialogueManager.OnClientDialogueComplete += CheckForPlushieTutorial;
        }

        private void StartWelcomeTutorial() {
            StartCoroutine(StartWelcomeTutorialRoutine());
        }

        IEnumerator StartWelcomeTutorialRoutine() {
            // Pause briefly to allow animation to finish before sending event
            yield return new WaitForSeconds(.25f);
            tutorialManager.StartTutorialSequence(welcomeTutorialSequence);
        }

        private void HandlePlushieLoadEvent(Plushie currentPlushie) {
            this.currentPlushie = currentPlushie;
            Assert.IsNotNull(currentPlushie.IssueDialogue, "Client issue dialogue is undefined!");
            Debug.Log(currentPlushie.IssueDialogue.Name);
            clientDialogueManager.StartDialogueSequence(currentPlushie.IssueDialogue);
        }

        private void CheckForPlushieTutorial() {
            if (currentPlushie.HasTutorialDialogue) {
                Assert.IsNotNull(currentPlushie.TutorialSequenceScriptableObject, "Client plushie states it has a tutoril, but scriptable is null!");
                tutorialManager.StartTutorialSequence(currentPlushie.TutorialSequenceScriptableObject);
            }
        }
    }
}

