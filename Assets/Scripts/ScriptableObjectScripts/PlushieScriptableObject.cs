using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageScripts;

[CreateAssetMenu (fileName = "Plushie", menuName = "Scriptable Objects/Plushie")]
public class PlushieScriptableObject : ScriptableObject
{
    public string plushieObjectName;
    public string clientName;
    public TMPro.TMP_FontAsset clientFont;
    public Sprite plushieSprite;
    public List<DamageType> damageTypeList;
    public List<Vector2> damagePositionList;
    public Dialogue issueDialogue;
    public ClientCardScriptableObject clientCard;
}
