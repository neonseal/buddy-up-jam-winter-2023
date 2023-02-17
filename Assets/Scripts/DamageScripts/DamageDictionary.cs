using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DamageScripts {
    public static class DamageDictionary {
        static DamageDictionary() {
            damageInformationDictionary = new Dictionary<DamageType, DamageTypeScriptableObject>();
            damageInformationDictionary.Add(DamageType.SMALL_RIP, Resources.Load<DamageTypeScriptableObject>("ScriptableObjects/PlushieDamage/SmallRip"));
            damageInformationDictionary.Add(DamageType.LARGE_RIP, Resources.Load<DamageTypeScriptableObject>("ScriptableObjects/PlushieDamage/LargeRip"));
            damageInformationDictionary.Add(DamageType.LARGE_RIP_STUFFED, Resources.Load<DamageTypeScriptableObject>("ScriptableObjects/PlushieDamage/LargeRipStuffed"));
        }

        public static Dictionary<DamageType, DamageTypeScriptableObject> damageInformationDictionary;
    }
}