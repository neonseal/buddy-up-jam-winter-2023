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
    private Button submitButton;
    private int checklistStepcount;
    private int repairCompletionCount;

    private void Awake()
    {
        steps = GetComponentsInChildren<ChecklistStep>();
        submitButton = GetComponentInChildren<Button>();
        this.submitButton.interactable = false;
    }

    private void Start()
    {
        CustomEventManager.Current.onDamageGeneration += populateChecklist;
        CustomEventManager.Current.onRepairCompletion += incrementRepairCompletionCount;
        submitButton.onClick.AddListener(CompleteRepairButtonClick);
    }

    private void Update()
    {

    }

    // When all damage points have been repaired, the finish/submit button will be activated,
    // allowing the player to package up the plushie and return it to the customer
    private void CompleteRepairButtonClick()
    {
        // Broadcast a plushie repair completion event
        CustomEventManager.Current.plushieOverallRepairCompletionEvent();

        // Set repair counters to 0
        this.checklistStepcount = 0;
        this.repairCompletionCount = 0;
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
            this.submitButton.interactable = true;
        }
    }
}
