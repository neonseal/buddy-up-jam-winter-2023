using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
    public CardScriptableObject cardDescriptionData;
    private TextMesh[] descriptionZones;

    private void Awake() {
        descriptionZones = GetComponentsInChildren<TextMesh>();
    }

    
}
