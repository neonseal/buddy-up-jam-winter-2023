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

    private ToolType tutorialToolType;
    public ToolType TutorialToolType {
        get { return tutorialToolType; }
    }

    [Header("Tutorial Sequence Progress Trackers")]
    private TutorialSequenceScriptableObject currentTutorialSequenceScriptableObject;
    private int currentStepIndex;

    [Header("UI Elements")]
    [SerializeField] private GameObject parentObject;
    [SerializeField] private GameObject tutorialStepPrefab;
    private GameObject tutorialDialogue;

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

        this.currentTutorialSequenceScriptableObject = newTutorialSequence;

        // Update tutorial step text
        tutorialStepPrefab.GetComponentInChildren<TMP_Text>().text = newTutorialSequence.sentences[this.currentStepIndex];

        // Instantiate and position tutorial step dialogue box under UI parent
        this.tutorialDialogue = Instantiate(tutorialStepPrefab, new Vector3(0, 0), Quaternion.identity);
        this.tutorialDialogue.transform.SetParent(parentObject.transform, false);
    }
}
