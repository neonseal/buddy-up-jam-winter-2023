using UnityEngine;
using GameData;

public class Plushie_old : MonoBehaviour
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

    private void FixedUpdate() {
        if (this.isSendingOff) {
            this.transform.position = Vector2.Lerp(this.transform.position, this.sendOffDestinationCoordinate, Time.deltaTime * this.sendOffSpeed);
            if (this.transform.position.y > (this.yDeletionThreshold - 2f)) {
                this.deletePlushie();
            }
        }
    }

    internal void AddPlushieDamageToScene(PlushieScriptableObject plushieScriptableObject, int index)
    {
        // Initialize damage as child GameObject
        GameObject plushieDamageGameObject = new GameObject();

        // Attach script for functions
        PlushieDamage_old plushieDamageScript = plushieDamageGameObject.AddComponent<PlushieDamage_old>();
        plushieDamageScript.generateCollider(plushieScriptableObject.damageColliderSizeList[index], plushieScriptableObject.damageZRotationList[index]);

        // Assign initial type to plushieDamage child game object
        plushieDamageScript.changeDamageType(plushieScriptableObject.damageTypeList[index]);

        // Set local position of damage to be parameter floats x and y
        plushieDamageGameObject.transform.localPosition = plushieScriptableObject.damagePositionList[index];

        // Add damage as a child GameObject of the plushie
        plushieDamageGameObject.transform.SetParent(this.transform);

        // Broadcast a damageGenerationEvent
        DamageLifeCycleEventManager.Current.generateDamage(plushieDamageScript, plushieScriptableObject.damageTypeList[index]);
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
