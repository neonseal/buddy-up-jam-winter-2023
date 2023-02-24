using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DamageScripts;
using UnityEngine.EventSystems;

public class ChecklistStep : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image checklistStepIcon;
    private TMP_Text checklistStepText;
    // PlushieDamage this step is associated with
    internal PlushieDamage plushieDamage;
    private Color mouseOverColor = new Color(0.75f, 1f, 0.75f, 1f);
    private bool isMouseOver = false;
    private float colorChangeSpeed = 2f;

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

    private void Update()
    {
        this.highlightDamage(isMouseOver);
    }

    // Generate a Rectangular collider for the message
    private void generateCollider()
    {
        BoxCollider2D oldCollider = this.gameObject.GetComponent<BoxCollider2D>();
        if (oldCollider != null)
        {
            Object.Destroy(oldCollider);
        }
        BoxCollider2D collider = this.gameObject.AddComponent<BoxCollider2D>();
    }

    // Listener method - change the status of multistep repair to the next step
    public void repairDamage(PlushieDamage plushieDamage, DamageType damageType)
    {
        if (this.plushieDamage.Equals(plushieDamage))
        {
            this.changeStepText(damageType);
        }
    }

    // Change step text to that of the parameter damage type
    public void changeStepText(DamageType damageType)
    {
        this.checklistStepText.text = DamageDictionary.damageInfoDictionary[damageType].damageChecklistMessage;
        this.generateCollider();
    }

    // Listener method - change status of damage to repair completed
    public void completeStep(PlushieDamage plushieDamage)
    {
        if (this.plushieDamage.Equals(plushieDamage))
        {

            this.checklistStepIcon.sprite = Resources.Load<Sprite>("Sprites/checkmark");
            Color visible = this.checklistStepIcon.color;
            visible.a = 1f;
            this.checklistStepIcon.color = visible;

            CustomEventManager.current.onRepairCompletion -= completeStep;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        this.isMouseOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        this.isMouseOver = false;
    }

    private void highlightDamage(bool mouseOverFlag)
    {
        if (mouseOverFlag)
        {
            this.plushieDamage.plushieDamageSpriteRenderer.color =
            Color.Lerp(
                this.plushieDamage.plushieDamageSpriteRenderer.color,
                this.mouseOverColor,
                Time.deltaTime * this.colorChangeSpeed
            );
        }
        else
        {
            this.plushieDamage.plushieDamageSpriteRenderer.color =
            Color.Lerp(
                this.plushieDamage.plushieDamageSpriteRenderer.color,
                Color.white,
                Time.deltaTime * this.colorChangeSpeed * 2f
            );
        }
    }
}
