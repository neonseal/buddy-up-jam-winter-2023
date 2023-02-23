using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;

public class CardStackController : MonoBehaviour, IPointerDownHandler {
    [SerializeField] private int xPos = 0;
    [SerializeField] private int yPos = 0;

    [SerializeField] private float cycleLength = 1.5f;
    [SerializeField] private Ease easeType;

    private Vector3 startingPosition;
    public bool moving;

    private void Awake() {
        DOTween.Init();
        startingPosition = this.gameObject.transform.localPosition;
    }

    private void Update() {
        if (this.gameObject.transform.localPosition == startingPosition || this.gameObject.transform.localPosition == new Vector3(xPos, yPos)) {
            moving = false;
        }
    }

    public void OnPointerDown(PointerEventData eventData) {
        Sequence sequence = DOTween.Sequence();
        if (!moving) {
            if (this.gameObject.transform.localPosition == startingPosition) {
                moving = true;
                sequence.Append(transform.DOLocalMove(new Vector3(xPos, yPos), cycleLength).SetEase(Ease.InCubic));
                sequence.Insert(0, transform.GetComponentInChildren<Image>().transform.DOScale(new Vector3(6, 9, 1), cycleLength)).SetEase(Ease.Unset);
            } else {
                moving = true;
                sequence.Append(transform.DOLocalMove(startingPosition, cycleLength).SetEase(Ease.InCubic));
                sequence.Insert(0, transform.GetComponentInChildren<Image>().transform.DOScale(new Vector3(2, 2.5f, 1), cycleLength)).SetEase(Ease.Unset);
            }
        }
    }
}
