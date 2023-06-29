using System;
using System.Collections.Generic;
using UnityEngine;
/* User-defined Namespaces */
using Scriptables.DamageInstructions; 

public class PlushieDamageGO : MonoBehaviour {
    // Initial damage type of the plushie damage
    [SerializeField] private DamageInstructrionsScriptableObject damageInstructions;
    [SerializeField]
    private List<GameObject> plushieDamagesDeletedOnCompletion;

    /* Damage life cycle events */
    public static event Action<DamageInstructrionsScriptableObject> OnPlushieDamageClicked;

    // Send out event when damage is clicked to 
    private void OnMouseDown() {
        OnPlushieDamageClicked?.Invoke(damageInstructions);
    }

    internal PlushieDamageType getInitialDamageType() {
        return damageInstructions.PlushieDamageType;
    }

    private void _finishRepairing() {
        foreach (GameObject gO in this.plushieDamagesDeletedOnCompletion) {
            Destroy(gO);
        }
    }
}