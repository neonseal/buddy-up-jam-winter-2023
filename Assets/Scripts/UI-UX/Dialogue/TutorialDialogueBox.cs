using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dialogue {
    [System.Serializable]
    public class TutorialDialogueBox : MonoBehaviour {
        [Header("Dialogue Box UI Elements")]
        [SerializeField] private TMP_Text dialogueText;
        [SerializeField] private Button continueButton;
        [SerializeField] private Slider fontSizeToggle;
        [SerializeField] private ScrollRect scrollRect;

        [Header("Font Variables")]
        [SerializeField] private int defaultFontSize;
        [SerializeField] private int largeFontSize;

        /* Tutorial Box Public Events */
        public static event Action OnContinueButtonPressed;

        private void Awake() {
            SetupTutorialDialogueBox();
        }

        private void SetupTutorialDialogueBox() {
            // Setup event actions
            continueButton.onClick.AddListener(() => { OnContinueButtonPressed?.Invoke(); });
            fontSizeToggle.onValueChanged.AddListener(delegate { SwitchFontSize(); });
        }

        public void SetTutorialStepTexts(string stepText) {
            if (scrollRect.verticalNormalizedPosition != 1f) {
                scrollRect.verticalNormalizedPosition = 1f;
            }

            dialogueText.text = stepText;
        }

        public void EnableDisableContinueButton(bool enabled) {
            continueButton.gameObject.SetActive(enabled);
        }

        private void SwitchFontSize() {
            bool largeFontSelected = this.fontSizeToggle.value > 0;
            dialogueText.fontSize = largeFontSelected ? this.largeFontSize : defaultFontSize;
        }
    }

}
