using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ClientCardController : MonoBehaviour, IPointerDownHandler {
    //[SerializeField] private Transform card;
    [SerializeField] private int xPos = 0;
    [SerializeField] private int yPos = 0;

    [SerializeField] private float cycleLength = 2f;
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
        if (!moving) {
            if (this.gameObject.transform.localPosition == startingPosition) {
                moving = true;
                this.gameObject.transform.DOLocalMove(new Vector3(xPos, yPos), cycleLength).SetEase(Ease.InCubic);
            } else {
                moving = true;
                this.gameObject.transform.DOLocalMove(startingPosition, cycleLength).SetEase(Ease.InCubic);
            }
        }
    }
}
