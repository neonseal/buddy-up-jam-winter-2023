using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;
using GameUI;
using DG.Tweening;

// Dash Class Definition
// Elements that make up the sewing and cutting games 
public class Dash : MonoBehaviour {
    public bool dashActive { get; set; }
    public bool triggered { get; set; }
    public ToolType requiredToolType { get; set; }
    public SpriteRenderer spriteRenderer { get; set; }

    private void Awake() {
        DOTween.Init();
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
            Sequence sequence = DOTween.Sequence();
            sequence.Append(this.gameObject.transform.DOScale(.15f, 0.25f));
            sequence.SetLoops(2, LoopType.Yoyo);
        }
    }
}
