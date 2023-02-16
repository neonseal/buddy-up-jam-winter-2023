using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DamageResources {
    public enum PlushieDamageType {
        SmallRip,
        LargeRip,
        WornStuffing,
        Dirty
    }
    public static class DamageTypes {
        static DamageTypes() {
            damageInfoDictionary = new Dictionary<PlushieDamageType, DamageInformation>();
            damageInfoDictionary.Add(PlushieDamageType.SmallRip, new DamageInformation("Sprites/Damage_Cut"));
            damageInfoDictionary.Add(PlushieDamageType.WornStuffing, new DamageInformation("Sprites/Damage_Dirt"));
        }


        public static Dictionary<PlushieDamageType, DamageInformation> damageInfoDictionary;
    }
}