using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bell : MonoBehaviour {
    private Button _bellButton;
    private AudioSource audioSource;
    private bool tutorialActionRequired;

    private void Awake() {
        tutorialActionRequired = false;
        this._bellButton = GetComponentInChildren<Button>();
        this.audioSource = GetComponent<AudioSource>(); 
        TutorialSequenceEventManager.Current.onRequireRingBellContinueAction += () => { tutorialActionRequired = true; };
    }

    private void Start() {
        this._bellButton.onClick.AddListener(this.bellClick);
    }

    private void bellClick() {
        audioSource.Play();

        // Check if tutorial interaction is required
        if (tutorialActionRequired) {
            tutorialActionRequired = false;
            StartCoroutine(TutorialSequenceEventManager.Current.HandleTutorialRequiredActionCompletion());
        }

        PlushieLifeCycleEventManager.Current.ringBell();
    }
}
