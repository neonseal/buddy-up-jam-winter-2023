using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Tool;

public class ToolController : MonoBehaviour, ITool {
    private Collider2D targetCollider;
    private GameObject selectedTool;
    private Vector3 mousePosition;
    private Vector3 startingPosition;

    [SerializeField] private string toolName;
    [SerializeField] private ToolType type;

    public bool pickedUp;

    private void Awake() {
        startingPosition = gameObject.transform.position;
        Renderer toolRenderer = GetComponent<Renderer>();
        toolRenderer.sortingLayerID = SortingLayer.NameToID("Tool");
    }

    private void Update() {
        if (Camera.main.ScreenToWorldPoint(Input.mousePosition) != mousePosition) {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (pickedUp && selectedTool) {
            selectedTool.transform.position = new Vector3(mousePosition.x, mousePosition.y, selectedTool.transform.position.z);
        }
    }

    // Check for mouse click when the user is hovering over a tool to trigger pick up
    private void OnMouseOver() {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        targetCollider = Physics2D.OverlapPoint(mousePosition);

        if (Input.GetMouseButtonDown(0)) {
            // If not holding a tool, try to pick up tool
            if (!pickedUp) {
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
            pickedUp = true;
        }
    }

    // Drop tool back on starting position
    private void DropTool() {
        selectedTool.transform.position = startingPosition;
        pickedUp = false;
        Debug.Log("DROP");
    }

    public void ApplyTool() {
        // On click, check if the collider is a valid damage type for the selected tool
    }

    private void OnEnable() {
        EventManager.StartListening("DropTool", DropTool);
    }

    private void OnDisable() {
        EventManager.StopListening("DropTool", DropTool);
    }
}
