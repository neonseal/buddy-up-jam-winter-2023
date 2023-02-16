using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageScripts;

public class PlushieDamage : MonoBehaviour
{
    // Fields
    internal PlushieDamageType plushieDamageType;
    internal Sprite plushieDamageSprite;
    
    // Components
    private SpriteRenderer damageSpriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer plushieDamageSpriteRenderer = this.gameObject.AddComponent<SpriteRenderer>();
        plushieDamageSpriteRenderer.sortingLayerID = SortingLayer.NameToID("PlushieDamageLayer");
    }

    public void changeDamageType(PlushieDamageType newDamageType) {
        this.plushieDamageType = newDamageType;
        this.plushieDamageSprite = DamageTypes.damageInfoDictionary[newDamageType].sprite;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
