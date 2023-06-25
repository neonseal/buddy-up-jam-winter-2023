using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plushie : MonoBehaviour {
    private PlushieDamageGO[] damageList;
    private PlushieDamageSM damageStateMachine;

    private void Awake() {
        damageList = this.GetComponentsInChildren<PlushieDamageGO>();

    }
}