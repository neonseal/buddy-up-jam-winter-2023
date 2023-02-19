using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GameUI;

public class CustomEventManager : MonoBehaviour
{
    //Singleton setup
    public static CustomEventManager CustomEventManagerInstance;

    private void Awake() {
        CustomEventManagerInstance = this;
    }

    [SerializeField]
    private Canvas canvas;
    public event Action onClickingDamage;

    public GameObject ClickingDamage() {
        return this.canvas.GetComponent<CanvasManager>().currentTool;
    }
}
 