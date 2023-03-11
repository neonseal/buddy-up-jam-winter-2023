using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;
using DG.Tweening;
using UnityEngine.UI;

public class MendingGameController : MonoBehaviour {
    /* Mending Game component for generating different mini game sprites */
    private MendingGames mendingGame;
    /* Lense Sprite on which we'll render the dashed line */
    private SpriteRenderer lenseSpriteRenderer;

    private Vector3 startingPosition;
    [SerializeField] private float duration = 0.5f;
    [SerializeField] private Ease easeType = Ease.InCirc;

    private void Awake() {
        mendingGame = GetComponentInChildren<MendingGames>();
        lenseSpriteRenderer = mendingGame.gameObject.GetComponent<SpriteRenderer>();

        startingPosition = this.transform.position;
        CustomEventManager.Current.onStartRepairMiniGame += StartRepairMiniGame;
        CustomEventManager.Current.onRepairDamage_Complete += StopRepairMiniGame;
    }

    private void StartRepairMiniGame(PlushieDamage plushieDamage, DamageType damageType) {
        DOTween.Init();


        Vector3 lensePosition = lenseSpriteRenderer.transform.position;
        List<Vector3> targetPositions = new List<Vector3> {
            new Vector3(lensePosition.x - 1, lensePosition.y + 2, -1),
            new Vector3(lensePosition.x + 1, lensePosition.y, -1),
            new Vector3(lensePosition.x - 1, lensePosition.y - 2, -1),
        };
        mendingGame.GenerateNewSewingGame(targetPositions, plushieDamage);

        this.transform.DOLocalMove(new Vector3(0, 0, -1), duration).SetEase(easeType);
    }

    private void StopRepairMiniGame(PlushieDamage plushieDamage) {
        this.transform.DOLocalMove(startingPosition, duration).SetEase(easeType);
        plushieDamage.deletePlushieDamage();
    }
}
