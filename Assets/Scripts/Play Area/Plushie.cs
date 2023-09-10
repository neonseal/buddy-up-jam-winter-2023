using DG.Tweening;
using Dialogue;
using System;
using System.Collections;
using UnityEngine;

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
    [SerializeField] private ClientCard resolutionClientCard;

    [Header("Tutorial Dialogue")]
    public bool HasTutorialDialogue;
    public TutorialSequenceScriptableObject TutorialSequenceScriptableObject;

    [SerializeField] private float duration;

    public PlushieDamageGO[] PlushieDamageList { get; private set; }

    public static event Action OnPlushieSendOffComplete;

    private void Awake() {
        DOTween.Init();
        PlushieDamageList = GetComponentsInChildren<PlushieDamageGO>();
    }

    public void SendOffPlushie() {
        StartCoroutine("SendOffRoutine");
    }

    public IEnumerator SendOffRoutine() {
        this.transform.DOLocalMoveY(20, duration);
        yield return new WaitForSeconds(1f);
        OnPlushieSendOffComplete?.Invoke();
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    public TMPro.TMP_FontAsset ClientFont { get => clientFont; }
    public int NameFontSize { get => nameFontSize; }
    public int DialogueFontSize { get => dialogueFontSize; }
    public bool NameBolded { get => nameBolded; }
    public bool DialogueBolder { get => dialogueBolded; }
    public int LineSpacingValue { get => lineSpacingValue; }
    public int CharacterSpacingValue { get => characterSpacingValue; }
    public int WordSpacingValue { get => wordSpacingValue; }
    public ClientCard ResolutionClientCard { get => resolutionClientCard; }

}