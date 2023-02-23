using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GameUI;

public class CustomEventManager : MonoBehaviour
{
    public static CustomEventManager instance;

    //Singleton setup
    private void Awake() {
        instance = this;
    }

    public event Action onGameStart;
    public void GameStart() {
        if (onGameStart != null) {
            onGameStart();
        }
    }
}
 