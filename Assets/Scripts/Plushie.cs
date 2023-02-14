using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plushie : MonoBehaviour
{
    private GameObject plushieDamage;

    // Start is called before the first frame update
    void Start()
    {
        this.AddPlushieDamageToScene();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void AddPlushieDamageToScene()
    {
        this.plushieDamage = new GameObject();
        this.plushieDamage.name = "PlushieDamage";
        this.plushieDamage.AddComponent<PlushieDamage>();
        this.plushieDamage.transform.SetParent(this.transform);
    }
}
