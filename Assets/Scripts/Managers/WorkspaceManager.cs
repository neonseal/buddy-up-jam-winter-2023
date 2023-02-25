using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkspaceManager : MonoBehaviour {
    [SerializeField]
    private DialogueManager dialogueManager;
    [SerializeField]
    private List<PlushieScriptableObject> plushieList;
    [SerializeField]
    private Button nextCustomerButton;
    private PlushieScriptableObject currentPlushie;
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
        currentPlushie = plushieList[plushieListCursor];

        // Broadcasts an event to initialize a plushie
        this.AddPlushieToScene();

        yield return new WaitForSeconds(.5f);

        CustomEventManager.Current.TriggerDialogue(currentPlushie.issueDialogue);
    }

    private void AddPlushieToScene() {
        GameObject plushie = new GameObject();
        plushie.transform.SetParent(this.transform);
        plushie.name = plushieList[plushieListCursor].plushieName;
        // REMOVE WHEN WE HAVE NEW SPRITES
        plushie.transform.localScale = new Vector3(4, 4, 1);

        Plushie plushieComponent = plushie.AddComponent<Plushie>();
        plushieComponent.sprite = plushieList[plushieListCursor].plushieSprite;

        for (int i = 0; i < plushieList[plushieListCursor].damageTypeList.Count; i++) {
            plushieComponent.AddPlushieDamageToScene(plushieList[plushieListCursor].damagePositionList[i], plushieList[plushieListCursor].damageTypeList[i]);
        }

        this.plushieListCursor++;
    }

    private void OnBecameInvisible() {
    }

    private void PlushieSendoff() {
        StartCoroutine(PlushieSendoffRoutine());
    }

    IEnumerator PlushieSendoffRoutine() {
        // Play repair complete fanfare
        // Wait briefly
        yield return new WaitForSeconds(4f);
        // Move plushie off screen
        // Destroy gameobject
        // Enable next customer button
    }
}
