using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;
using GameUI;

// Dash Class Definition
// Elements that make up the sewing and cutting games 
public class Dash : MonoBehaviour {
    public bool dashActive { get; set; }
    public bool triggered { get; set; }
    public ToolType requiredToolType { get; set; }
    private SpriteRenderer spriteRenderer;

    private void Awake() {
        // Set state flags
        dashActive = false;
        triggered = false;

        // Get components
        spriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    public void Activate() {
        dashActive = true;
        spriteRenderer.color = Color.blue;
    }

    public void Reset(bool active) {
        triggered = false;
        dashActive = active;
        spriteRenderer.color = Color.black;
    }

    private void OnMouseOver() {
        if (Input.GetMouseButton(0) &&
            dashActive &&
            !triggered &&
            CanvasManager.toolType == requiredToolType
        ) {
            spriteRenderer.color = Color.yellow;
            triggered = true;

        }
    }
}
