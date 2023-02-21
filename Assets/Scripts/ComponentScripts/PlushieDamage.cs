using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageScripts;
using GameUI;

public class PlushieDamage : MonoBehaviour
{
    // Fields
    private DamageType plushieDamageType;

    // Components
    private SpriteRenderer plushieDamageSpriteRenderer;

    void Awake()
    {
        this.plushieDamageSpriteRenderer = this.gameObject.AddComponent<SpriteRenderer>();
        plushieDamageSpriteRenderer.sortingLayerID = SortingLayer.NameToID("PlushieDamageLayer");
    }

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.name = this.plushieDamageType.ToString();
        this.gameObject.tag = "Damage";
        this.gameObject.layer = LayerMask.NameToLayer("Game Workspace");
        this.generateCollider(); 
    }

    public void setDamageType(DamageType newDamageType)
    {
        this.plushieDamageType = newDamageType;
        this.plushieDamageSpriteRenderer.sprite = DamageDictionary.damageInfoDictionary[newDamageType].sprite;
    }

    public void deletePlushieDamage()
    {
        Object.Destroy(this.gameObject); // F
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void generateCollider() {
        CapsuleCollider2D collider = this.gameObject.AddComponent<CapsuleCollider2D>();
        collider.direction = CapsuleDirection2D.Horizontal;
    }

    void OnMouseDown() {
        if (DamageDictionary.damageInfoDictionary[this.plushieDamageType].correctToolType.Equals(CanvasManager.currentTool.GetComponent<ToolScript>().toolScriptableObject.toolType)) {
            this.deletePlushieDamage();
        }
    }
}
