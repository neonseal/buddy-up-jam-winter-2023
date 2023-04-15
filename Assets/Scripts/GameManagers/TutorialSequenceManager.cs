using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;
using TMPro;
using UnityEngine.UI;

public class TutorialSequenceManager : MonoBehaviour {
    [Header("Public Status Fields")]
    private bool tutorialActive;
    public bool TutorialActive {
        get { return tutorialActive; }
    }

    private bool hasRequiredTool;
    public bool HasRequiredTool {
        get { return hasRequiredTool; }
    }


    private ToolType tutorialToolType;
    public ToolType TutorialToolType {
        get { return tutorialToolType; }
    }

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

    [Header("Test Elements")]
    [SerializeField] private Button startTutorialButton;
    [SerializeField] private TutorialSequenceScriptableObject testSequence;

    private void Awake() {
        tutorialActive = false;
        currentStepIndex = 0;

        CustomEventManager.Current.onStartTutorialSequence += StartTutorialSequence;
        startTutorialButton.onClick.AddListener(delegate { StartTutorialSequence(testSequence); });
    }

    public void StartTutorialSequence(TutorialSequenceScriptableObject newTutorialSequence) {
        if (newTutorialSequence == null) {
            Debug.LogError("Tutorial Sequence Scriptable Object must not be null");
        }

        // Start new sequence
        this.tutorialActive = true;
        this.currentTutorialSequence = newTutorialSequence;
        this.currentStepIndex = 0;
        if (newTutorialSequence.hasRequiredTool) {
            this.tutorialToolType = newTutorialSequence.requiredToolType;
        }

        CreateOrUpdateTutorialSequenceDialogue();

        if (newTutorialSequence.tutorialArrowLocations[this.currentStepIndex].x != -1) {
            CreateOrUpdateTutorialSequenceArrow();
        }

        this.continueButton = this.tutorialDialogue.GetComponentInChildren<Button>();
        this.continueButton.onClick.AddListener(HandleContinueButtonClick);
    }

    private void CreateOrUpdateTutorialSequenceDialogue() {
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
        // Gather position and rotation of current step arrow
        Vector2 arrowPosition = this.currentTutorialSequence.tutorialArrowLocations[this.currentStepIndex];
        int arrowRotationZ = this.currentTutorialSequence.arrowZRotationValue[this.currentStepIndex];

        if (this.tutorialArrow == null) {
            this.tutorialArrow = Instantiate(tutorialArrowPrefab, arrowPosition, Quaternion.Euler(0,0,arrowRotationZ), this.tutorialDialogue.transform);
            this.tutorialArrow.transform.SetParent(this.parentObject.transform, false);
        }

        this.tutorialArrow.transform.localPosition = arrowPosition;
        this.tutorialArrow.transform.localRotation = Quaternion.Euler(0, 0, arrowRotationZ);
    }

    private void HandleContinueButtonClick() {
        this.currentStepIndex++;

        if (this.currentStepIndex >= this.currentTutorialSequence.stepTextSet.Length) {
            Destroy(this.tutorialDialogue);
            if (this.tutorialArrow != null) {
                Destroy(this.tutorialArrow);
            }
        } else {
            DisplayNextTutorialStep();
        }
    }

    private void DisplayNextTutorialStep() {
        CreateOrUpdateTutorialSequenceDialogue();

    }
}
