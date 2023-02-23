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

    private void Awake()
    {
        steps = GetComponentsInChildren<ChecklistStep>();
        btn = GetComponentInChildren<Button>();
        this.checklistStepcount = 0;
    }

    private void Start()
    {
        CustomEventManager.current.onDamageGeneration += populateChecklist;
        btn.onClick.AddListener(HandleButtonClick);
    }

    private void Update()
    {

    }

    private void HandleButtonClick()
    {
        Debug.Log("CLICK");
    }

    // Listener method
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
}
