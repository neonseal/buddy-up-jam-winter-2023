/* User-defined Namespaces */
using Dialogue;
using UnityEngine;

[CreateAssetMenu(fileName = "Plushie", menuName = "Scriptable Objects/Plushie")]
public class PlushieScriptableObject : ScriptableObject {
    [Header("Client Name")]
    public string plushieObjectName;
    public string clientName;

    [Header("Font Styling")]
    public TMPro.TMP_FontAsset clientFont;
    public int nameFontSize;
    public int dialogueFontSize;
    public bool nameBolded;
    public bool dialogueBolded;
    public int lineSpacingValue;
    public int characterSpacingValue;
    public int wordSpacingValue;

    [Header("Plushie Rendering Elements")]
    public Sprite damagedPlushieSprite;
    public Sprite repairedPlushieSprite;
    public Vector3 plushieScale;
    public int spriteZRotationValue;


    [Header("Issue and Resolution Elements")]
    public ClientDialogueSet issueDialogue;
    //public ClientCard resolutionClientCard;

    [Header("Tutorial Dialogue")]
    public bool hasTutorialDialogue;
    public TutorialSequenceScriptableObject TutorialSequenceScriptableObject;
}
