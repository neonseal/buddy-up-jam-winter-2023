using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DamageScripts;

public class CustomEventManager : MonoBehaviour
{   
    //Singleton setup
    public static CustomEventManager current;

    private void Awake() {
        current = this;
    }

    public event Action<PlushieDamage, DamageType> onDamageGeneration;
    public void damageGenerationEvent(PlushieDamage plushieDamage, DamageType damageType) {
        if (this.onDamageGeneration != null) {
            this.onDamageGeneration(plushieDamage, damageType);
        }
    }

    public event Action<PlushieDamage, DamageType> onRepair;
    public void repairEvent(PlushieDamage plushieDamage, DamageType damageType) {
        if (this.onRepair != null) {
            this.onRepair(plushieDamage, damageType);
        }
    }

    public event Action<PlushieDamage> onRepairCompletion;
    public void repairCompletionEvent(PlushieDamage plushieDamage) {
        if (this.onRepairCompletion != null) {
            this.onRepairCompletion(plushieDamage);
        }
    }
}
 