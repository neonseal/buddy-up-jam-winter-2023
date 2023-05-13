using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;
using TMPro;
using UnityEngine.UI;

namespace TutorialSequence {
    // Some tutorial steps require a specifc action to progress the sequence
    public enum TutorialActionRequiredContinueType {
        None,
        RingBell,
        SelectDamage,
        SelectTool,
        DropTool,
        CompleteRepair,
        CompleteJob
    }

    public class TutorialSequenceManager : MonoBehaviour {
        [Header("Public Status Fields")]
        internal static bool tutorialActive;
        internal static bool hasRequiredTool;
        internal static ToolType tutorialToolType;

        [Header("Tutorial Sequence Progress Trackers")]
        private TutorialSequenceScriptableObject currentTutorialSequence;
        private int currentStepIndex;

        [Header("UI Elements")]
        [SerializeField] private GameObject parentObject;
        [SerializeField] private GameObject tutorialStepPrefab;
        [SerializeField] private GameObject tutorialArrowPrefab;
        private GameObject tutorialDialogue;
        private GameObject tutorialArrow;
        private Button continueButton;


        private void Awake() {
            tutorialActive = false;
            currentStepIndex = 0;

            TutorialSequenceEventManager.Current.onStartTutorialSequence += StartTutorialSequence;
            TutorialSequenceEventManager.Current.onContinueTutorialSequence += ContinueTutorialSequence;
        }

        public void StartTutorialSequence(TutorialSequenceScriptableObject newTutorialSequence) {
            Destroy(this.tutorialDialogue);
            Destroy(this.tutorialArrow);

            if (newTutorialSequence == null) {
                Debug.LogError("Tutorial Sequence Scriptable Object must not be null");
            }

            // Start new sequence
            TutorialSequenceManager.tutorialActive = true;
            this.currentTutorialSequence = newTutorialSequence;
            this.currentStepIndex = 0;

            if (newTutorialSequence.hasRequiredTool) {
                TutorialSequenceManager.hasRequiredTool = true;
                TutorialSequenceManager.tutorialToolType = newTutorialSequence.requiredToolType;
            }

            CreateOrUpdateTutorialSequenceStep();
            CreateOrUpdateTutorialSequenceArrow();


            this.continueButton = this.tutorialDialogue.GetComponentInChildren<Button>();
            this.continueButton.onClick.AddListener(ContinueTutorialSequence);

            SetContinueRequirements();
        }

        private void CreateOrUpdateTutorialSequenceStep() {
            // Gather position and text for current step
            Vector2 tutorialPosition = this.currentTutorialSequence.tutorialDialogueLocations[this.currentStepIndex];

            // Update tutorial step text
            string tutorialStepText = this.currentTutorialSequence.stepTextSet[this.currentStepIndex];

            // Instantiate and position tutorial step dialogue box under UI parent
            if (this.tutorialDialogue == null) {
                tutorialStepPrefab.GetComponentInChildren<TMP_Text>().text = tutorialStepText;
                this.tutorialDialogue = Instantiate(tutorialStepPrefab, tutorialPosition, Quaternion.identity);
                this.tutorialDialogue.transform.SetParent(parentObject.transform, false);
            } else {
                this.tutorialDialogue.GetComponentInChildren<TMP_Text>().text = tutorialStepText;
            }

            // Set local position
            this.tutorialDialogue.transform.localPosition = tutorialPosition;


        }

        private void CreateOrUpdateTutorialSequenceArrow() {
            // Destroy arrow from previous step if no longer needed
            if (this.currentStepIndex > 0 &&
                this.tutorialArrow != null &&
                this.currentTutorialSequence.tutorialArrowLocations[this.currentStepIndex].x == -1) {
                Destroy(this.tutorialArrow);
                return;
            }

            if (this.currentTutorialSequence.tutorialArrowLocations[this.currentStepIndex].x == -1) {
                return;
            }

            // Gather position and rotation of current step arrow
            Vector2 arrowPosition = this.currentTutorialSequence.tutorialArrowLocations[this.currentStepIndex];
            int arrowRotationZ = this.currentTutorialSequence.arrowZRotationValue[this.currentStepIndex];

            if (this.tutorialArrow == null) {
                this.tutorialArrow = Instantiate(tutorialArrowPrefab, arrowPosition, Quaternion.Euler(0, 0, arrowRotationZ), this.tutorialDialogue.transform);
                this.tutorialArrow.transform.SetParent(this.parentObject.transform, false);
            }

            this.tutorialArrow.transform.localPosition = arrowPosition;
            this.tutorialArrow.transform.localRotation = Quaternion.Euler(0, 0, arrowRotationZ);

        }

        private void ContinueTutorialSequence() {
            this.currentStepIndex++;

            if (!this.continueButton.enabled) {
                this.continueButton.gameObject.SetActive(true);
            }

            if (this.currentStepIndex >= this.currentTutorialSequence.stepTextSet.Length) {
                Destroy(this.tutorialDialogue);
                if (this.tutorialArrow != null) {
                    Destroy(this.tutorialArrow);
                }

                // Reset public fields
                TutorialSequenceManager.tutorialActive = false;
                TutorialSequenceManager.hasRequiredTool = false;
            } else {
                DisplayNextTutorialStep();
            }
        }

        private void DisplayNextTutorialStep() {
            CreateOrUpdateTutorialSequenceStep();
            CreateOrUpdateTutorialSequenceArrow();


            SetContinueRequirements();
        }

        private void SetContinueRequirements() {
            if (this.currentTutorialSequence.requiredContinueActionSet[this.currentStepIndex] == TutorialActionRequiredContinueType.None) {
                this.continueButton.gameObject.SetActive(true);
                return;
            }

            // Set continue button invisible
            this.continueButton.gameObject.SetActive(false);

            // Check which type of action is required
            switch (this.currentTutorialSequence.requiredContinueActionSet[this.currentStepIndex]) {
                case TutorialActionRequiredContinueType.RingBell:
                    TutorialSequenceEventManager.Current.RequireRingBellContinueAction();
                    break;
                case TutorialActionRequiredContinueType.SelectDamage:
                    TutorialSequenceEventManager.Current.RequireDamageSelectContinueAction();
                    break;
                case TutorialActionRequiredContinueType.SelectTool:
                    Debug.Assert(this.currentTutorialSequence.hasRequiredTool, "This tutorial sequence is not set to require a tool!");
                    TutorialSequenceEventManager.Current.RequireToolPickupContinueAction(this.currentTutorialSequence.requiredToolType);
                    break;
                case TutorialActionRequiredContinueType.CompleteRepair:
                    TutorialSequenceEventManager.Current.RequireRepairCompletionAction();
                    break;
                case TutorialActionRequiredContinueType.DropTool:
                    TutorialSequenceEventManager.Current.RequireToolDropContinueAction(this.currentTutorialSequence.requiredToolType);
                    break;
                case TutorialActionRequiredContinueType.CompleteJob:
                    TutorialSequenceEventManager.Current.RequireJobCompletionAction();
                    break;
            }
        }
    }
}