using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlushieDamage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Renderer pluishieDamageRenderer = this.transform.gameObject.GetComponent<Renderer>();
        pluishieDamageRenderer.sortingLayerID = SortingLayer.NameToID("PlushieDamageLayer");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
