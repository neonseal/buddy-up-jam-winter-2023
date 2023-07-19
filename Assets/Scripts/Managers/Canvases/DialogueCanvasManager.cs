using PlayArea;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using UserInterface;

/// <summary>
/// Dialogue Canvas Manager
/// 
/// High-level manager for the client dialogue and tutorial canvas elements, 
/// responsible for activating/deactivating UI elements and reacting to 
/// state events when they are thrown
/// </summary>
namespace Dialogue {
    public class DialogueCanvasManager : MonoBehaviour {
        private Plushie currentPlushie;
        [SerializeField] private ClientDialogueSet clientDialogue;
        [SerializeField] private TutorialSequenceScriptableObject welcomeTutorialSequence;

        /* Private Member Variables */
        [Header("Client Dialogue Elements")]
        [SerializeField] private ClientDialogueManager clientDialogueManager;

        /* Public Event Actions */
        public static event Action<ClientDialogueSet> OnClientDialogueStart;
        public static event Action<TutorialSequenceScriptableObject> OnTutorialSequenceStart;
        private void Awake() {
            SetupDialogueCanvasManager();
        }

        public void SetupDialogueCanvasManager() {
            MainMenuCanvasManager.OnStartButtonPressed += StartWelcomeTutorial;
            Workspace.OnClientPlushieloaded += HandlePlushieLoadEvent;
            ClientDialogueManager.OnClientDialogueComplete += CheckForPlushieTutorial;
        }

        private void StartWelcomeTutorial() {
            OnTutorialSequenceStart?.Invoke(welcomeTutorialSequence);
        }

        private void HandlePlushieLoadEvent(Plushie currentPlushie)
        {
            this.currentPlushie = currentPlushie;
            Assert.IsNotNull(currentPlushie.IssueDialogue, "Client issue dialogue is undefined!");
            OnClientDialogueStart?.Invoke(currentPlushie.IssueDialogue);
        }

        private void CheckForPlushieTutorial()
        {
            if (currentPlushie.HasTutorialDialogue)
            {
                Assert.IsNotNull(currentPlushie.TutorialSequenceScriptableObject, "Client plushie states it has a tutoril, but scriptable is null!");
                OnTutorialSequenceStart?.Invoke(currentPlushie.TutorialSequenceScriptableObject);
            }
        }
    }
}

