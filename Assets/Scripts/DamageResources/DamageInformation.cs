using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DamageResources
{
    public sealed class DamageInformation
    {
        public DamageInformation(string spriteFilepath)
        {
            this.sprite = Resources.Load<Sprite>(spriteFilepath); ;
        }
        public readonly Sprite sprite;
    }
}