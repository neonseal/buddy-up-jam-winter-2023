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
    private Color mouseOverColor = new Color(0.6f, 1f, 0.6f, 1f);
    private bool isMouseOver = false;
    private float colorChangeSpeed = 5f;
    private BoxCollider2D checklistCollider;

    void Awake()
    {
        // Initialize component local fields;
        this.checklistStepIcon = this.gameObject.transform.Find("CheckBackground").gameObject.transform.Find("CheckmarkImage").GetComponent<Image>();
        this.checklistStepText = this.gameObject.transform.Find("StepText").GetComponent<TMP_Text>();
        this.checklistCollider = this.gameObject.GetComponent<BoxCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Subscribe to onRepairCompletion event
        CustomEventManager.Current.onRepairCompletion += completeStep;
        // Subscribe to onRepair event
        CustomEventManager.Current.onRepair += repairDamage;
    }

    private void Update()
    {
        // Run highlighting logic if collider isn't null (i.e. the step hasn't been completed yet)
        if (this.checklistCollider != null) {
            this.highlightDamage(isMouseOver);
        }
    }

    // Generate a Rectangular collider for the message
    private void generateCollider()
    {
        // Get rid of the old collider if there's an existing one in place
        if (this.checklistCollider != null)
        {
            Object.Destroy(this.checklistCollider);
        }
        // Create a new box collider (with a fit size)
        this.checklistCollider = this.gameObject.AddComponent<BoxCollider2D>();
    }

    // Listener method - change the status of multistep repair to the next step
    public void repairDamage(PlushieDamage plushieDamage, DamageType damageType)
    {
        if (this.plushieDamage != null && this.plushieDamage.Equals(plushieDamage))
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

            Object.Destroy(this.checklistCollider);
            CustomEventManager.Current.onRepairCompletion -= completeStep;
        }
    }

    // Set mouseOver flag to true
    public void OnPointerEnter(PointerEventData eventData)
    {
        this.isMouseOver = true;
    }

    // Set mouseOver flag to false
    public void OnPointerExit(PointerEventData eventData)
    {
        this.isMouseOver = false;
    }

    // When mousing over, changes color of associated damage; otherwise, revert to default color
    private void highlightDamage(bool mouseOverFlag)
    {
        if (this.isMouseOver)
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
                Time.deltaTime * this.colorChangeSpeed
            );
        }
    }
}
