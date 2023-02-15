using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DamageResoueces
{
    public static class DamageTypes
    {
        static DamageTypes()
        {
            damageInfoDictionary = new Dictionary<plushieDamageType, DamageInformation>();
            damageInfoDictionary.Add(plushieDamageType.CUT, new DamageInformation("Sprites/Damage_Cut"));
            damageInfoDictionary.Add(plushieDamageType.DIRT, new DamageInformation("Sprites/Damage_Dirt"));
        }

        public enum plushieDamageType
        {
            CUT,
            DIRT
        }

        public static Dictionary<plushieDamageType, DamageInformation> damageInfoDictionary;
    }
}