using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ClientCardController : MonoBehaviour, IPointerDownHandler {
    //[SerializeField] private Transform card;
    [SerializeField] private float cycleLength = 2f;
    [SerializeField] private Ease easeType;
    [SerializeField] private float targetScale;

    private Vector3 startingPosition;

    private void Awake() {
        DOTween.Init();
        startingPosition = this.gameObject.transform.position;
        easeType = Ease.InCirc;
    }

    public void OnPointerDown(PointerEventData eventData) {
        if (this.gameObject.transform.position == startingPosition) {
            this.gameObject.transform.DOMove(new Vector3(-7, 3), cycleLength).SetEase(easeType);
            //this.gameObject.transform.DOScale(targetScale, cycleLength);
        } else {
            this.gameObject.transform.DOMove(startingPosition, cycleLength).SetEase(easeType);
        }
    }
}
