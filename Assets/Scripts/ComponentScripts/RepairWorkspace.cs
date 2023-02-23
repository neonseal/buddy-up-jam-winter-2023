using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairWorkspace : MonoBehaviour
{
    private GameObject plushie;

    private void Start() {
        CustomEventManager.instance.onGameStart += AddPlushieToScene;
    }

    private void AddPlushieToScene()
    {
        this.plushie = new GameObject();
        this.plushie.name = "Plushie";
        this.plushie.AddComponent<Plushie>();
        this.plushie.transform.SetParent(this.transform);
    }
}
