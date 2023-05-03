using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;

public class ClientCard : MonoBehaviour, IPointerDownHandler {
    [SerializeField] private Ease easeType;
    private float cycleLength;
    private Vector3 centerPosition;
    private Vector3 boardPosition;
    private bool inCenterView;

    private void Awake() {
        DOTween.Init();
        cycleLength = 1.5f;
        inCenterView = true;
    }

    private void Start() {
        centerPosition = new Vector3(0, 0, -1);
        boardPosition = new Vector3(775, 375, -1);
    }

    private void Update() {
    }

    public void OnPointerDown(PointerEventData eventData) {
        Sequence sequence = DOTween.Sequence();
        if (inCenterView) {
            sequence.Append(this.gameObject.transform.DOScaleX(2f, cycleLength / 2));
            sequence.Insert(0, this.gameObject.transform.DOScaleY(2.5f, cycleLength / 2));
            sequence.Insert(0, this.gameObject.transform.DOLocalMove(boardPosition, cycleLength)).SetEase(Ease.InOutSine);
            inCenterView = false;
        } else {
            sequence.Append(this.gameObject.transform.DOScaleX(5f, cycleLength / 2));
            sequence.Insert(0, this.gameObject.transform.DOScaleY(7f, cycleLength / 2));
            sequence.Insert(0, this.gameObject.transform.DOLocalMove(centerPosition, cycleLength)).SetEase(Ease.InOutSine);
            inCenterView = true;
        }

    }
}
