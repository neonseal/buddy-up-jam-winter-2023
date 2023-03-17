using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameLoopManager : MonoBehaviour {
    [SerializeField]
    private DialogueManager dialogueManager;
    [SerializeField]
    private List<PlushieScriptableObject> plushieList;
    [SerializeField]
    private CardStack cardStackController;
    [SerializeField]
    private GameObject cardSpawner;

    private List<ClientCard> clientCardCollection;
    private PlushieScriptableObject currentPlushieScriptableObject;
    private int plushieListCursor;

    #if UNITY_EDITOR
        [UnityEditor.Callbacks.DidReloadScripts]
        private static void OnScriptsReloaded() {
            if (UnityEditor.EditorApplication.isPlaying) {
                SceneHelper.ReloadScene();
            }
        }
    #endif

    private void Awake() {
        DOTween.Init();
    }

    private void Start() {
        // Initialize plushie list cursor
        this.plushieListCursor = 0;
        this.clientCardCollection = new List<ClientCard>();

        // Subscribe methods to event triggers
        CustomEventManager.Current.onGameStart += this.StartGame;
        PlushieLifeCycleEventManager.Current.onFinishPlushieRepair += this.PlushieSendoff;
    }

    // Update the scene to bring in a new customer's plushie, note, and information
    private void StartGame() {
        StartCoroutine(StartNextCustomerRoutine());
    }

    IEnumerator StartNextCustomerRoutine() {
        // Set current plushie scriptable object
        currentPlushieScriptableObject = plushieList[plushieListCursor];

        // Set client dialogue font
        this.dialogueManager.SetClientFont(currentPlushieScriptableObject.clientFont);

        yield return new WaitForSeconds(.5f);

        CustomEventManager.Current.TriggerDialogue(currentPlushieScriptableObject);
    }

    private void PlushieSendoff() {
        StartCoroutine(PlushieSendoffRoutine());
    }

    IEnumerator PlushieSendoffRoutine() {
        /* Complete Repair and Send Plushie to Customer */
        // Play repair complete fanfare
        // Wait briefly
        yield return new WaitForSeconds(.4f);
        // Move plushie off screen and destroy it
        PlushieLifeCycleEventManager.Current.sendOffPlushie();

        /* Show Client Resolution Card */
        // Create resolution text object, and instantiate above the screen
        ClientCard clientCard = currentPlushieScriptableObject.resolutionClientCard;
        clientCard.name = currentPlushieScriptableObject.plushieObjectName + "ClientCard";
        clientCard.gameObject.transform.localScale = new Vector3(5, 7, 1);
        ClientCard newCard = Instantiate(clientCard, this.cardSpawner.transform.position, Quaternion.identity, this.cardStackController.transform);

        //yield return new WaitForSeconds(2f);

        // Tween the card into view
        Sequence sequence = DOTween.Sequence();
        sequence.Append(newCard.transform.DOLocalMove(new Vector3(0, 0, -1), 1.5f).SetEase(Ease.OutBack));
        DOTween.Play(sequence);

        clientCardCollection.Add(newCard);
    }

    public void PlayCardAnimation() {
        Debug.Log("PLAY");
        ClientCard card = clientCardCollection[0];
        Debug.Log("CARD: " + card.name);

        float yPos = card.transform.localPosition.y;
        float targetY = yPos == 0f ? 1000f : 0f;

        Debug.Log("CURRENT POSITION: " + card.transform.localPosition);
        Debug.Log("TARGET: " + targetY);

        Sequence sequence = DOTween.Sequence();
        sequence.Append(card.transform.DOLocalMove(new Vector3(0, targetY, -1), 1.5f).SetEase(Ease.InOutBack));
        DOTween.Play(sequence);
    }
}
