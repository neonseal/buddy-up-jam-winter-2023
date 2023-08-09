using DG.Tweening;
using PlayArea;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClientCardCanvasManager : MonoBehaviour {
    [Header("Client Card Canvas UI Elements")]
    [SerializeField] private Button sendThankYouBtn;
    [SerializeField] private GameObject clientCardStack;

    [Header("Game Object Elements")]
    private Plushie currentPlushie;
    private ClientCard clientCard;
    private List<ClientCard> cardStack;

    [Header("Tweening Elements")]
    [SerializeField] private float duration;
    [SerializeField] private Ease ease;
    [SerializeField] private float scaleX;
    [SerializeField] private float scaleY;

    private void Awake() {
        DOTween.Init();
        Workspace.OnClientPlushieloaded += HandlePlushieLoadEvent;
        sendThankYouBtn.onClick.AddListener(SendClientCard);
        ClientCard.OnClientCardClick += MoveClientCardToBoard;

        cardStack = new List<ClientCard>();
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
        clientCard.transform.localPosition = new Vector3(0, 1500, 0);
        clientCard.name = $"{currentPlushie.name} Client Card";
        yield return new WaitForSeconds(.5f);

        // Tween the card into view
        clientCard.transform.DOLocalMove(new Vector3(0, 0, -1), 1.5f).SetEase(Ease.OutBack);

        cardStack.Add(clientCard);
    }

    private void MoveClientCardToBoard(ClientCard clientCard) {
        if (clientCard == this.clientCard) {
            clientCard.transform.SetParent(this.clientCardStack.transform);

            Debug.Log(clientCard.transform.localPosition);
            Debug.Log(clientCard.transform.localScale);

            Vector3 v = clientCard.transform.localPosition.x == 0 ? new Vector3(-700, -300, 0) : new Vector3(0, 0, 1);
            float s_x = clientCard.transform.localScale.x > scaleX ? scaleX : 5f;
            float s_y = clientCard.transform.localScale.y > scaleY ? scaleY : 7f;

            Sequence sequence = DOTween.Sequence();
            sequence.Append(clientCard.transform.DOLocalMove(v, duration));
            sequence.Insert(0, clientCard.transform.DOScaleX(s_x, duration));
            sequence.Insert(0, clientCard.transform.DOScaleY(s_y, duration));
            sequence.Play();
        }
    }

}
