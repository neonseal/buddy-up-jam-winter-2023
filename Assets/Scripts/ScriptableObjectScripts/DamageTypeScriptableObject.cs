using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;

[CreateAssetMenu (fileName = "Damage Type", menuName = "Scriptable Objects/Damage Type")]
public class DamageTypeScriptableObject : ScriptableObject
{
    public DamageType damageType;
    public Sprite sprite;
    public Texture2D repairIncompleteTexture;
    public Texture2D repairCompleteTexture;
    public ToolType correctToolType;
    public string damageChecklistMessage;

}