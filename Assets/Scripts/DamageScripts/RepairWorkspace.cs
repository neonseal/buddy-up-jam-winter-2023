using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairWorkspace : MonoBehaviour
{
    [SerializeField]
    private PlushieSetScriptableObject plushieSetScriptableObject;
    private List<PlushieScriptableObject> plushieList;
    private int plushieListCursor;

    private void Awake() {
        // Load in the list of plushie scriptable objects defined in plushieSetScriptableObject
        this.plushieList = this.plushieSetScriptableObject.plushieList;
    }

    private void Start() {
        this.plushieListCursor = 0;
        CustomEventManager.current.onGameStart += this.StartGame;
        CustomEventManager.current.onPlushieInitialization += this.AddPlushieToScene;
    }

    private void StartGame() {
        // Broadcasts an event to initialize a plushie
        CustomEventManager.current.initializePlushieEvent();
    }

    // Update the scene to bring in a new customer's plushie, note, and information
    private void NextCustomer() {

    }

    private void AddPlushieToScene()
    {
        GameObject plushie = new GameObject();
        plushie.transform.SetParent(this.transform);
        plushie.name = plushieList[plushieListCursor].plushieName;

        Plushie plushieComponent = plushie.AddComponent<Plushie>();
        plushieComponent.sprite = plushieList[plushieListCursor].plushieSprite;

        for (int i = 0; i < plushieList[plushieListCursor].damageTypeList.Count; i++) {
            plushieComponent.AddPlushieDamageToScene(plushieList[plushieListCursor].damagePositionList[i], plushieList[plushieListCursor].damageTypeList[i]);
        }

        this.plushieListCursor++;
    }
}
