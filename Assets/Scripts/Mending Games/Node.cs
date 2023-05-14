using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;
using GameUI;

public class Node : MonoBehaviour {
    public bool targetNode { get; set; }
    public bool triggered { get; set; }
    public ToolType requiredToolType { get; set; }
    public SpriteRenderer spriteRenderer { get; set; }


    private void Awake() {
        targetNode = false;
        triggered = false;

        // Get components
        spriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    public void Reset(bool target) {
        triggered = false;
        targetNode = target;
        spriteRenderer.color = Color.black;
    }

    public void TriggerNode(bool target) {
        targetNode = target;
        spriteRenderer.color = Color.blue;
    }

    private void OnMouseDown() {
        if (!triggered && 
            targetNode && 
            CanvasManager.toolType == requiredToolType
        ) {
            triggered = true;
            MendingGameEventManager.Current.NodeTriggered(this);
        }
    }

    private void OnMouseOver() {
        Debug.Log(Input.GetMouseButton(0) &&
            !triggered &&
            targetNode &&
            CanvasManager.toolType == requiredToolType);
        if (Input.GetMouseButton(0) && 
            !triggered && 
            targetNode && 
            CanvasManager.toolType == requiredToolType
        ) {
            Debug.Log("CLICK");
            triggered = true;
            MendingGameEventManager.Current.NodeTriggered(this);
        }
    }
}
