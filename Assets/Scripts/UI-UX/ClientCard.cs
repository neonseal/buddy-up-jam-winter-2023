using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClientCard : MonoBehaviour, IPointerClickHandler {
    private bool movedToBoard = false;
    public static event Action<ClientCard> OnClientCardClick;

    public void OnPointerClick(PointerEventData eventData) {
        OnClientCardClick?.Invoke(this);
    }
}
