using Dialogue;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClientCard : MonoBehaviour, IPointerClickHandler {
    public bool MovedToBoard = false;
    public bool InitialInstantiation = false;
    [SerializeField] public TutorialSequenceScriptableObject TutorialSequence;
    public TutorialManager TutorialManager;

    public static event Action<ClientCard> OnClientCardInitialClick;
    public static event Action<ClientCard> OnClientCardClick;

    public void OnPointerClick(PointerEventData eventData) {
        if (MovedToBoard) {
            OnClientCardClick?.Invoke(this);
        } else {
            MovedToBoard = true;
            OnClientCardInitialClick?.Invoke(this);
        }

        if (TutorialSequence != null && TutorialManager.TutorialActive) {
            TutorialManager.ContinueTutorialSequence();
        }
    }
}
