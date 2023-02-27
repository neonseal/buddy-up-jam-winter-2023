using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkspaceManager : MonoBehaviour {
    public GameObject clientCardPrefab;
    [SerializeField]
    private DialogueManager dialogueManager;
    [SerializeField]
    private List<PlushieScriptableObject> plushieList;
    [SerializeField]
    private Button nextCustomerButton;
    private PlushieScriptableObject currentPlushieScriptableObject;
    private int plushieListCursor;

    private void Awake() {
        this.plushieListCursor = 0;
        CustomEventManager.Current.onGameStart += this.StartGame;
        CustomEventManager.Current.onPlushieOverallRepairCompletion += this.PlushieSendoff;
        nextCustomerButton.onClick.AddListener(NextCustomer);
    }

    private void Start() {
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
        // Play repair complete fanfare
        // Wait briefly
        yield return new WaitForSeconds(.4f);
        // Move plushie off screen
        // Destroy gameobject
        // Create resolution text object
        GameObject clientCard = clientCardPrefab;
        //clientCard.transform.SetParent(this.
        //transform.GetComponentInChildren<CardStackController>().transform);
        clientCard.name = currentPlushieScriptableObject.plushieObjectName + "ClientCard";
        //clientCard.transform.position = new Vector3(0, 0, -1);

        Instantiate(clientCard, new Vector3(0,0,0), Quaternion.identity);
        // Enable next customer button
    }
}
