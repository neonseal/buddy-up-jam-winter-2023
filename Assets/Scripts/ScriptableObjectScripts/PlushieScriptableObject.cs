using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;

[CreateAssetMenu (fileName = "Plushie", menuName = "Scriptable Objects/Plushie")]
public class PlushieScriptableObject : ScriptableObject
{
    [Header("Client Name")]
    public string plushieObjectName;
    public string clientName;

    [Header("Font Styling")]
    public TMPro.TMP_FontAsset clientFont;
    public int nameFontSize;
    public int dialogueFontSize;
    public int lineSpacingValue;
    public bool bolded;

    [Header("Plushie/Damage Elements")]
    public Sprite plushieSprite;
    public Vector3 plushieScale;

    public List<DamageType> damageTypeList;
    public List<Vector2> damageColliderSizeList;
    public List<Vector2> damagePositionList;
    public List<int> damageZRotationList;

    [Header("Issue and Resolution Elements")]
    public Dialogue issueDialogue;
    public ClientCard resolutionClientCard;

    [Header("Tutorial Dialogue")]
    public bool hasTutorialDialogue;
    public TutorialSequenceScriptableObject TutorialSequenceScriptableObject; 
}
