using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairWorkspace : MonoBehaviour
{
    private GameObject plushie;

    // Start is called before the first frame update
    void Start()
    {
        this.AddPlushieToScene();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void AddPlushieToScene()
    {
        this.plushie = new GameObject();
        this.plushie.name = "Plushie";
        this.plushie.AddComponent<Plushie>();
        this.plushie.transform.SetParent(this.transform);
    }
}
