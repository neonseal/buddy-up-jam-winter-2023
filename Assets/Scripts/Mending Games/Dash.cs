using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;
using GameUI;

// Dash Class Definition
// Elements that make up the sewing and cutting games 
public class Dash : MonoBehaviour {
    [SerializeField] private float colorChangeSpeed = 20f;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D dashCollider;
    private bool active;
    private bool complete;
    private ToolType requiredToolType;

    public bool Active {
        get { return this.active; }
        set { this.active = value; }
    }
    public bool Complete {
        get { return this.complete; }
        set { this.complete = value; }
    }
    public ToolType requiredToolTypeType {
        get { return this.requiredToolType; }
        set { this.requiredToolType = value; }
    }

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        active = false;
        complete = false;
    }

    private void Update() {
        if (this.spriteRenderer.color.r > 0.5f && this.spriteRenderer.color.g > 0.5f) {
            CompleteDash(); 
        }
    }

    private void OnMouseOver() {
        // If we are holding the mouse down over the dash with the correct tool,
        // and the dash is active, complete the dash
        if (Input.GetMouseButton(0) 
            && this.active 
            && CanvasManager.currentTool != null
            && CanvasManager.toolType == this.requiredToolType) {
            this.spriteRenderer.color = Color.Lerp(this.spriteRenderer.color, Color.yellow, Time.deltaTime * this.colorChangeSpeed);
        }
    }
    public void Reset() {
        this.complete = false;
        this.spriteRenderer.color = Color.black;
    }

    public void CompleteDash() {
        this.spriteRenderer.color = Color.yellow;
        this.complete = true;
    }





    /*private BoxCollider2D coll2D;

    [SerializeField] private float colorChangeSpeed = 20f;

    private bool mouseHeld;
    private bool complete;

    public bool Complete {
        get { return this.complete; }
    }

    private void Awake() {

        mouseHeld = false;
        complete = false;

        CustomEventManager.Current.onMouseHoldStatusToggle += ToggleMouseStatus;
    }

    private void Update() {
        if (this.spriteRenderer.color.r > 0.8f && this.spriteRenderer.color.g > 0.8f) {
            this.complete = true;
        }
    }

    private void ToggleMouseStatus(bool mouseStatus) {
        this.mouseHeld = mouseStatus;
    }

    private void OnMouseOver() {
        if (this.mouseHeld) {
            this.spriteRenderer.color = Color.Lerp(this.spriteRenderer.color, Color.yellow, Time.deltaTime * this.colorChangeSpeed);
        }
    }

    private void Reset() {
        this.complete = false;
        this.spriteRenderer.color = Color.black;
    }*/
}
