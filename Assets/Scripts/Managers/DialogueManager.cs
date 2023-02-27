using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour {
    [Header("UI Components")]
    [SerializeField]
    private TMP_Text nameText;
    [SerializeField]
    private TMP_Text dialogueText;
    [SerializeField]
    private Button continueButton;

    [Header("Animation Variables")]
    [SerializeField]
    private Animator animator;
    private Queue<string> sentences;
    private bool clientFontVisible;

    [Header("Font Variables")]
    private TMP_FontAsset openDyslexicFont;
    private TMP_FontAsset clientFont;

    private void Awake() {
        sentences = new Queue<string>();
        clientFontVisible = true;
        openDyslexicFont = Resources.Load<TMP_FontAsset>("Fonts/OpenDyslexic3-Regular SDF");
        continueButton.onClick.AddListener(DisplayNextSentence);
        CustomEventManager.Current.onTriggerDialogue += StartDialogue;

    }

    private void StartDialogue(Dialogue dialogue) {
        StartCoroutine(StartDialogueRoutine(dialogue));
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
    }

    private void SetFont(TMP_FontAsset font) {
        nameText.font = font;
        dialogueText.font = font;
    }

    public void SetClientFont(TMP_FontAsset font) {
        this.clientFont = font;
        this.SetFont(font);
    }

    public void SwitchFonts() {
        // Switch to Open Dyslexic if showing client specific font
        if (clientFontVisible) {
            this.SetFont(openDyslexicFont);
            this.clientFontVisible = false;
        } else {
            this.SetFont(clientFont);
            this.clientFontVisible = true;
        }
    }
}
