using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;
using GameUI;

public class PlushieDamage : MonoBehaviour
{
    // Fields
    private DamageType plushieDamageType;
    private bool gameActive = false;

    public bool GameActive {
        get { return gameActive; }
        set { gameActive = value; }
    }

    // Components
    internal SpriteRenderer plushieDamageSpriteRenderer;

    void Awake()
    {
        this.plushieDamageSpriteRenderer = this.gameObject.AddComponent<SpriteRenderer>();
        plushieDamageSpriteRenderer.sortingLayerID = SortingLayer.NameToID("PlushieDamageLayer");
    }

    // Start is called before the first frame update
    void Start()
    {
        this.initializeDamage();
        this.generateCollider();
    }

    private void initializeDamage() {
        this.gameObject.name = this.plushieDamageType.ToString();
        this.gameObject.tag = "Damage";
        this.gameObject.layer = LayerMask.NameToLayer("Game Workspace");
    }

    public void changeDamageType(DamageType newDamageType)
    {
        this.plushieDamageType = newDamageType;
        this.plushieDamageSpriteRenderer.sprite = DamageDictionary.damageInfoDictionary[newDamageType].sprite;
    }

    public void deletePlushieDamage()
    {
        Object.Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void generateCollider()
    {
        CapsuleCollider2D oldCollider = this.gameObject.GetComponent<CapsuleCollider2D>();
        if (oldCollider != null) {
            Object.Destroy(oldCollider);
        }
        CapsuleCollider2D collider = this.gameObject.AddComponent<CapsuleCollider2D>();
        collider.direction = CapsuleDirection2D.Horizontal;
    }

    void OnMouseDown()
    {
        // Pick correct routine
        if (this.plushieDamageType == DamageType.SmallRip) {
            //this.deletePlushieDamage();
            gameActive = true;
            CustomEventManager.Current.startRepairMiniGame(this, DamageType.SmallRip);
        } else if (this.plushieDamageType == DamageType.LargeRip) {
            //this.changeDamageType(DamageType.LargeRipMissingStuffing);
            gameActive = true;
            CustomEventManager.Current.startRepairMiniGame(this, DamageType.LargeRip);
        } else if (this.plushieDamageType == DamageType.LargeRipMissingStuffing) {
            //this.deletePlushieDamage();
            gameActive = true;
            CustomEventManager.Current.startRepairMiniGame(this, DamageType.LargeRipMissingStuffing);
        } else if (this.plushieDamageType == DamageType.WornStuffing) {
            //this.changeDamageType(DamageType.LargeRip);
            gameActive = true;
            CustomEventManager.Current.startRepairMiniGame(this, DamageType.WornStuffing);
        }

        // Check if player is holding any tool
        /*if (CanvasManager.currentTool != null && !gameActive)
        {
            // Check for correct tool type
            if (DamageDictionary.damageInfoDictionary[this.plushieDamageType].correctToolType.Equals(CanvasManager.currentTool.GetComponent<Tool>().toolScriptableObject.toolType))
            {
                
            }
        }*/
    }
}
