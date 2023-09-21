using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Client Dialogue Manager
/// 
/// This class is responsible for managing the client specific dialogue box, 
/// including actions related to 
/// - Rendering lines of dialogue
/// - Setting up and switching between font/size styling
/// - Maintaining progress status through dialogue sequence
/// </summary>
namespace Dialogue {
    public class ClientDialogueManager : MonoBehaviour {
        /* Private Member Variables */
        [Header("UI Components")]
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text dialogueText;
        [SerializeField] private Button continueButton;
        [SerializeField] private Slider fontToggle;
        [SerializeField] private Slider fontSizeToggle;
        [SerializeField] private ScrollRect scrollRect;

        [Header("Animation Variables")]
        [SerializeField] private Animator animator;
        [SerializeField] private float defaultAnimationDelay;
        private string currentSentence;
        private float animationDelay;
        private bool animationPlaying;
        private Queue<string> sentences;
        private bool clientFontVisible;

        [Header("Font Variables")]
        [SerializeField] private TMP_FontAsset openDyslexicFont;
        [SerializeField] private int openDyslexicTitleSize;
        [SerializeField] private int openDyslexicFontSize;
        [SerializeField] private int largeFontSize;

        private TMP_FontAsset clientFont;
        private int clientTitleSize;
        private int clientFontSize;

        /* Public Event Actions */
        public static event Action OnClientDialogueComplete;

        private void Awake() {
            SetupClientDialogueBox();
        }

        private void Update() {
            RaycastHit2D hit = Physics2D.Raycast(Input.mousePosition, Vector2.zero);

            if (Input.GetMouseButtonDown(0) && animationPlaying && hit.collider != null && hit.collider.name == "ClientDialogueManager") {
                SkipSentenceAnimation();
            }
        }

        public void SetupClientDialogueBox() {
            // Setup sentence animation variables
            sentences = new Queue<string>();
            defaultAnimationDelay = 0.025f;
            animationDelay = defaultAnimationDelay;
            animationPlaying = false;

            // Setup font styling and switching variables
            clientFontVisible = true;

            // Setup event listeners
            continueButton.onClick.AddListener(DisplayNextSentence);
            fontToggle.onValueChanged.AddListener(delegate { SwitchFonts(); });
            fontSizeToggle.onValueChanged.AddListener(delegate { SwitchFontSize(); });
        }

        public void SetClientStyling(Plushie plushie) {
            this.clientFontVisible = true;
            this.clientFont = plushie.ClientFont;
            this.clientFontSize = plushie.DialogueFontSize;
            this.clientTitleSize = plushie.NameFontSize;
            this.SetFont(this.clientFont);
        }

        public void StartDialogueSequence(ClientDialogueSet dialogue) {
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

            if (scrollRect.verticalNormalizedPosition != 1f) {
                scrollRect.verticalNormalizedPosition = 1f;
            }

            this.currentSentence = sentences.Dequeue();

            StopAllCoroutines();
            StartCoroutine(TypeSentence(this.currentSentence));

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

        private void SkipSentenceAnimation() {
            StopAllCoroutines();
            dialogueText.text = this.currentSentence;
        }

        private void EndDialogue() {
            animator.SetBool("isOpen", false);
            OnClientDialogueComplete?.Invoke();
            continueButton.gameObject.GetComponentInChildren<TMP_Text>().text = ">>";
        }

        private void SetFont(bool clientFontVisibility) {
            // Update client font visible boolean 
            this.clientFontVisible = clientFontVisibility;

            nameText.font = this.clientFontVisible ? this.clientFont : this.openDyslexicFont;
            nameText.fontSize = this.clientFontVisible ? this.clientTitleSize : this.openDyslexicTitleSize;

            dialogueText.font = this.clientFontVisible ? this.clientFont : this.openDyslexicFont;

            if (fontSizeToggle.value > 0) {
                dialogueText.fontSize = largeFontSize;
            } else {
                dialogueText.fontSize = this.clientFontVisible ? this.clientFontSize : this.openDyslexicFontSize;

            }
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

