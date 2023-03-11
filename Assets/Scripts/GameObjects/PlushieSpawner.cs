using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlushieSpawner: MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Subscribe methods to event triggers
        CustomEventManager.Current.onGeneratePlushie += this.AddPlushie;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void AddPlushie(PlushieScriptableObject plushieScriptableObject) {
        GameObject plushie = new GameObject();
        plushie.transform.SetParent(this.transform);
        plushie.name = plushieScriptableObject.plushieObjectName + "Plushie";
        // REMOVE WHEN WE HAVE NEW SPRITES
        plushie.transform.localScale = new Vector3(4, 4, 1);

        Plushie plushieComponent = plushie.AddComponent<Plushie>();
        plushieComponent.sprite = plushieScriptableObject.plushieSprite;

        for (int i = 0; i < plushieScriptableObject.damageTypeList.Count; i++) {
            plushieComponent.AddPlushieDamageToScene(plushieScriptableObject.damagePositionList[i], plushieScriptableObject.damageTypeList[i]);
        }
    }
}
