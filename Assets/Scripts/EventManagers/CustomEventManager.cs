using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GameData;

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
    public event Action<PlushieDamage, DamageType> onGenerateDamage;
    public void generateDamage(PlushieDamage plushieDamage, DamageType damageType)
    {
        if (this.onGenerateDamage != null)
        {
            this.onGenerateDamage(plushieDamage, damageType);
        }
    }

    public event Action<PlushieDamage, DamageType> onStartRepairMiniGame;
    public void startRepairMiniGame(PlushieDamage plushieDamage, DamageType damageType) {
        if (this.onStartRepairMiniGame != null) {
            this.onStartRepairMiniGame(plushieDamage, damageType);
        }
    }

    public event Action<PlushieDamage, DamageType> onRepairDamage_Partial;
    public void repairDamage_Partial(PlushieDamage plushieDamage, DamageType damageType)
    {
        if (this.onRepairDamage_Partial != null)
        {
            this.onRepairDamage_Partial(plushieDamage, damageType);
        }
    }

    public event Action<PlushieDamage> onRepairDamage_Complete;
    public void repairDamage_Complete(PlushieDamage plushieDamage)
    {
        if (this.onRepairDamage_Complete != null)
        {
            this.onRepairDamage_Complete(plushieDamage);
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
    public event Action<PlushieScriptableObject> onGeneratePlushie;
    public void generatePlushie(PlushieScriptableObject plushieScriptableObject)
    {
        if (this.onGeneratePlushie != null)
        {
            this.onGeneratePlushie(plushieScriptableObject);
        }
    }

    // Plushie overall repair complete event
    public event Action onFinishPlushieRepair;
    public void finishPlushieRepair()
    {
        if (this.onFinishPlushieRepair != null)
        {
            this.onFinishPlushieRepair();
        }
    }

    // Plushie deletion event
    public event Action onDeletePlushie;
    public void deletePlushie()
    {
        if (this.onDeletePlushie != null) { }
        {
            this.onDeletePlushie();
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
