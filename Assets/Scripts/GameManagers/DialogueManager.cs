using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour {
    [Header("UI Components")]
    [SerializeField]
    private TMP_Text nameText;
    [SerializeField]
    private TMP_Text dialogueText;
    [SerializeField]
    private Button continueButton;
    [SerializeField]
    private Slider fontToggle;
    [SerializeField]
    private TMP_Text clientFontToggleText;

    [Header("Animation Variables")]
    [SerializeField]
    private float defaultAnimationDelay;
    private Animator animator;
    private float animationDelay;
    private bool animationPlaying;
    private Queue<string> sentences;
    private bool clientFontVisible;

    [Header("Font Variables")]
    private TMP_FontAsset openDyslexicFont;
    private TMP_FontAsset clientFont;
    [SerializeField]
    private int clientTitleSize;
    public int ClientTitleSize {
        set { clientTitleSize = value; }
    }
    [SerializeField]
    private int clientFontSize;
    public int ClientFontSize {
        set { clientFontSize = value; }
    }
    [SerializeField]
    private int openDyslexicTitleSize;
    [SerializeField]
    private int openDyslexicFontSize;

    // Setup singleton pattern
    private static DialogueManager current;
    public static DialogueManager Current {
        get {
            if (current == null) {
                current = FindAnyObjectByType(typeof(DialogueManager)) as DialogueManager;
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
        fontToggle.onValueChanged.AddListener(delegate { SwitchFonts(); });
    }

    private void Update() {
        RaycastHit2D hit = Physics2D.Raycast(Input.mousePosition, Vector2.zero);

        if (Input.GetMouseButtonDown(0) && animationPlaying && (hit.collider != null && hit.collider.name == "ClientDialogueBox")) {
            animationDelay = 0f;
        }
    }

    public void StartDialogue(Dialogue dialogue, Animator inputAnimator) {
        this.animator = inputAnimator;
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

        if (this.animator.name == "ClientDialogueBox") {
            this.animator.SetBool("isOpen", true);
            // Pause briefly to allow animation to finish before rendering text
            yield return new WaitForSeconds(.5f);
        }


        sentences.Clear();

        foreach (string sentence in dialogue.sentences) {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    IEnumerator TypeSentence(string sentence) {
        animationDelay = defaultAnimationDelay;
        animationPlaying = true;

        dialogueText.text = "";

        foreach (char letter in sentence.ToCharArray()) {
            dialogueText.text += letter;
            yield return new WaitForSeconds(animationDelay);
        }

        animationPlaying = false;
    }

    private void EndDialogue() {
        this.animator.SetBool("isOpen", false);
    }

    private void SetFont(TMP_FontAsset font, bool clientFont) {
        nameText.font = font;
        dialogueText.font = font;

        nameText.fontSize = clientFont ? clientTitleSize : openDyslexicTitleSize;
        dialogueText.fontSize = clientFont ? clientFontSize : openDyslexicFontSize;

        this.clientFontVisible = clientFont;
    }

    public void SetClientFont(TMP_FontAsset font, int dialogueFontSize, int dialogueTitleSize) {
        this.clientFont = font;
        this.clientFontSize = dialogueFontSize;
        this.clientTitleSize = dialogueTitleSize;
        this.SetFont(font, true);
    }

    public void SwitchFonts() {
        // Switch to Open Dyslexic if showing client specific font
        TMP_FontAsset nextFont = clientFontVisible ? openDyslexicFont : clientFont;
        this.SetFont(nextFont, !clientFontVisible);
    }

}
