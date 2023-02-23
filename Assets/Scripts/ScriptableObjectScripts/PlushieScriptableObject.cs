using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageScripts;

[CreateAssetMenu (fileName = "Plushie", menuName = "Scriptable Objects/Plushie")]
public class PlushieScriptableObject : ScriptableObject
{
    public string plushieName;
    public Sprite plushieSprite;
    public string backstory;
    public List<DamageType> damageTypeList;
    public List<Vector2> damagePositionList;
}
