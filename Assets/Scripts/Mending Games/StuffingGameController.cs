using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuffingGameController : MonoBehaviour
{
    [SerializeField] private Texture2D mainTex;
    [SerializeField] private Texture2D unstuffedTex;

    private void Start() {

    }

    private void OnMouseDown() {
        Debug.Log("CLICK");
    }
}
