using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
/* User-defined Namespaces */
using Utility;

namespace Dialogue {
    public class TutorialManager : MonoBehaviour {
        [Header("Static Progress Trackers")]
        private static bool tutorialActive;

        [Header("Tutorial UI Elements")]
        [SerializeField] private GameObject tutorialDialoguePrefab;
        [SerializeField] private GameObject tutorialArrowPrefab;
        private TutorialDialogueBox activeTutorialDialogue;
        private GameObject activeTutorialArrow;

        [Header("Tutorial Sequence Progress Tracking")]
        private int currentStepIndex;
        private TutorialStep currentTutorialStep;
        private TutorialSequenceScriptableObject currentTutorialSequence;

        private void Awake() {
            SetupTutorialManager();
        }

        public void SetupTutorialManager() {
            TutorialManager.SetTutorialStatus(false);
            currentStepIndex = 0;

            // Setup dialogue event listeners
            DialogueCanvasManager.OnTutorialSequenceStart += StartTutorialSequence;
            TutorialDialogueBox.OnContinueButtonPressed += ContinueTutorialSequence;
        }

        private static void SetTutorialStatus(bool status) {
            TutorialManager.tutorialActive = status;
        }

        private void StartTutorialSequence(TutorialSequenceScriptableObject tutorialSequence) {
            TutorialManager.SetTutorialStatus(true);

            if (this.activeTutorialArrow || this.activeTutorialDialogue) {
                Destroy(this.activeTutorialDialogue);
                Destroy(this.activeTutorialArrow);
            }

            if (tutorialSequence == null) {
                Debug.LogError("Tutorial Sequence Scriptable Object must not be null");
                return;
            }

            // Initialize new tutorial sequence tracking
            currentStepIndex = 0;
            currentTutorialSequence = tutorialSequence;

            // Setup dialogue and arrow elements
            CreateOrUpdateTutorialDialogue();
            CreateOrUpdateTutorialArrow();
        }

        private void CreateOrUpdateTutorialDialogue() {
            currentTutorialStep = currentTutorialSequence.tutorialSteps[currentStepIndex];
            Vector2 dialoguePosition = currentTutorialStep.tutorialDialogueLocation;
            string stepText = currentTutorialStep.stepText;

            // Create or update the tutorial dialogue box
            if (activeTutorialDialogue == null) {
                GameObject tutorialDialogue = Instantiate(tutorialDialoguePrefab);
                activeTutorialDialogue = tutorialDialogue.GetComponent<TutorialDialogueBox>();
                activeTutorialDialogue.transform.SetParent(this.transform);
            }

            activeTutorialDialogue.SetTutorialStepTexts(stepText);
            activeTutorialDialogue.transform.localPosition = dialoguePosition;
        }

        private void CreateOrUpdateTutorialArrow() {
            // If no arrow required for this step, destroy arrow if ative and return
            if (currentTutorialStep.requiresArrow) {
                Vector2 arrowPosition = currentTutorialStep.tutorialArrowLocation;
                int arrowZRotation = currentTutorialStep.arrowZRotationValue;

                if (activeTutorialArrow == null) {
                    activeTutorialArrow = Instantiate(tutorialArrowPrefab);
                    activeTutorialArrow.transform.SetParent(this.transform);
                }

                activeTutorialArrow.transform.localPosition = arrowPosition;
                activeTutorialArrow.transform.localRotation = Quaternion.Euler(0, 0, arrowZRotation);
            } else if (activeTutorialArrow) {
                Destroy(activeTutorialArrow);
            }
        }

        private void ContinueTutorialSequence() {
            currentStepIndex++;

            // Exit tutorial if we have exceeded the turn count
            if (currentStepIndex >= currentTutorialSequence.tutorialSteps.Length) {
                TutorialManager.SetTutorialStatus(false);
                DestroyImmediate(activeTutorialDialogue.gameObject);
                if (activeTutorialArrow != null) {
                    DestroyImmediate(activeTutorialArrow);
                }
            } else {
                // Update dialogue box and show next step
                CreateOrUpdateTutorialDialogue();
                CreateOrUpdateTutorialArrow();
            }
        }

        public static bool TutorialActive { get => tutorialActive; }
    }
}

