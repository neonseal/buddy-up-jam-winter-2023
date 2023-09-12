using DG.Tweening;
using Dialogue;
using PlayArea;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientCardCanvasManager : MonoBehaviour {
    [Header("Client Card Canvas UI Elements")]
    [SerializeField] private ClientCardContainer clientCardContainer;
    private Vector3 lastCardPosition;
    private float lastCardZRotation;
    [SerializeField] private float positionRandomizerMinMax;
    [SerializeField] private float rotationRandomizerMinMax;

    [Header("Game Object Elements")]
    private Plushie currentPlushie;
    private ClientCard clientCard;
    private List<ClientCard> cardCollection;

    [Header("Tweening Elements")]
    [SerializeField] private float duration;
    [SerializeField] private Ease ease;
    [SerializeField] private float scaleX;
    [SerializeField] private float scaleY;

    [SerializeField] public TutorialManager TutorialManager;

    private void Awake() {
        DOTween.Init();
        Workspace.OnClientPlushieloaded += HandlePlushieLoadEvent;
        ClientCard.OnClientCardInitialClick += MoveClientCardToBoard;
        Plushie.OnPlushieSendOffComplete += SendClientCard;

        cardCollection = new List<ClientCard>();
    }

    private void HandlePlushieLoadEvent(Plushie currentPlushie) {
        this.currentPlushie = currentPlushie;
    }

    private void SendClientCard() {
        if (currentPlushie != null) {
            StartCoroutine("SendClientCardRoutine");
        }
    }

    private IEnumerator SendClientCardRoutine() {
        clientCard = Instantiate(currentPlushie.ResolutionClientCard, this.transform, false);
        clientCard.InitialInstantiation = true;
        clientCard.transform.localPosition = new Vector3(0, 1500, 0);
        clientCard.name = $"{currentPlushie.name} Client Card";
        yield return new WaitForSeconds(.5f);

        // Tween the card into view
        clientCard.transform.DOLocalMove(new Vector3(0, 0, 0), 1.5f).SetEase(Ease.OutBack);
        yield return new WaitForSeconds(1.75f);

        if (clientCard.TutorialSequence) {
            clientCard.TutorialManager = this.TutorialManager;
            TutorialManager.StartTutorialSequence(clientCard.TutorialSequence);
        }

        cardCollection.Add(clientCard);


    }

    private void MoveClientCardToBoard(ClientCard clientCard) {
        if (clientCard == this.clientCard) {
            clientCard.transform.SetParent(this.clientCardContainer.transform);
            // Calculate new position for card on stack
            Vector3 newCardPosition = lastCardPosition;
            float newCardZRotation = lastCardZRotation;

            // Get random values for new position and rotation
            float randomX = Random.Range(-positionRandomizerMinMax, positionRandomizerMinMax);
            float randomY = Random.Range(-positionRandomizerMinMax, positionRandomizerMinMax);
            newCardPosition = new Vector3(randomX, randomY, 0);
            newCardZRotation = Random.Range(-rotationRandomizerMinMax, rotationRandomizerMinMax);

            // Negate rotation if new value too close to previous value
            float rotDiff = Mathf.Abs(newCardZRotation) - Mathf.Abs(lastCardZRotation);
            if (Mathf.Abs(rotDiff) <= 0.5f) {
                newCardZRotation *= -1;
            }

            // Tween to new position and rotation on board
            Sequence sequence = DOTween.Sequence();
            sequence.Append(clientCard.transform.DOLocalMove(newCardPosition, duration));
            sequence.Insert(0, clientCard.transform.DOScaleX(scaleX, duration));
            sequence.Insert(0, clientCard.transform.DOScaleY(scaleY, duration));
            sequence.Insert(0, clientCard.transform.DORotate(new Vector3(0, 0, newCardZRotation), duration));
            sequence.Play();

            // Update board and last values
            clientCardContainer.UpdateCardStack();
            clientCard.MovedToBoard = true;
            lastCardPosition = newCardPosition;
            lastCardZRotation = newCardZRotation;
        }
    }

}
