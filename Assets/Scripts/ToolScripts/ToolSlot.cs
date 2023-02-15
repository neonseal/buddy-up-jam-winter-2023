using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolSlot : MonoBehaviour {
    private ToolController tool;

    private void Awake() {
        tool = GetComponentInChildren<ToolController>();
        Renderer toolSlotRenderer = GetComponent<Renderer>();
        toolSlotRenderer.sortingLayerID = SortingLayer.NameToID("ToolSlot");
    }

    private void OnMouseOver() {
        if (Input.GetMouseButtonDown(0) && tool.pickedUp) {
            EventManager.TriggerEvent("DropTool");
        }
    }
}
