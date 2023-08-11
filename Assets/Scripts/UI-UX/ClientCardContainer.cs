using DG.Tweening;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClientCardContainer : MonoBehaviour, IPointerClickHandler {
    private ClientCard[] cards;
    private bool focused;

    [Header("Card Stack Navigation Elements")]
    [SerializeField] private Button nextCardBtn;
    [SerializeField] private Button prevCardBtn;
    [SerializeField] private float clickSizeModifier;
    [SerializeField] private float clickDuration;
    [SerializeField] private float cardCycleDistance;
    [SerializeField] private float cardCycleDuration;
    [SerializeField] private float waitForCycleAnimation;

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

        if (focused) {
            focused = false;
            nextCardBtn.gameObject.SetActive(false);
            prevCardBtn.gameObject.SetActive(false);
        } else {
            focused = true;
            nextCardBtn.gameObject.SetActive(true);
            prevCardBtn.gameObject.SetActive(true);
        }
    }

    private void ShowNextCard() {
        CycleCard(true, nextCardBtn.transform);
    }

    private void ShowPrevCard() {
        CycleCard(false, prevCardBtn.transform);
    }

    private void CycleCard(bool topToBottom, Transform transform) {
        Sequence btnSequence = DOTween.Sequence();
        btnSequence.Append(transform.DOScale(clickSizeModifier, clickDuration));
        btnSequence.SetLoops(2, LoopType.Yoyo);
        btnSequence.Play();

        StartCoroutine(CycleCardRoutine(topToBottom));
    }

    private IEnumerator CycleCardRoutine(bool topToBottom) {
        ClientCard[] cardStack = this.GetComponentsInChildren<ClientCard>();

        Sequence cardSequence = DOTween.Sequence();
        if (topToBottom) {
            ClientCard topMostCard = cardStack.Last();
            cardSequence.Append(topMostCard.transform.DOLocalMoveX(topMostCard.transform.localPosition.x + cardCycleDistance, cardCycleDuration));
            cardSequence.SetLoops(2, LoopType.Yoyo);
            cardSequence.Play();
            yield return new WaitForSeconds(waitForCycleAnimation);
            topMostCard.transform.SetAsFirstSibling();
        } else {
            ClientCard bottomMostCard = cardStack.First();
            cardSequence.Append(bottomMostCard.transform.DOLocalMoveX(bottomMostCard.transform.localPosition.x - cardCycleDistance, cardCycleDuration));
            cardSequence.SetLoops(2, LoopType.Yoyo);
            cardSequence.Play();
            yield return new WaitForSeconds(waitForCycleAnimation);
            bottomMostCard.transform.SetAsLastSibling();
        }
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (cards != null) {
            FocusOnCardContainer();
        }
    }

    public void UpdateCardStack() {
        cards = this.gameObject.GetComponentsInChildren<ClientCard>();
    }

    public int GetCardCount() { return cards != null ? cards.Length : 0; }
}
