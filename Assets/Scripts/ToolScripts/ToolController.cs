using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Tool;

public class ToolController : MonoBehaviour, IPointerDownHandler, ITool {
    [SerializeField] private ToolType toolType; 

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector3 mousePosition;
    private Collider2D targetCollider;

    private bool pickedUp;
    public bool PickedUp {
        get { return pickedUp; }
    }

    private void Awake() {
        pickedUp = false;
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Update() {
        if (Camera.main.ScreenToWorldPoint(Input.mousePosition) != mousePosition) {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (pickedUp) {
            rectTransform.position = Input.mousePosition;
        }
    }

    public void OnPointerDown(PointerEventData eventData) {
        // Check if the tool needs to be picked up
        if (!pickedUp) {
            pickedUp = true;
            canvasGroup.alpha = .6f;
            canvasGroup.blocksRaycasts = false;
        } else {
            // Check if we are above a damage spot
            targetCollider = Physics2D.OverlapPoint(mousePosition);

            // Check if over plushy
            if (targetCollider.transform.gameObject.tag == "Damage") {
                PlushieDamage damageObject = targetCollider.transform.gameObject.GetComponent<PlushieDamage>();
                ApplyTool(damageObject);
            }
        }
    }

    public void ApplyTool(PlushieDamage damageObject) {
        // On click, check if the collider is a valid damage type for the selected tool
        switch (toolType) {
            case (ToolType.Scissors):
                // Check if large rip or worn stuffing
                break;
            case (ToolType.Needle):
                // Check if small rip
                // Or large rip that has been stuffed
                // Or worn stuffing that has been cut and stuffed
                break;
            case (ToolType.Stuffing):
                // Check if large rip
                // Or worn stuffing that has been cut
                break;
        }
    }

    // Drop tool back on starting position
    private void DropTool() {
        pickedUp = false;
    }


    private void OnEnable() {
        EventManager.StartListening("DropTool", DropTool);
    }

    private void OnDisable() {
        EventManager.StopListening("DropTool", DropTool);
    }

}
