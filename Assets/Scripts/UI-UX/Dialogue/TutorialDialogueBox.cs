using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Dialogue {
    [System.Serializable]
    public class TutorialDialogueBox : MonoBehaviour {
        [Header("Dialogue Box UI Elements")]
        [SerializeField] private TMP_Text dialogueText;
        [SerializeField] private Button continueButton;
        [SerializeField] private Slider fontSizeToggle;

        [Header("Font Variables")]
        private int defaultFontSize;
        private int largeFontSize;

        private void Awake() {
            SetupTutorialDialogueBox();
        }

        private void SetupTutorialDialogueBox() {
            defaultFontSize = 22;
            largeFontSize = 50;
        }
        
        public void SetTutorialStepTexts(string stepText) {
            dialogueText.text = stepText;
        }
    }

}
