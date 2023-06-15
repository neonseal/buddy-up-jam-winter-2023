using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;

public class PlushieDamage_new : MonoBehaviour
{
    public DamageType damageType;
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

    private void _spawnDamage() {
        this.gameObject.name = this.damageType.ToString();
    }

    private void _finishRepairing() {
        foreach (GameObject gO in this.plushieDamagesDeletedOnCompletion) {
            Destroy(gO);
        }
    }
}
