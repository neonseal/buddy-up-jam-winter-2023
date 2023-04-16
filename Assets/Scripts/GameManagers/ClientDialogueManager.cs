using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClientDialogueManager : MonoBehaviour {
    [Header("UI Components")]
    [SerializeField]
    private TMP_Text nameText;
    [SerializeField]
    private TMP_Text dialogueText;
    [SerializeField]
    private Button continueButton;
    [SerializeField]
    private Slider fontToggle;

    [Header("Animation Variables")]
    [SerializeField]
    private Animator animator;
    private float defaultAnimationDelay;
    private float animationDelay;
    private bool animationPlaying;
    private Queue<string> sentences;
    private bool clientFontVisible;

    [Header("Font Variables")]
    private TMP_FontAsset openDyslexicFont;
    private TMP_FontAsset clientFont;
    private int clientTitleSize;
    private int clientFontSize;
    private int openDyslexicTitleSize;
    private int openDyslexicFontSize;


    private PlushieScriptableObject currentPlushie;

    // Setup singleton pattern
    private static ClientDialogueManager current;
    public static ClientDialogueManager Current {
        get {
            if (current == null) {
                current = FindAnyObjectByType(typeof(ClientDialogueManager)) as ClientDialogueManager;
            }
            return current;
        }
        set { current = value; }
    }

    private void Awake() {
        sentences = new Queue<string>();
        defaultAnimationDelay = 0.025f;
        animationDelay = defaultAnimationDelay;
        animationPlaying = false;

        clientFontVisible = true;
        openDyslexicFont = Resources.Load<TMP_FontAsset>("Fonts/OpenDyslexic3-Regular SDF");
        openDyslexicTitleSize = 40;
        openDyslexicFontSize = 28;

        continueButton.onClick.AddListener(DisplayNextSentence);
        CustomEventManager.Current.onTriggerDialogue += StartDialogue;

    }

    private void StartDialogue(PlushieScriptableObject plushieScriptableObject) {
        this.currentPlushie = plushieScriptableObject;
        StartCoroutine(StartDialogueRoutine(currentPlushie.issueDialogue));
    }

    public void DisplayNextSentence() {
        if (sentences.Count <= 0) {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));

        if (sentences.Count == 0) {
            continueButton.gameObject.GetComponentInChildren<TMP_Text>().text = "END";
        }
    }

    IEnumerator StartDialogueRoutine(Dialogue dialogue) {
        // Initialize Dialogue Box for current speaker
        nameText.text = dialogue.name;
        dialogueText.text = "";

        animator.SetBool("isOpen", true);
        // Pause briefly to allow animation to finish before rendering text
        yield return new WaitForSeconds(.5f);


        sentences.Clear();

        foreach (string sentence in dialogue.sentences) {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    IEnumerator TypeSentence(string sentence) {
        dialogueText.text = "";

        foreach (char letter in sentence.ToCharArray()) {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.025f);
        }
    }

    private void EndDialogue() {
        animator.SetBool("isOpen", false);
        // Broadcasts an event to initialize a plushie
        PlushieLifeCycleEventManager.Current.generatePlushie(currentPlushie);
    }

    private void SetFont(bool clientFontVisibility) {
        // Update client font visible boolean 
        this.clientFontVisible = clientFontVisibility;

        nameText.font = this.clientFontVisible ? this.clientFont : this.openDyslexicFont;
        nameText.fontSize = this.clientFontVisible ? this.clientTitleSize : this.openDyslexicTitleSize;

        dialogueText.font = this.clientFontVisible ? this.clientFont : this.openDyslexicFont;
        dialogueText.fontSize = this.clientFontVisible ? this.clientFontSize : this.openDyslexicFontSize;
    }

    public void SetClientStyling(PlushieScriptableObject clientPlushieObject) {
        this.clientFontVisible = true;
        this.clientFont = clientPlushieObject.clientFont;
        this.clientFontSize = clientPlushieObject.dialogueFontSize;
        this.clientTitleSize = clientPlushieObject.nameFontSize;
        this.SetFont(this.clientFont);
    }

    public void SwitchFonts() {
        // Switch to Open Dyslexic if showing client specific font
        this.SetFont(!clientFontVisible);
    }
}
