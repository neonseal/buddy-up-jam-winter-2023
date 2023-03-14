using UnityEngine;
using GameData;

public class Plushie : MonoBehaviour
{
    private SpriteRenderer plushieSpriteRenderer;
    internal Sprite sprite;
    private bool isSendingOff = false;
    private float yDeletionThreshold = 20f;
    private Vector2 sendOffDestinationCoordinate;
    private float sendOffSpeed = 1f;

    private void Awake() {
        this.plushieSpriteRenderer = this.transform.gameObject.AddComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        PlushieLifeCycleEventManager.Current.onSendOffPlushie += sendOffPlushie;
        this.plushieSpriteRenderer.sprite = this.sprite;

        this.plushieSpriteRenderer.sortingLayerID = SortingLayer.NameToID("PlushieLayer");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate() {
        if (this.isSendingOff) {
            this.transform.position = Vector2.Lerp(this.transform.position, this.sendOffDestinationCoordinate, Time.deltaTime * this.sendOffSpeed);
            if (this.transform.position.y > (this.yDeletionThreshold - 2f)) {
                this.deletePlushie();
            }
        }
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
        DamageLifeCycleEventManager.Current.generateDamage(plushieDamageScript, damageType);
    }

    private void sendOffPlushie() {
        this.isSendingOff = true;
        this.sendOffDestinationCoordinate = this.transform.position + new Vector3(0, this.yDeletionThreshold);
    }

    private void deletePlushie() {
        PlushieLifeCycleEventManager.Current.onFinishPlushieRepair -= this.sendOffPlushie;
        PlushieLifeCycleEventManager.Current.deletePlushie();
        Object.Destroy(this);
    }
}
