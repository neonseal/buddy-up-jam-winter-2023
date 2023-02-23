using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DamageScripts;

public class ChecklistController : MonoBehaviour
{
    // Game object that stores all the CheckListSteps
    [SerializeField]
    private GameObject checklistItemsObject;
    // Prefab of ChecklistStep
    [SerializeField]
    private GameObject checklistStepPrefab;

    private ChecklistStep[] steps;
    private Button btn;
    private int checklistStepcount;
    private int repairCompletionCount;

    private void Awake()
    {
        steps = GetComponentsInChildren<ChecklistStep>();
        btn = GetComponentInChildren<Button>();
        this.checklistStepcount = 0;
        this.repairCompletionCount = 0;
    }

    private void Start()
    {
        CustomEventManager.current.onDamageGeneration += populateChecklist;
        CustomEventManager.current.onRepairCompletion += incrementRepairCompletionCount;
        btn.onClick.AddListener(HandleButtonClick);
        this.btn.interactable = false;
    }

    private void Update()
    {

    }

    private void HandleButtonClick()
    {
        Debug.Log("CLICK");
    }

    // Listener method - add a checklist step to checklistItemsObject for each generation event
    private void populateChecklist(PlushieDamage plushieDamage, DamageType damageType)
    {
        this.addChecklistStep(plushieDamage, damageType);
    }

    private void addChecklistStep(PlushieDamage plushieDamage, DamageType damageType)
    {
        this.checklistStepcount++;
        GameObject checklistEntry = Instantiate(checklistStepPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        checklistEntry.transform.SetParent(checklistItemsObject.transform);

        ChecklistStep checklistStepComponent = checklistEntry.GetComponent<ChecklistStep>();

        checklistEntry.name = "Damage " + checklistStepcount + " of type " + damageType;

        checklistEntry.GetComponent<RectTransform>().localScale = Vector3.one;
        checklistStepComponent.changeStepText(damageType);
        checklistStepComponent.plushieDamage = plushieDamage;
    }

    // Listener method - increment repair completion count for each repair completion
    private void incrementRepairCompletionCount(PlushieDamage plushieDamage) {
        this.repairCompletionCount++;
        if (repairCompletionCount >= checklistStepcount) {
            this.btn.interactable = true;
        }
    }
}
