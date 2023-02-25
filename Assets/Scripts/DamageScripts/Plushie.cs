using UnityEngine;
using DamageScripts;

public class Plushie : MonoBehaviour
{
    private SpriteRenderer plushieSpriteRenderer;
    internal Sprite sprite;

    private void Awake() {
        this.plushieSpriteRenderer = this.transform.gameObject.AddComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        CustomEventManager.Current.onPlushieDeletionRequest += deletePlushie;
        this.plushieSpriteRenderer.sprite = this.sprite;

        this.plushieSpriteRenderer.sortingLayerID = SortingLayer.NameToID("PlushieLayer");
    }

    // Update is called once per frame
    void Update()
    {

    }

    internal void AddPlushieDamageToScene(Vector2 position, DamageType damageType)
    {
        // Initialize damage as child GameObject
        GameObject plushieDamageGameObject = new GameObject();

        // Attach script for functions
        PlushieDamage plushieDamageScript = plushieDamageGameObject.AddComponent<PlushieDamage>();

        // Assign initial type to plushieDamage child game object
        plushieDamageScript.changeDamageType(damageType);

        // Set local position of damage to be parameter floats x and y
        plushieDamageGameObject.transform.localPosition = position;

        // Add damage as a child GameObject of the plushie
        plushieDamageGameObject.transform.SetParent(this.transform);

        // Broadcast a damageGenerationEvent
        CustomEventManager.Current.damageGenerationEvent(plushieDamageScript, damageType);
    }

    private void deletePlushie() {
        Object.Destroy(this);
        CustomEventManager.Current.onPlushieDeletionRequest -= deletePlushie;
    }
}
