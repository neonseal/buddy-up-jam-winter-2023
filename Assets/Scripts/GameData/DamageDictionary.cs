using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameData {
    public static class DamageDictionary {
        static DamageDictionary() {
            damageInfoDictionary = new Dictionary<DamageType, DamageTypeScriptableObject>();
            damageInfoDictionary.Add(DamageType.SmallRip, Resources.Load<DamageTypeScriptableObject>("ScriptableObjects/PlushieDamage/SmallRip"));
            damageInfoDictionary.Add(DamageType.LargeRip, Resources.Load<DamageTypeScriptableObject>("ScriptableObjects/PlushieDamage/LargeRip"));
            damageInfoDictionary.Add(DamageType.LargeRipMissingStuffing, Resources.Load<DamageTypeScriptableObject>("ScriptableObjects/PlushieDamage/LargeRipMissingStuffing"));
            damageInfoDictionary.Add(DamageType.WornStuffing, Resources.Load<DamageTypeScriptableObject>("ScriptableObjects/PlushieDamage/WornStuffing"));
        }

        public static Dictionary<DamageType, DamageTypeScriptableObject> damageInfoDictionary;
    }
}