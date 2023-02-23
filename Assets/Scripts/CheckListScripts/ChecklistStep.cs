using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DamageScripts;

public class ChecklistStep : MonoBehaviour
{
    private Image checklistStepIcon;
    private TMP_Text checklistStepText;
    // PlushieDamage this step is associated with
    internal PlushieDamage plushieDamage;

    void Awake()
    {
        this.checklistStepIcon = this.gameObject.transform.Find("CheckBackground").gameObject.transform.Find("CheckmarkImage").GetComponent<Image>();
        this.checklistStepText = this.gameObject.transform.Find("StepText").GetComponent<TMP_Text>();
    }

    // Start is called before the first frame update
    void Start()
    {
        CustomEventManager.current.onRepairCompletion += completeStep;
        CustomEventManager.current.onRepair += repairDamage;
    }

    // Listener method - change the status of multistep repair to the next step
    public void repairDamage(PlushieDamage plushieDamage, DamageType damageType) {
        if (this.plushieDamage.Equals(plushieDamage)) {
            this.changeStepText(damageType);
        }
    }

    // Change step text to that of the parameter damage type
    public void changeStepText(DamageType damageType) {
        this.checklistStepText.text = DamageDictionary.damageInfoDictionary[damageType].damageChecklistMessage;
    }

    // Listener method - change status of damage to repair completed
    public void completeStep(PlushieDamage plushieDamage) {
        if (this.plushieDamage.Equals(plushieDamage)) {

            this.checklistStepIcon.sprite = Resources.Load<Sprite>("Sprites/checkmark");
            Color visible = this.checklistStepIcon.color;
            visible.a = 1f;
            this.checklistStepIcon.color = visible;

            CustomEventManager.current.onRepairCompletion -= completeStep;
        }
    }
}
