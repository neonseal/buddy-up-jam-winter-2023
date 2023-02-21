using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GameUI;

public class CustomEventManager : MonoBehaviour
{
    public static CustomEventManager CustomEventManagerInstance;

    //Singleton setup
    private void Awake() {
        CustomEventManagerInstance = this;
    }
}
 