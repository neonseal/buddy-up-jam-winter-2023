using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DamageScripts {
    public static class DamageDictionary {
        static DamageDictionary() {
            damageInfoDictionary = new Dictionary<DamageType, DamageTypeScriptableObject>();
            damageInfoDictionary.Add(DamageType.SMALL_RIP, Resources.Load<DamageTypeScriptableObject>("ScriptableObjects/PlushieDamage/SmallRip"));
            damageInfoDictionary.Add(DamageType.LARGE_RIP, Resources.Load<DamageTypeScriptableObject>("ScriptableObjects/PlushieDamage/LargeRip"));
            damageInfoDictionary.Add(DamageType.LARGE_RIP_STUFFED, Resources.Load<DamageTypeScriptableObject>("ScriptableObjects/PlushieDamage/LargeRipStuffed"));
        }

        public static Dictionary<DamageType, DamageTypeScriptableObject> damageInfoDictionary;
    }
}