using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour {
    private SpriteRenderer spriteRenderer;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();

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
