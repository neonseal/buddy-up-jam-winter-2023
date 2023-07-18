using System;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

namespace Dialogue {
    public class ClientDialogueManager : MonoBehaviour
    {
        [Header("UI Components")]
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text dialogueText;
        [SerializeField] private Button continueButton;
        [SerializeField] private Slider fontToggle;
        [SerializeField] private Slider fontSizeToggle;

        [Header("Animation Variables")]
        [SerializeField] private Animator animator;
        [SerializeField] private float defaultAnimationDelay;
        [SerializeField] private float animationDelay;
        private bool animationPlaying;
        private Queue<string> sentences;
        private bool clientFontVisible;

        [Header("Font Variables")]
        [SerializeField] private TMP_FontAsset openDyslexicFont;
        [SerializeField] private int openDyslexicTitleSize;
        [SerializeField] private int openDyslexicFontSize;
        private TMP_FontAsset clientFont;
        private int clientTitleSize;
        private int clientFontSize;
        private int largeFontSize;

        private PlushieScriptableObject currentPlushie;

        private void Awake()
        {
            sentences = new Queue<string>();

        }
    }
}
