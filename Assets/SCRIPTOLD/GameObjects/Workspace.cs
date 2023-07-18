/*using System.Collections;
using System.Collections.Generic;
using GameLoop;
using UnityEngine;

public class Workspace : MonoBehaviour {
    private GameObject currentPlushieObject;

    private void Awake() {
        // Subscribe methods to event triggers
        PlushieLifeCycleEventManager.Current.onGeneratePlushie += this.AddPlushie;
        PlushieLifeCycleEventManager.Current.onAllRepairsComplete += this.UpdatePlushieOnRepair;
    }

    private void AddPlushie(PlushieScriptableObject plushieScriptableObject) {
        if (plushieScriptableObject == null) {
            Debug.LogError("Plushie Scriptable Object must not be null");
        }
        GameObject plushie = new GameObject();
        plushie.transform.SetParent(this.transform);
        plushie.name = plushieScriptableObject.plushieObjectName + "Plushie";
        plushie.transform.localScale = plushieScriptableObject.plushieScale;

        Plushie_old plushieComponent = plushie.AddComponent<Plushie_old>();
        plushieComponent.sprite = plushieScriptableObject.damagedPlushieSprite;

        for (int i = 0; i < plushieScriptableObject.damageTypeList.Count; i++) {
            plushieComponent.AddPlushieDamageToScene(plushieScriptableObject, i);
        }

        currentPlushieObject = plushie;

        // Trigger tutorial sequence if needed
        if (plushieScriptableObject.hasTutorialDialogue) {
            TutorialSequenceEventManager.Current.StartTutorialSequence(plushieScriptableObject.TutorialSequenceScriptableObject);
        }
    }

    private void UpdatePlushieOnRepair() {
        this.currentPlushieObject.GetComponent<SpriteRenderer>().sprite = GameLoopManager.currentPlushieScriptableObject.repairedPlushieSprite;
    }
}
*/