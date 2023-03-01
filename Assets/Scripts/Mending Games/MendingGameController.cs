using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageScripts;
using DG.Tweening;

public class MendingGameController : MonoBehaviour
{
    private MendingGames mendingGame;
    private Vector3 startingPosition;
    [SerializeField] private float duration = 0.5f;
    [SerializeField] private Ease easeType = Ease.InCirc;

    private void Awake() {
        mendingGame = GetComponentInChildren<MendingGames>();
        startingPosition = this.transform.position;
        CustomEventManager.Current.onStartRepairMiniGame += StartRepairMiniGame;
        CustomEventManager.Current.onRepairCompletion += StopRepairMiniGame;
    }

    private void StartRepairMiniGame(PlushieDamage plushieDamage, DamageType damageType) {
        DOTween.Init();


        this.transform.DOLocalMove(new Vector3(0, 0, -1), duration).SetEase(easeType);
        mendingGame.CreateSewingMiniGame(plushieDamage);
    }

    private void StopRepairMiniGame(PlushieDamage plushieDamage) {
        this.transform.DOLocalMove(startingPosition, duration).SetEase(easeType);
        plushieDamage.deletePlushieDamage();
    }
}
