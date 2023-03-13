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

    private Vector3 homePosition;
    private Vector3 centerPosition;
    [SerializeField] private float duration = 150f;
    private float startTime;

    private void Awake() {
        mendingGame = GetComponentInChildren<MendingGames>();
        lenseSpriteRenderer = mendingGame.gameObject.GetComponent<SpriteRenderer>();

        homePosition = this.transform.position;
        centerPosition = new Vector3(0, 0, -1);
        CustomEventManager.Current.onStartRepairMiniGame += StartRepairMiniGame;
        CustomEventManager.Current.onRepairDamage_Complete += StopRepairMiniGame;
    }


    private void Update() {

    }

    private void StartRepairMiniGame(PlushieDamage plushieDamage, DamageType damageType) {
        Vector3 lensePosition = lenseSpriteRenderer.transform.position;
        List<Vector3> targetPositions = new List<Vector3> {
            new Vector3(lensePosition.x - 1, lensePosition.y + 2, -1),
            new Vector3(lensePosition.x + 1, lensePosition.y, -1),
            new Vector3(lensePosition.x - 1, lensePosition.y - 2, -1),
        };
        mendingGame.GenerateNewSewingGame(targetPositions, plushieDamage);
        StartCoroutine(MoveLenseIntoFocus(homePosition, centerPosition));
    }

    private IEnumerator MoveLenseIntoFocus(Vector3 start, Vector3 end) {
        startTime = Time.time;

        // Calculate the center of the arc
        Vector3 center = (start + end) * 0.5F;
        center -= new Vector3(2, 6, 0);

        // Interpolate over the arc relative to center
        Vector3 startRelCenter = start - center;
        Vector3 endRelCenter = end - center;

        // Calculate the fraction of animation that has been completed so far
        float fracComplete = (Time.time - startTime) / duration;

        // Perform position Slerp
        for (float t = 0f; t < duration; t += (Time.time - startTime) / duration*2f) {
            this.transform.position = Vector3.Slerp(startRelCenter, endRelCenter, t);

            this.transform.position += center;

            yield return null;
        }
    }

    private void StopRepairMiniGame(PlushieDamage plushieDamage) {
        StartCoroutine(MoveLenseIntoFocus(centerPosition, homePosition));
        plushieDamage.deletePlushieDamage();
        mendingGame.DestroyAllGameElements();
    }
}
