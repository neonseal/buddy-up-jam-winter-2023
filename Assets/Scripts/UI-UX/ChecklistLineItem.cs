using UnityEngine;
using UnityEngine.UI;

public class ChecklistLineItem : MonoBehaviour {
    [SerializeField] private Image checkmark;
    [SerializeField] private TMPro.TMP_Text lineItemText;
    private PlushieDamageGO[] plushieDamageSet;

    public void SetParameters(string lineItemText, PlushieDamageGO[] plushieDamageSet) {
        this.lineItemText.text = lineItemText;
        this.plushieDamageSet = plushieDamageSet;
    }
}
