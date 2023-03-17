using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {
    public bool targetNode { get; set; }
    public bool triggered { get; set; }
    public int index { get; set; }

    private void Awake() {
        targetNode = false;
        triggered = false;
    }


    private void OnMouseDown() {
        if (!triggered && targetNode) {
            triggered = true;
            MendingGameEventManager.Current.NodeTriggered(this);
            Debug.Log("GAME START");
        }
    }

    private void OnMouseOver() {
        if (Input.GetMouseButton(0) && !triggered && targetNode) {
            triggered = true;
            MendingGameEventManager.Current.NodeTriggered(this);
            Debug.Log("NODE TRIGGERED");
        }
    }
}
