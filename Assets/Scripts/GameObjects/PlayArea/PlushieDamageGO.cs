using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlushieDamageGO : MonoBehaviour
{
    // Initial damage type of the plushie damage
    public readonly PlushieDamageType initialPlushieDamageType;

    public List<GameObject> plushieDamagesDeletedOnCompletion;

    // Start is called before the first frame update
    void Start()
    {
        this._spawnDamage();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal PlushieDamageType getInitialDamageType() {
        return initialPlushieDamageType;
    }

    private void _spawnDamage() {
        //this.gameObject.name = this.damageType.ToString();
    }

    private void _finishRepairing() {
        foreach (GameObject gO in this.plushieDamagesDeletedOnCompletion) {
            Destroy(gO);
        }
    }
}
