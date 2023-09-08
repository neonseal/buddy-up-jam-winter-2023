using UnityEngine;
using UnityEngine.UI;

public class ChecklistLineItem : MonoBehaviour {
    [SerializeField] private Image checkmark;
    [SerializeField] private Sprite checkmarkSprite;
    [SerializeField] private TMPro.TMP_Text lineItemText;
    private PlushieDamageGO[] plushieDamageSet;
    public PlushieDamageType PlushieDamageType { get; private set; }

    public void SetParameters(string lineItemText, PlushieDamageGO[] plushieDamageSet) {
        this.lineItemText.text = lineItemText;
        this.plushieDamageSet = plushieDamageSet;
        this.PlushieDamageType = plushieDamageSet[0].GetInitialDamageType();
    }

    public void CompleteLineItem() {
        checkmark.sprite = checkmarkSprite;
    }
}
