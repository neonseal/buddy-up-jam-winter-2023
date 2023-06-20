using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
/* User-defined Namespaces */
using Utility;

namespace Dialogue {
    public class TutorialManager : MonoBehaviour {
        private Helper helper;

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
            helper = new Helper();

            currentStepIndex = 0;

            // Setup dialogue event listeners
            DialogueCanvasManager.OnTutorialSequenceStart += StartTutorialSequence;
        }

        private void StartTutorialSequence(TutorialSequenceScriptableObject tutorialSequence) {
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
            currentTutorialStep = tutorialSequence.tutorialSteps[currentStepIndex];

            // Setup dialogue and arrow elements
            CreateOrUpdateTutorialDialogue();

            /*continueButton = this.activeTutorialDialogue.GetComponentInChildren<Button>();
            continueButton.onClick.AddListener(ContinueTutorialSequence);*/
        }

        private void CreateOrUpdateTutorialDialogue() {
            Vector2 dialoguePosition = currentTutorialStep.tutorialDialogueLocation;
            string stepText = currentTutorialStep.stepText;

            // Create or update the tutorial dialogue box
            if (activeTutorialDialogue == null) {
                GameObject tutorialDialogue = Instantiate(tutorialDialoguePrefab, dialoguePosition, Quaternion.identity);
                activeTutorialDialogue = tutorialDialogue.GetComponent<TutorialDialogueBox>();
            }

            activeTutorialDialogue.SetTutorialStepTexts(stepText);
            activeTutorialDialogue.transform.SetParent(this.transform);
            activeTutorialDialogue.transform.localPosition = dialoguePosition;
        }
    }
}

