using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour {
    public bool startingTarget { get; set; }
    public bool active { get; set; }

    private void OnMouseDown() {
        if (active) {
            Debug.Log("CLICK");
        }
    }
}
