using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageScripts;
using Tool;


[CreateAssetMenu]
public class DamageTypeScriptableObject : ScriptableObject
{
    public DamageType damageType;
    public Sprite sprite;
    public Collider2D collider;
    public ToolType correctToolType;
}