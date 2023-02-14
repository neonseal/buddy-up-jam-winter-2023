using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plushie : MonoBehaviour
{
    private GameObject plushieDamage;
    private SpriteRenderer spriteRenderer;
    private Renderer plushieRenderer;
    private string spritePath = "Sprites/Circle";
    private Sprite sprite;

    // Start is called before the first frame update
    void Start()
    {
        this.sprite = Resources.Load<Sprite>(spritePath);

        this.transform.gameObject.AddComponent<SpriteRenderer>();
        this.spriteRenderer = this.transform.gameObject.GetComponent<SpriteRenderer>();
        this.spriteRenderer.sprite = this.sprite;

        Debug.Log(this.sprite);

        this.plushieRenderer = this.transform.gameObject.GetComponent<Renderer>();
        this.plushieRenderer.sortingLayerID = SortingLayer.NameToID("PlushieLayer");
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
