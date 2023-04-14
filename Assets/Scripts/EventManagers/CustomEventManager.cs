using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GameData;

public class CustomEventManager : MonoBehaviour {
    //Singleton setup
    public static CustomEventManager Current {
        get {
            if (current == null)
                current = FindObjectOfType(typeof(CustomEventManager)) as CustomEventManager;

            return current;
        }
        set {
            current = value;
        }
    }
    private static CustomEventManager current;

    private void Awake() {
        current = this;
    }

    // High-level Game State Events
    public event Action onGameStart;
    public void GameStart() {
        if (this.onGameStart != null) {
            this.onGameStart();
        }
    }

    // Dialog Trigger
    public event Action<PlushieScriptableObject> onTriggerDialogue;
    public void TriggerDialogue(PlushieScriptableObject plushieScriptableObject) {
        if (this.onTriggerDialogue != null) {
            this.onTriggerDialogue(plushieScriptableObject);
        }
    }

    // Tutorial Sequence
    public event Action<TutorialSequenceScriptableObject> onStartTutorialSequence;
    public void StartTutorialSequence(TutorialSequenceScriptableObject TutorialSequenceScriptableObject) {
        if (this.onStartTutorialSequence != null) {
            this.onStartTutorialSequence(TutorialSequenceScriptableObject);
        }
    }


    // Music/Sound controls
    public event Action onPauseMusic;
    public void PauseMusic() {
        if (this.onPauseMusic != null) {
            this.onPauseMusic();
        }
    }

    public event Action onChangeVolume;
    public void ChangeVolume() {
        if (this.onChangeVolume != null) {
            this.onChangeVolume();
        }
    }

    // Mouse Held event
    public event Action<bool> onMouseHoldStatusToggle;
    public void mouseHoldStatusToggle(bool mouseHeld) {
        if (this.onMouseHoldStatusToggle != null) {
            this.onMouseHoldStatusToggle(mouseHeld);
        }
    }
}
