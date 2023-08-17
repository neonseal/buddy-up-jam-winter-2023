using Scriptables.DamageInstructions;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlushieDamageGO : MonoBehaviour {
    // Initial damage type of the plushie damage
    [SerializeField] private DamageInstructrionsScriptableObject[] damageInstructions;
    [SerializeField]
    private List<GameObject> plushieDamagesDeletedOnCompletion;

    private PlushieDamageSM plushieDamageSM;

    /* Damage life cycle events */
    public static event Action<DamageInstructrionsScriptableObject[]> OnPlushieDamageClicked;
    public bool DamageRepairComplete { get; private set; }

    private void Awake() {
        DamageRepairComplete = false;
    }

    public void Start() {
        InitializeReferences();
    }

    // Initialize reference fields in this class
    private void InitializeReferences() {
        plushieDamageSM = GetComponent<PlushieDamageSM>();
        plushieDamageSM._plushieDamageGO = this;
    }

    // Send out event when damage is clicked to 
    public void OnMouseDown() {
        plushieDamageSM.SubscribeToMendingGame();
        OnPlushieDamageClicked?.Invoke(this.damageInstructions);
    }

    public PlushieDamageType GetInitialDamageType() {
        return this.damageInstructions[0].PlushieDamageType;
    }

    public string GenerateChecklistLineItem(int count) {
        string lineItemDescription = string.Empty;

        switch (GetInitialDamageType()) {
            case PlushieDamageType.SmallRip:
                lineItemDescription = count == 1 ? "Fix the small rip" : $"Fix all {count} small rips";
                break;
            case PlushieDamageType.LargeRip:
                lineItemDescription = count == 1 ? "Fix the large rip" : $"Fix all {count} large rips";
                break;
            case PlushieDamageType.MissingStuffing:
                lineItemDescription = count == 1 ? "Replace the missing stuffing" : $"Fix {count} areas with missing stuffing";
                break;
            case PlushieDamageType.WornStuffing:
                lineItemDescription = count == 1 ? "Fix all 4 areas with worn stuffing " : $"Fix all {count} large rips";
                break;
        }

        return lineItemDescription;
    }

    internal void FinishRepair() {
        foreach (GameObject gO in plushieDamagesDeletedOnCompletion) {
            Destroy(gO);
        }
        Destroy(this.gameObject);
    }
}