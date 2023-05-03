using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;
using DG.Tweening;
using UnityEngine.UI;

public class MendingGameController : MonoBehaviour {
    /* Mending Game component for generating different mini game sprites */
    private MendingGames mendingGame;
    private StuffingGames stuffingGame;

    /* Lense Sprite on which we'll render the dashed line */
    private SpriteRenderer lenseSpriteRenderer;

    private Vector3 homePosition;

    [SerializeField] private float duration;
    [SerializeField] private GameObject checklist;

    [Header("Tutorial Interaction Variables")]
    private bool tutorialActionRequired;

    private void Awake() {
        DOTween.Init();
        duration = 0.85f;

        mendingGame = GetComponentInChildren<MendingGames>();
        stuffingGame = GetComponentInChildren<StuffingGames>();

        lenseSpriteRenderer = mendingGame.gameObject.GetComponent<SpriteRenderer>();

        homePosition = this.transform.position;

        tutorialActionRequired = false;

        DamageLifeCycleEventManager.Current.onStartRepairMiniGame += StartRepairMiniGame;
        MendingGameEventManager.Current.onMendingGameComplete += StopRepairMiniGame;
        TutorialSequenceEventManager.Current.onRequiredRepairCompletionAction += () => { tutorialActionRequired = true; };
    }


    private void StartRepairMiniGame(PlushieDamage plushieDamage, DamageType damageType) {
        Vector3 lensePosition = lenseSpriteRenderer.transform.position;
        List<Vector3> targetPositions;
        // Check damage type to determine which repair game to create
        switch (damageType) {
            case DamageType.SmallRip:
                targetPositions = new List<Vector3> {
                    new Vector3(lensePosition.x - 1, lensePosition.y + 2, -1),
                    new Vector3(lensePosition.x + 1, lensePosition.y, -1),
                    new Vector3(lensePosition.x - 1, lensePosition.y - 2, -1),
                };
                mendingGame.CreateSewingGame(targetPositions, plushieDamage);
                break;
            case DamageType.LargeRipMissingStuffing:
                stuffingGame.StartGameRoutine(mendingGame, plushieDamage);
                break;
            case DamageType.WornStuffing:
                break;
        }



        this.transform.DOLocalMove(new Vector3(-4.375f, 0, -1), duration).SetEase(Ease.InCirc);
        this.checklist.SetActive(true);
    }

    private void StopRepairMiniGame(PlushieDamage plushieDamage) {
        // Move and clear repair game
        this.transform.DOLocalMove(homePosition, duration).SetEase(Ease.InCirc);
        plushieDamage.deletePlushieDamage();
        mendingGame.ResetAllElements();

        // Update checklist
        DamageLifeCycleEventManager.Current.repairDamage_Complete(plushieDamage);
        this.checklist.SetActive(true);

        if (tutorialActionRequired) {
            tutorialActionRequired = false;
            StartCoroutine(TutorialSequenceEventManager.Current.HandleTutorialRequiredActionCompletion());
        }
    }
}
