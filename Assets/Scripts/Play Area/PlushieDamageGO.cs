using System;
using System.Collections.Generic;
using UnityEngine;
/* User-defined Namespaces */
using PlayArea; 

[System.Serializable]
public struct DamageInstructions {
    [SerializeField] private string title;
    [SerializeField] private PlushieDamageType plushieDamageType;
    [SerializeField] private ToolType requiredToolType;
    [SerializeField] private Vector2[] targetLocations;

    /* Public Properties */
    public PlushieDamageType PlushieDamageType { get => plushieDamageType; }
    public ToolType RequiredToolType { get => requiredToolType; }
    public Vector2[] SewingTargetLocations { get => targetLocations; }

}

public class PlushieDamageGO : MonoBehaviour {
    // Initial damage type of the plushie damage
    [SerializeField] private DamageInstructions damageInstructions;
    [SerializeField]
    private List<GameObject> plushieDamagesDeletedOnCompletion;

    /* Damage life cycle events */
    public static event Action<DamageInstructions> OnPlushieDamageClicked;

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