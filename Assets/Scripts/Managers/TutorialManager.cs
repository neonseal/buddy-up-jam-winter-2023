using PlayArea;
using UnityEngine;

namespace Dialogue {
    public class TutorialManager : MonoBehaviour {
        [Header("Static Progress Trackers")]
        private bool tutorialActive;

        [Header("Tutorial UI Elements")]
        [SerializeField] private GameObject parentObject;
        [SerializeField] private TutorialDialogueBox tutorialDialoguePrefab;
        [SerializeField] private GameObject tutorialArrowPrefab;
        [SerializeField] private Checklist checklist;
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
            tutorialActive = false;
            currentStepIndex = 0;

            TutorialDialogueBox.OnContinueButtonPressed += ContinueTutorialSequence;
        }

        public void ContinueTutorialSequence() {
            currentStepIndex++;

            // Exit tutorial if we have exceeded the turn count
            if (currentStepIndex >= currentTutorialSequence.tutorialSteps.Length) {
                tutorialActive = false;
                DestroyImmediate(activeTutorialDialogue.gameObject);
                if (activeTutorialArrow != null) {
                    DestroyImmediate(activeTutorialArrow);
                }
            } else {
                // Update dialogue box and show next step
                CreateOrUpdateTutorialDialogue();
                CreateOrUpdateTutorialArrow();

                // Show checklist if tutorial requires interacting with it
                if (currentTutorialStep.requiredContinueAction == TutorialActionRequiredContinueType.CompleteJob) {
                    checklist.ShowHideChecklist(true);
                }
            }
        }

        public void StartTutorialSequence(TutorialSequenceScriptableObject tutorialSequence) {
            tutorialActive = true;

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

        public TutorialActionRequiredContinueType GetRequiredContinueAction() {
            if (currentTutorialStep == null) {
                return TutorialActionRequiredContinueType.None;
            }

            return currentTutorialStep.requiredContinueAction;
        }


        private void CreateOrUpdateTutorialDialogue() {
            currentTutorialStep = currentTutorialSequence.tutorialSteps[currentStepIndex];
            Vector2 dialoguePosition = currentTutorialStep.tutorialDialogueLocation;
            string stepText = currentTutorialStep.stepText;

            // Create or update the tutorial dialogue box
            if (activeTutorialDialogue == null) {
                activeTutorialDialogue = Instantiate(tutorialDialoguePrefab, this.parentObject.transform, false);
                /*activeTutorialDialogue = Instantiate(tutorialDialoguePrefab);
                activeTutorialDialogue.transform.SetParent(this.transform);*/
            }

            // If there is a required continue action (i.e. ring bell, pickup tool), hide continue button
            activeTutorialDialogue.GetComponent<TutorialDialogueBox>().EnableDisableContinueButton(currentTutorialStep.requiredContinueAction == TutorialActionRequiredContinueType.None);

            // Update tutorial text and location
            activeTutorialDialogue.SetTutorialStepTexts(stepText);
            activeTutorialDialogue.transform.localPosition = dialoguePosition;
        }

        private void CreateOrUpdateTutorialArrow() {
            // If no arrow required for this step, destroy arrow if ative and return
            if (currentTutorialStep.requiresArrow) {
                Vector2 arrowPosition = currentTutorialStep.tutorialArrowLocation;
                int arrowZRotation = currentTutorialStep.arrowZRotationValue;

                if (activeTutorialArrow == null) {
                    activeTutorialArrow = Instantiate(tutorialArrowPrefab, this.parentObject.transform, false);
                    /*activeTutorialArrow = Instantiate(tutorialArrowPrefab);
                    activeTutorialArrow.transform.SetParent(this.transform);*/
                }

                activeTutorialArrow.transform.localPosition = arrowPosition;
                activeTutorialArrow.transform.localRotation = Quaternion.Euler(0, 0, arrowZRotation);
            } else if (activeTutorialArrow) {
                Destroy(activeTutorialArrow);
            }
        }

        public bool TutorialActive { get => tutorialActive; }
    }
}

