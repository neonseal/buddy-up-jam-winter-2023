using UnityEngine;
using UnityEngine.UI;

public class ChecklistLineItem : MonoBehaviour {
    [SerializeField] private Image checkmark;
    [SerializeField] private TMPro.TMP_Text lineItemText;
    private PlushieDamageGO[] plushieDamageSet;
    public PlushieDamageType PlushieDamageType { get; private set; }

    public void SetParameters(string lineItemText, PlushieDamageGO[] plushieDamageSet) {
        this.lineItemText.text = lineItemText;
        this.plushieDamageSet = plushieDamageSet;
        this.PlushieDamageType = plushieDamageSet[0].GetInitialDamageType();
    }

    public void CompleteLineItem() {
        Color tempColor = checkmark.color;
        tempColor.a = 1f;
        checkmark.color = tempColor;
    }
}
