using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GameUI;

public class CustomEventManager : MonoBehaviour
{
    public static CustomEventManager current;

    //Singleton setup
    private void Awake() {
        current = this;
    }

}
 