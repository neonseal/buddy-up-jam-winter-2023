using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Dialogue Canvas Manager
/// 
/// High-level manager for the client dialogue and tutorial canvas elements, 
/// responsible for activating/deactivating UI elements and reacting to 
/// state events when they are thrown
/// </summary>
namespace Dialogue {
    public class DialogueCanvasManager : MonoBehaviour {
        [Header("Testing Elements")]
        [SerializeField] private Button tempStartButton;
        [SerializeField] private ClientDialogueSet clientDialogue;
        [SerializeField] private TutorialSequenceScriptableObject tutorialSequence;

        /* Private Member Variables */
        [Header("Client Dialogue Elements")]
        [SerializeField] private ClientDialogueBox clientDialogueBox;

        /* Public Event Actions */
        public static event Action<ClientDialogueSet> OnClientDialogueStart;
        public static event Action<TutorialSequenceScriptableObject> OnTutorialSequenceStart;
        private void Awake() {
            SetupDialogueCanvasManager();
        }

        public void SetupDialogueCanvasManager() {
            tempStartButton.onClick.AddListener( () => { OnTutorialSequenceStart?.Invoke(tutorialSequence); });
        }
    }
}

