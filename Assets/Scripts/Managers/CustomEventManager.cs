using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DamageScripts;

public class CustomEventManager : MonoBehaviour
{
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

    private void Awake()
    {
        current = this;
    }

    // High-level Game State Events
    public event Action onGameStart;
    public void GameStart()
    {
        if (this.onGameStart != null)
        {
            this.onGameStart();
        }
    }

    // Dialog Trigger
    public event Action<Dialogue> onTriggerDialogue;
    public void TriggerDialogue(Dialogue dialogue)
    {
        if (this.onTriggerDialogue != null)
        {
            this.onTriggerDialogue(dialogue);
        }
    }

    // Damage Management Events
    public event Action<PlushieDamage, DamageType> onDamageGeneration;
    public void damageGenerationEvent(PlushieDamage plushieDamage, DamageType damageType)
    {
        if (this.onDamageGeneration != null)
        {
            this.onDamageGeneration(plushieDamage, damageType);
        }
    }

    public event Action<PlushieDamage, DamageType> onRepair;
    public void repairEvent(PlushieDamage plushieDamage, DamageType damageType)
    {
        if (this.onRepair != null)
        {
            this.onRepair(plushieDamage, damageType);
        }
    }

    public event Action<PlushieDamage> onRepairCompletion;
    public void repairCompletionEvent(PlushieDamage plushieDamage)
    {
        if (this.onRepairCompletion != null)
        {
            this.onRepairCompletion(plushieDamage);
        }
    }

    // Music/Sound controls
    public event Action onPauseMusic;
    public void PauseMusic()
    {
        if (this.onPauseMusic != null)
        {
            this.onPauseMusic();
        }
    }

    public event Action onChangeVolume;
    public void ChangeVolume()
    {
        if (this.onChangeVolume != null)
        {
            this.onChangeVolume();
        }
    }

    // Plushie spawning event
    public event Action onPlushieInitialization;
    public void initializePlushieEvent()
    {
        if (this.onPlushieInitialization != null)
        {
            this.onPlushieInitialization();
        }
    }

    // Plushie overall repair complete event
    public event Action onPlushieOverallRepairCompletion;
    public void plushieOverallRepairCompletionEvent()
    {
        if (this.onPlushieOverallRepairCompletion != null)
        {
            this.onPlushieOverallRepairCompletion();
        }
    }

    // Plushie deletion event
    public event Action onPlushieDeletionRequest;
    public void plushieDeletionEvent()
    {
        if (this.onPlushieDeletionRequest != null) { }
        {
            this.onPlushieDeletionRequest();
        }
    }

    // Mouse Held event
    public event Action<bool>onMouseHoldStatusToggle;
    public void mouseHoldStatusToggle(bool mouseHeld) {
        if (this.onMouseHoldStatusToggle != null) {
            this.onMouseHoldStatusToggle(mouseHeld);
        } 
    }
}
