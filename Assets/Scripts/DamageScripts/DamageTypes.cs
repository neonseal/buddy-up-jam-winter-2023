using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DamageScripts {
    public enum PlushieDamageType {
        SMALL_RIP,
        LARGE_RIP,
        WORN_STUFFING,
        LARGE_RIP_STUFFED
    }

    // Use ScriptableObjects instead - migrate this bit in the future
    public static class DamageTypes {
        static DamageTypes() {
            damageInfoDictionary = new Dictionary<PlushieDamageType, DamageInformation>();
            damageInfoDictionary.Add(PlushieDamageType.SMALL_RIP, new DamageInformation("Sprites/Damage_Cut"));
            damageInfoDictionary.Add(PlushieDamageType.WORN_STUFFING, new DamageInformation("Sprites/Damage_Dirt"));
        }

        public static Dictionary<PlushieDamageType, DamageInformation> damageInfoDictionary;
    }
}