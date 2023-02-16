using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToolSlot : MonoBehaviour, IPointerDownHandler {
    private ToolController tool;


    private void Awake() {
        tool = GetComponentInChildren<ToolController>();
    }

    public void OnPointerDown(PointerEventData eventData) {
        if (tool.PickedUp) {
            tool.GetComponent<RectTransform>().position = GetComponent<RectTransform>().position;

            tool.GetComponent<CanvasGroup>().alpha = 1;
            tool.GetComponent<CanvasGroup>().blocksRaycasts = true;
            EventManager.TriggerEvent("DropTool");
        }
    }
}