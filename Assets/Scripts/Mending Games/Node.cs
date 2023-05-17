using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;
using GameUI;
using DG.Tweening;

public class Node : MonoBehaviour {
    public bool targetNode { get; set; }
    public bool triggered { get; set; }
    public ToolType requiredToolType { get; set; }
    public SpriteRenderer spriteRenderer { get; set; }


    private void Awake() {
        DOTween.Init();
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
        if (Input.GetMouseButton(0) && 
            !triggered && 
            targetNode && 
            CanvasManager.toolType == requiredToolType
        ) {
            spriteRenderer.color = Color.yellow;
            triggered = true;
            Sequence sequence = DOTween.Sequence();
            sequence.Append(this.gameObject.transform.DOScale(.15f, 0.25f));
            sequence.SetLoops(2, LoopType.Yoyo);
            MendingGameEventManager.Current.NodeTriggered(this);
        }
    }
}
