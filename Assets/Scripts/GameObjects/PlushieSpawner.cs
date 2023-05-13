using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlushieSpawner : MonoBehaviour {

    void Start() {
        // Subscribe methods to event triggers
        PlushieLifeCycleEventManager.Current.onGeneratePlushie += this.AddPlushie;
    }

    private void AddPlushie(PlushieScriptableObject plushieScriptableObject) {
        if (plushieScriptableObject == null) {
            Debug.LogError("Plushie Scriptable Object must not be null");
        }
        GameObject plushie = new GameObject();
        plushie.transform.SetParent(this.transform);
        plushie.name = plushieScriptableObject.plushieObjectName + "Plushie";
        plushie.transform.localScale = plushieScriptableObject.plushieScale;

        Plushie plushieComponent = plushie.AddComponent<Plushie>();
        plushieComponent.sprite = plushieScriptableObject.damagedPlushieSprite;

        for (int i = 0; i < plushieScriptableObject.damageTypeList.Count; i++) {
            plushieComponent.AddPlushieDamageToScene(plushieScriptableObject, i);
        }

        // Trigger tutorial sequence if needed
        if (plushieScriptableObject.hasTutorialDialogue) {
            TutorialSequenceEventManager.Current.StartTutorialSequence(plushieScriptableObject.TutorialSequenceScriptableObject);
        }
    }
}
