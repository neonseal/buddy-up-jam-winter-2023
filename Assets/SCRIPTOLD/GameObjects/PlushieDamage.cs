using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;
using TutorialSequence;

public class PlushieDamage_old : MonoBehaviour {
    // Fields
    private DamageType plushieDamageType;
    private bool tutorialActionRequired;
    private bool gameActive;
    public bool GameActive {
        get { return gameActive; }
        set { gameActive = value; }
    }

    // Components
    internal SpriteRenderer plushieDamageSpriteRenderer;

    void Awake() {
        tutorialActionRequired = false;
        gameActive = false;

        this.plushieDamageSpriteRenderer = this.gameObject.AddComponent<SpriteRenderer>();
        plushieDamageSpriteRenderer.sortingLayerID = SortingLayer.NameToID("PlushieDamageLayer");

        TutorialSequenceEventManager.Current.onRequireDamageSelectContinueAction += () => {tutorialActionRequired = true; };
    }

    // Start is called before the first frame update
    void Start() {
        this.initializeDamage();
    }

    public void changeDamageType(DamageType newDamageType) {
        this.plushieDamageType = newDamageType;
        //this.plushieDamageSpriteRenderer.sprite = DamageDictionary.damageInfoDictionary[newDamageType].sprite;
    }

    public void deletePlushieDamage() {
        Object.Destroy(this.gameObject);
    }

    private void initializeDamage() {
        this.gameObject.name = this.plushieDamageType.ToString();
        this.gameObject.tag = "Damage";
        this.gameObject.layer = LayerMask.NameToLayer("Game Workspace");
    }

    public void generateCollider(Vector3 scale, int zRotation) {
        CapsuleCollider2D oldCollider = this.gameObject.GetComponent<CapsuleCollider2D>();
        if (oldCollider != null) {
            Object.Destroy(oldCollider);
        }
        CapsuleCollider2D collider = this.gameObject.AddComponent<CapsuleCollider2D>();
        collider.transform.localScale = scale;
        collider.transform.rotation = Quaternion.Euler(0, 0, zRotation);
        collider.direction = CapsuleDirection2D.Horizontal;
    }

    private void OnMouseDown() {
        // Don't open if already open on a tutorial is active and action is not yet required
        if (gameActive || (TutorialSequenceManager.isTutorialActive && !tutorialActionRequired)) { 
            return;
        }

        // Pick correct routine
        if (this.plushieDamageType == DamageType.SmallRip) {
            //this.deletePlushieDamage();
            gameActive = true;
            DamageLifeCycleEventManager.Current.startRepairMiniGame(this, DamageType.SmallRip);
        } else if (this.plushieDamageType == DamageType.LargeRip) {
            //this.changeDamageType(DamageType.LargeRipMissingStuffing);
            gameActive = true;
            DamageLifeCycleEventManager.Current.startRepairMiniGame(this, DamageType.LargeRip);
        } else if (this.plushieDamageType == DamageType.LargeRipMissingStuffing) {
            //this.deletePlushieDamage();
            gameActive = true;
            DamageLifeCycleEventManager.Current.startRepairMiniGame(this, DamageType.LargeRipMissingStuffing);
        } else if (this.plushieDamageType == DamageType.WornStuffing) {
            //this.changeDamageType(DamageType.LargeRip);
            gameActive = true;
            DamageLifeCycleEventManager.Current.startRepairMiniGame(this, DamageType.WornStuffing);
        }

        // Check if tutorial interaction is required
        if (tutorialActionRequired) {
            tutorialActionRequired = false;
            StartCoroutine(TutorialSequenceEventManager.Current.HandleTutorialRequiredActionCompletion());
        }
    }



}
