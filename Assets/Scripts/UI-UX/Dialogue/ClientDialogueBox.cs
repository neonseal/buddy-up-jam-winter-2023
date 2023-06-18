using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Client Dialogue Box
/// 
/// This class is responsible for managing the client specific dialogue box, 
/// including actions related to 
/// - Rendering lines of dialogue
/// - Setting up and switching between font/size styling
/// - Maintaining progress status through dialogue sequence
/// </summary>
namespace Dialogue {
    public class ClientDialogueBox : MonoBehaviour {
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
        private Slider fontSizeToggle;

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
        private int largeFontSize;

        private void Awake() {
            InitializeClientDialogueBox();
        }

        private void Update() {
            RaycastHit2D hit = Physics2D.Raycast(Input.mousePosition, Vector2.zero);

            if (Input.GetMouseButtonDown(0) && animationPlaying && (hit.collider != null && hit.collider.name == "ClientDialogueBox")) {
                animationDelay = 0f;
            }
        }

        public void InitializeClientDialogueBox() {
            // Setup sentence animation variables
            sentences = new Queue<string>();
            defaultAnimationDelay = 0.025f;
            animationDelay = defaultAnimationDelay;
            animationPlaying = false;

            // Setup font styling and switching variables
            clientFontVisible = true;
            openDyslexicFont = Resources.Load<TMP_FontAsset>("Fonts/OpenDyslexic3-Regular SDF");
            openDyslexicTitleSize = 26;
            openDyslexicFontSize = 22;
            largeFontSize = 50;

            // Setup event listeners
            DialogueCanvasManager.OnDialogueStart += StartDialogue;
            continueButton.onClick.AddListener(DisplayNextSentence);
            fontToggle.onValueChanged.AddListener(delegate { SwitchFonts(); });
            fontSizeToggle.onValueChanged.AddListener(delegate { SwitchFontSize(); });
        }

        /*public void SetClientStyling(PlushieScriptableObject clientPlushieObject) {
            this.clientFontVisible = true;
            this.clientFont = clientPlushieObject.clientFont;
            this.clientFontSize = clientPlushieObject.dialogueFontSize;
            this.clientTitleSize = clientPlushieObject.nameFontSize;
            this.SetFont(this.clientFont);
        }*/

        private void StartDialogue(ClientDialogueSet dialogue) {
            StartCoroutine(StartDialogueRoutine(dialogue));
        }

        IEnumerator StartDialogueRoutine(ClientDialogueSet dialogue) {
            // Initialize Dialogue Box for current speaker
            nameText.text = dialogue.Name;
            dialogueText.text = "";

            animator.SetBool("isOpen", true);
            // Pause briefly to allow animation to finish before rendering text
            yield return new WaitForSeconds(.5f);

            sentences.Clear();

            foreach (string sentence in dialogue.Sentences) {
                sentences.Enqueue(sentence);
            }

            DisplayNextSentence();
        }

        private void DisplayNextSentence() {
            if (sentences.Count <= 0) {
                EndDialogue();
                return;
            }
            animationDelay = defaultAnimationDelay;

            string sentence = sentences.Dequeue();
            StopAllCoroutines();
            StartCoroutine(TypeSentence(sentence));

            if (sentences.Count == 0) {
                continueButton.gameObject.GetComponentInChildren<TMP_Text>().text = "END";
            }
        }
        IEnumerator TypeSentence(string sentence) {
            dialogueText.text = "";
            animationPlaying = true;

            foreach (char letter in sentence) {
                dialogueText.text += letter;
                yield return new WaitForSeconds(animationDelay);
            }

            animationPlaying = false;
        }

        private void EndDialogue() {
            animator.SetBool("isOpen", false);            
        }

        private void SetFont(bool clientFontVisibility) {
            // Update client font visible boolean 
            this.clientFontVisible = clientFontVisibility;

            nameText.font = this.clientFontVisible ? this.clientFont : this.openDyslexicFont;
            nameText.fontSize = this.clientFontVisible ? this.clientTitleSize : this.openDyslexicTitleSize;

            dialogueText.font = this.clientFontVisible ? this.clientFont : this.openDyslexicFont;
            dialogueText.fontSize = this.clientFontVisible ? this.clientFontSize : this.openDyslexicFontSize;
        }

        private void SwitchFonts() {
            // Switch to Open Dyslexic if showing client specific font
            this.SetFont(!clientFontVisible);
        }

        private void SwitchFontSize() {
            bool largeFontSelected = this.fontSizeToggle.value > 0;
            int smallFontSize = this.clientFontVisible ? this.clientFontSize : this.openDyslexicFontSize;

            dialogueText.fontSize = largeFontSelected ? this.largeFontSize : smallFontSize;
        }
    }
}

