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
            damageInfoDictionary.Add(PlushieDamageType.SMALL_RIP, new DamageInformation("Sprites/Damage_SMALL_RIP"));
            damageInfoDictionary.Add(PlushieDamageType.LARGE_RIP, new DamageInformation("Sprites/Damage_LARGE_RIP"));
            damageInfoDictionary.Add(PlushieDamageType.LARGE_RIP_STUFFED, new DamageInformation("Sprites/Damage_LARGE_RIP_STUFFED"));
        }

        public static Dictionary<PlushieDamageType, DamageInformation> damageInfoDictionary;
    }
}