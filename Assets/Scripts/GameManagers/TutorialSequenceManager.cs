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
    private TutorialSequence currentTutorialSequence;
    private int currentStep;

    [Header("UI Elements")]
    [SerializeField] private GameObject tutorialSequenceParent;
    [SerializeField] private GameObject tutorialStepPrefab;

    [Header("Test Elements")]
    [SerializeField] private Button startTutorialButton;
    [SerializeField] private TutorialSequence testSequence;
    
    private void Awake() {
        tutorialActive = false;
        currentStep = 0;

        CustomEventManager.Current.onStartTutorialSequence += StartTutorialSequence;
        startTutorialButton.onClick.AddListener(delegate { StartTutorialSequence(testSequence); });
    }

    public void StartTutorialSequence(TutorialSequence newTutorialSequence) {
        if (newTutorialSequence == null) {
            Debug.LogError("Tutorial Sequence Scriptable Object must not be null");
        }

        this.currentTutorialSequence = newTutorialSequence;

        tutorialStepPrefab.GetComponent<TMP_Text>().text = newTutorialSequence.sentences[0];
        Instantiate(tutorialStepPrefab, new Vector3(0, 0), Quaternion.identity, tutorialSequenceParent.transform);
    }
}
