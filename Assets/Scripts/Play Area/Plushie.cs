using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* User-defined Namespaces */
using Dialogue;

public class Plushie : MonoBehaviour {
    [Header("Client Name")]
    [SerializeField] private string clientName;

    [Header("Font Styling")]
    [SerializeField] private TMPro.TMP_FontAsset clientFont;
    [SerializeField] private int nameFontSize;
    [SerializeField] private int dialogueFontSize;
    [SerializeField] private bool nameBolded;
    [SerializeField] private bool dialogueBolded;
    [SerializeField] private int lineSpacingValue;
    [SerializeField] private int characterSpacingValue;
    [SerializeField] private int wordSpacingValue;

    [Header("Issue and Resolution Elements")]
    public ClientDialogueSet IssueDialogue;
    //[SerializeField] private ClientCard resolutionClientCard;

    [Header("Tutorial Dialogue")]
    public bool HasTutorialDialogue;
    public TutorialSequenceScriptableObject TutorialSequenceScriptableObject;
}