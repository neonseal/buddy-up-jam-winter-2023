using UnityEngine;

public class StuffingTarget {
    public GameObject targetGameObject { get; set; }
    public float unstuffedAreaTotal { get; set; }
    public float unstuffedAreaCurrent { get; set; }
    public Texture2D stuffingMaskTexture { get; set; }
    public bool complete { get; set; }
    public StuffingTarget(GameObject target, float unstuffedTot, Texture2D texture) {
        this.complete = false;
        this.targetGameObject = target;
        this.unstuffedAreaTotal = unstuffedTot;
        this.unstuffedAreaCurrent = this.unstuffedAreaTotal;
        this.stuffingMaskTexture = texture;
    }
    public float GetStuffedAmount() {
        return this.unstuffedAreaCurrent / unstuffedAreaTotal;
    }

}
