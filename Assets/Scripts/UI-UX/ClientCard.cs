using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClientCard : MonoBehaviour, IPointerClickHandler {
    public bool MovedToBoard = false;


    public static event Action<ClientCard> OnClientCardInitialClick;
    public static event Action<ClientCard> OnClientCardClick;

    public void OnPointerClick(PointerEventData eventData) {
        if (MovedToBoard) {
            OnClientCardClick?.Invoke(this);
        } else {
            OnClientCardInitialClick?.Invoke(this);
        }
    }
}
