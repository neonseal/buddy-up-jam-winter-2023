using UnityEngine;
using DamageScripts;

public class Plushie : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Renderer plushieRenderer;
    private string spritePath = "Sprites/nuigurumi_bear";
    private Sprite sprite;

    // Start is called before the first frame update
    void Start()
    {
        this.sprite = Resources.Load<Sprite>(spritePath);

        this.transform.gameObject.AddComponent<SpriteRenderer>();
        this.spriteRenderer = this.transform.gameObject.GetComponent<SpriteRenderer>();
        this.spriteRenderer.sprite = this.sprite;

        this.plushieRenderer = this.transform.gameObject.GetComponent<Renderer>();
        this.plushieRenderer.sortingLayerID = SortingLayer.NameToID("PlushieLayer");
        this.AddPlushieDamageToScene(2f, 0f, PlushieDamageType.SmallRip);
        this.AddPlushieDamageToScene(2.5f, 0.5f, PlushieDamageType.WornStuffing);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void AddPlushieDamageToScene(float x, float y, PlushieDamageType PlushieDamageType)
    {
        // Initialize damage as child GameObject
        GameObject plushieDamage = new GameObject();
        plushieDamage.name = "PlushieDamage";

        // Modify components
        // Attach script for functions
        plushieDamage.AddComponent<PlushieDamage>();
        // Attach SpriteRenderer to add damage spirte
        plushieDamage.AddComponent<SpriteRenderer>();

        // Add sprite to damage, corresponding to parameter PlushieDamageType
        SpriteRenderer damageSpriteRenderer = plushieDamage.transform.gameObject.GetComponent<SpriteRenderer>();
        // Retrieve sprite (or any other information if needed) from dictionary
        damageSpriteRenderer.sprite = DamageTypes.damageInfoDictionary[PlushieDamageType].sprite;

        

        // Set local position of damage to be parameter floats x and y
        plushieDamage.transform.localPosition = new Vector3(x, y, 1f);

        //Add damage as a child GameObject of the plushie
        plushieDamage.transform.SetParent(this.transform);
    }
}
