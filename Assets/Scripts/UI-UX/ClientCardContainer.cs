using DG.Tweening;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClientCardContainer : MonoBehaviour, IPointerClickHandler {
    private ClientCard[] cards;

    [Header("Card Stack Navigation Elements")]
    [SerializeField] private Button nextCardBtn;
    [SerializeField] private Button prevCardBtn;
    [SerializeField] private float clickSizeModifier;
    [SerializeField] private float clickDuration;

    [Header("Tweening Elements")]
    [SerializeField] private Vector3 screenHomePosition;
    [SerializeField] private float duration;
    [SerializeField] private Ease ease;
    [SerializeField] private float scaleX;
    [SerializeField] private float scaleY;

    private void Awake() {
        DOTween.Init();
        ClientCard.OnClientCardClick += HandleClientCardClick;

        nextCardBtn.onClick.AddListener(ShowNextCard);
        prevCardBtn.onClick.AddListener(ShowPrevCard);
    }

    private void HandleClientCardClick(ClientCard card) {
        if (cards != null && cards.Contains(card)) {
            FocusOnCardContainer();
        }
    }

    private void FocusOnCardContainer() {
        Vector3 v = this.transform.localPosition.x == 0 ? screenHomePosition : new Vector3(0, 0, 1);
        float s_x = this.transform.localScale.x < scaleX ? scaleX : 1f;
        float s_y = this.transform.localScale.y < scaleY ? scaleY : 1f;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(this.transform.DOLocalMove(v, duration));
        sequence.Insert(0, this.transform.transform.DOScaleX(s_x, duration));
        sequence.Insert(0, this.transform.transform.DOScaleY(s_y, duration));
        sequence.Play();
    }

    private void ShowNextCard() {
        Debug.Log("SHOW NEXT CARD");
        Sequence sequence = DOTween.Sequence();
        sequence.Append(nextCardBtn.transform.DOScale(clickSizeModifier, clickDuration));
        sequence.SetLoops(2, LoopType.Yoyo);
    }

    private void ShowPrevCard() {
        Debug.Log("SHOW NEXT CARD");
        Sequence sequence = DOTween.Sequence();
        sequence.Append(prevCardBtn.transform.DOScale(clickSizeModifier, clickDuration));
        sequence.SetLoops(2, LoopType.Yoyo);
    }

    public void OnPointerClick(PointerEventData eventData) {
        FocusOnCardContainer();
    }

    public void UpdateCardStack() {
        cards = this.gameObject.GetComponentsInChildren<ClientCard>();
    }
}
