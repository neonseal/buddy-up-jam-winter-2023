using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlushieDamage : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Renderer pluishieDamageRenderer;
    private string spritePath = "Sprites/IsometricDiamond";
    private Sprite sprite;

    // Start is called before the first frame update
    void Start()
    {
        this.sprite = Resources.Load<Sprite>(spritePath);

        this.transform.gameObject.AddComponent<SpriteRenderer>();
        this.spriteRenderer = this.transform.gameObject.GetComponent<SpriteRenderer>();
        this.spriteRenderer.color = Color.red;
        this.spriteRenderer.sprite = this.sprite;
        
        this.pluishieDamageRenderer = this.transform.gameObject.GetComponent<Renderer>();
        this.pluishieDamageRenderer.sortingLayerID = SortingLayer.NameToID("PlushieDamageLayer");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
