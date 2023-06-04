using System;
using UnityEngine;
using UnityEngine.UI;
/* User-defined Namespaces */
using PlayArea;

/// <summary>
/// Play Area UI Canvas Manager
/// 
/// Controls UI elements relevant to the play area/workspace, including
/// - Next client bell
/// - Checklist
/// - Tool Roll
/// </summary>
namespace UserInterface {
    public class PlayAreaCanvasManager : MonoBehaviour {
        /* Primary UI Elements */
        [SerializeField]
        private Button nextClientBtn;
        [SerializeField]
        private Checklist checklist;
        [SerializeField]
        private Tool[] tools;

        /* Private Member Variables */
        private AudioSource bellSound;

        /* Public Member Variables */
        public static readonly Tool currentTool;
        public static readonly ToolType currentToolType;

        /* UI Interaction Event Actions */
        public static event Action OnNextClientBellRung;

        private void Awake() {
            bellSound = this.gameObject.GetComponent<AudioSource>();
            nextClientBtn.onClick.AddListener(HandleNextClientBtnClick);
        }

        // When the next client button is clicked, we play the bell ring sound effect and 
        // send out an event that will only send in the next client if we are in the 
        // appropriate, workspace empty, game state
        private void HandleNextClientBtnClick() {
            bellSound.Play();
            OnNextClientBellRung?.Invoke();
        }
    }
}
