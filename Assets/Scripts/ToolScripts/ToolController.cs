using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tool;

public class ToolController : MonoBehaviour, ITool {
    private Collider2D targetCollider;
    private GameObject selectedTool;
    private Vector3 mousePosition;

    [SerializeField] private string toolName;
    [SerializeField] private ToolType type;

    private bool toolPickedUp = false;

    private void Update() {
        if (Camera.main.ScreenToWorldPoint(Input.mousePosition) != mousePosition) {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (toolPickedUp && selectedTool) {
            selectedTool.transform.position = new Vector3(mousePosition.x, mousePosition.y, selectedTool.transform.position.z);
        }
    }

    // Check for mouse click when the user is hovering over a tool to trigger pick up
    private void OnMouseOver() {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        targetCollider = Physics2D.OverlapPoint(mousePosition);

        if (Input.GetMouseButtonDown(0)) {
            // If not holding a tool, try to pick up tool
            if (!toolPickedUp) {
                PickUpTool();
            } else {
                // Check if over plushy
                if (targetCollider.transform.gameObject.tag == "Damage") {
                    ApplyTool();
                } 
            }
        }
    }

    // Attempt to pick up tool
    private void PickUpTool() {
        if (targetCollider && targetCollider.transform.gameObject.tag == "Tool") {
            selectedTool = targetCollider.transform.gameObject;
            toolPickedUp = true;
        }
    }

    public void ApplyTool() {
        // On click 
    }
}
