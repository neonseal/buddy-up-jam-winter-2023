using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class WorkspaceManager : MonoBehaviour {
    [SerializeField]
    private DialogueManager dialogueManager;
    [SerializeField]
    private List<PlushieScriptableObject> plushieList;
    [SerializeField]
    private Button nextCustomerButton;
    [SerializeField]
    private CardStackController cardStackController;
    [SerializeField]
    private GameObject cardSpawner;

    private List<ClientCard> clientCardCollection;
    private PlushieScriptableObject currentPlushieScriptableObject;
    private int plushieListCursor;

    private void Awake() {
        DOTween.Init();
        
        this.plushieListCursor = 0;
        this.clientCardCollection = new List<ClientCard>();

        CustomEventManager.Current.onGameStart += this.StartGame;
        CustomEventManager.Current.onPlushieDeletionEvent += this.PlushieSendoff;
        nextCustomerButton.onClick.AddListener(NextCustomer);
    }

    private void StartGame() {
    }

    // Update the scene to bring in a new customer's plushie, note, and information
    public void NextCustomer() {
        StartCoroutine(StartNextCustomerRoutine());
    }

    IEnumerator StartNextCustomerRoutine() {
        this.nextCustomerButton.interactable = false;

        // Set current plushie scriptable object
        currentPlushieScriptableObject = plushieList[plushieListCursor];

        // Set client dialogue font
        this.dialogueManager.SetClientFont(currentPlushieScriptableObject.clientFont);

        // Broadcasts an event to initialize a plushie
        this.AddPlushieToScene();

        yield return new WaitForSeconds(.5f);

        CustomEventManager.Current.TriggerDialogue(currentPlushieScriptableObject.issueDialogue);
    }

    private void AddPlushieToScene() {
        GameObject plushie = new GameObject();
        plushie.transform.SetParent(this.transform);
        plushie.name = currentPlushieScriptableObject.plushieObjectName + "Plushie";
        // REMOVE WHEN WE HAVE NEW SPRITES
        plushie.transform.localScale = new Vector3(4, 4, 1);

        Plushie plushieComponent = plushie.AddComponent<Plushie>();
        plushieComponent.sprite = plushieList[plushieListCursor].plushieSprite;

        for (int i = 0; i < plushieList[plushieListCursor].damageTypeList.Count; i++) {
            plushieComponent.AddPlushieDamageToScene(plushieList[plushieListCursor].damagePositionList[i], plushieList[plushieListCursor].damageTypeList[i]);
        }

        this.plushieListCursor++;
    }

    private void PlushieSendoff() {
        StartCoroutine(PlushieSendoffRoutine());
    }

    IEnumerator PlushieSendoffRoutine() {
        /* Complete Repair and Send Plushie to Customer */
        // Play repair complete fanfare
        // Wait briefly
        yield return new WaitForSeconds(.4f);
        // Move plushie off screen
        // Destroy gameobject

        /* Show Client Resolution Card */
        // Create resolution text object, and instantiate above the screen
        ClientCard clientCard = currentPlushieScriptableObject.resolutionClientCard;
        clientCard.name = currentPlushieScriptableObject.plushieObjectName + "ClientCard";
        clientCard.gameObject.transform.localScale = new Vector3(5, 7, 1);
        ClientCard newCard = Instantiate(clientCard, this.cardSpawner.transform.position, Quaternion.identity, this.cardStackController.transform);

        //yield return new WaitForSeconds(2f);

        // Tween the card into view
         Sequence sequence = DOTween.Sequence();
         sequence.Append(newCard.transform.DOLocalMove(new Vector3(0,0,-1), 1.5f).SetEase(Ease.OutBack));
         DOTween.Play(sequence);
        
        clientCardCollection.Add(newCard);

        /* Prep Space for Next Customer */
        // Enable next customer button
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
         sequence.Append(card.transform.DOLocalMove(new Vector3(0,targetY,-1), 1.5f).SetEase(Ease.InOutBack));
         DOTween.Play(sequence);
    }
}
