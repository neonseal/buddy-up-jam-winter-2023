using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardController : MonoBehaviour
{
    public CardScriptableObject cardDescriptionData;
    private TMP_Text[] descriptionZones;

    private void Awake() {
        descriptionZones = GetComponentsInChildren<TMP_Text>();
        descriptionZones[0].text = cardDescriptionData.description_part1;
        descriptionZones[1].text = cardDescriptionData.description_part2;
    }


}
