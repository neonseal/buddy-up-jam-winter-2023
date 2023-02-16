using UnityEngine;
using DamageScripts;

public class Plushie : MonoBehaviour
{
    private SpriteRenderer plushieSpriteRenderer;
    private string spritePath = "Sprites/nuigurumi_bear";
    private Sprite sprite;

    // Start is called before the first frame update
    void Start()
    {
        this.sprite = Resources.Load<Sprite>(spritePath);

        this.plushieSpriteRenderer = this.transform.gameObject.AddComponent<SpriteRenderer>();
        this.plushieSpriteRenderer.sprite = this.sprite;

        this.plushieSpriteRenderer.sortingLayerID = SortingLayer.NameToID("PlushieLayer");
        this.AddPlushieDamageToScene(2f, 0f, PlushieDamageType.SMALL_RIP);
        this.AddPlushieDamageToScene(2.5f, 0.5f, PlushieDamageType.LARGE_RIP);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void AddPlushieDamageToScene(float x, float y, PlushieDamageType plushieDamageTypeParameter)
    {
        // Initialize damage as child GameObject
        GameObject plushieDamageGameObject = new GameObject();
        plushieDamageGameObject.name = "PlushieDamage";

        // Attach script for functions
        PlushieDamage plushieDamageScript = plushieDamageGameObject.AddComponent<PlushieDamage>();

        // Assign initial fields to plushieDamage child game object
        plushieDamageScript.setDamageType(plushieDamageTypeParameter);

        // Set local position of damage to be parameter floats x and y
        plushieDamageGameObject.transform.localPosition = new Vector3(x, y, 1f);

        //Add damage as a child GameObject of the plushie
        plushieDamageGameObject.transform.SetParent(this.transform);
    }
}
