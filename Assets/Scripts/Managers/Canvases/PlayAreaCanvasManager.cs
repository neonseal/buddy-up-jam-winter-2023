using DG.Tweening;
using Dialogue;
using GameState;
using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Play Area UI Canvas Manager
/// 
/// Controls UI elements relevant to the play area/workspace, including
/// - Next client bell
/// - Checklist
/// - Tool Roll
/// </summary>
namespace PlayArea {
    public class PlayAreaCanvasManager : MonoBehaviour {
        /* Private Member Variables */
        [Header("Checklist Elements")]
        [SerializeField] private Button nextClientBtn;
        [SerializeField] private Checklist checklist;
        [SerializeField] private Tool[] tools;
        private Button clickableNotepad;

        [Header("Bell Animation Elements")]
        [SerializeField] private Image bellPlunger;
        [SerializeField] private Image bellDome;
        [SerializeField] private float plungerEndYValue;
        [SerializeField] private float plungerAnimationDuration;

        [Header("Dome Animation Elements")]
        [SerializeField] float duration;
        [SerializeField] Vector3 strength;
        [SerializeField] int vibrato;
        [SerializeField] float randomness;
        [SerializeField] bool fadeOut;

        [Header("Mending Tool Elements")]
        private AudioSource bellSound;
        private Tool currentTool;
        private ToolType currentToolType;

        [Header("Tutorial/Dialogue Managers")]
        [SerializeField] private TutorialManager tutorialManager;

        /* UI Interaction Event Actions */
        public static event Action OnNextClientBellRung;

        private void Awake() {
            InitializeCanvasManager();
        }

        /*                       PLAY AREA SETUP                       */
        /* ----------------------------------------------------------- */
        public void InitializeCanvasManager() {
            DOTween.Init();

            bellSound = this.gameObject.GetComponent<AudioSource>();

            nextClientBtn.onClick.AddListener(HandleNextClientBtnClick);

            // Set up event listeners
            WorkspaceEmptyState.OnNextClientRequested += EnablePlayArea;
            Tool.OnToolClicked += HandleToolSelection;

            SetToolRollColliderStatus(false);
        }

        public void EnablePlayArea() {
            SetToolRollColliderStatus(true);
            checklist.EnableNotepad();
        }

        public void DisablePlayArea() {
            SetToolRollColliderStatus(false);
            checklist.DisableNotepad();
            WorkspaceEmptyState.OnNextClientRequested -= EnablePlayArea;
            Tool.OnToolClicked -= HandleToolSelection;
        }


        /*                       TOOL SELECTION                        */
        /* ----------------------------------------------------------- */
        private void HandleToolSelection(Tool tool, ToolType toolType) {
            // If no tool selected or different tool, pick up clicked tool
            if (currentTool == null || currentTool != tool) {
                SetCurrentTool(tool, toolType);

                // Set held tool cursor
                SetToolCursor();

                // Check if there is a tutorial active that requires a continue action, and continue tutorial
                if (tutorialManager.GetRequiredContinueAction() == TutorialActionRequiredContinueType.SelectTool) {
                    tutorialManager.ContinueTutorialSequence();
                }
            } else {
                currentTool.Place();

                // If currently selected tool matched clicked tool, drop tool
                SetCurrentTool(null, ToolType.None);

                // Reset cursor
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);


                // Check if there is a tutorial active that requires a continue action, and continue tutorial
                if (tutorialManager.GetRequiredContinueAction() == TutorialActionRequiredContinueType.DropTool) {
                    tutorialManager.ContinueTutorialSequence();
                }
            }
        }

        private void SetCurrentTool(Tool tool, ToolType toolType) {
            currentTool = tool;
            currentToolType = toolType;
        }

        private void SetToolCursor() {
            if (
                currentTool.ToolScriptableObject.ToolType.Equals(ToolType.Cleaning) ||
                currentTool.ToolScriptableObject.ToolType.Equals(ToolType.Stuffing)
            ) {
                // Set cursor at center of sprite
                Cursor.SetCursor(
                    currentTool.ToolScriptableObject.ToolCursorTexture,
                    new Vector2(
                        currentTool.ToolScriptableObject.ToolCursorTexture.width,
                        currentTool.ToolScriptableObject.ToolCursorTexture.height
                    ) / 2f,
                    CursorMode.ForceSoftware
                );
            } else {
                // Set cursor at crosshair
                Cursor.SetCursor(
                    currentTool.ToolScriptableObject.ToolCursorTexture,
                    new Vector2(
                        40f,
                        29f
                    ),
                    CursorMode.ForceSoftware
                );
            }
            currentTool.Pickup();
        }


        /*                   PRIVATE MEMBER FUNCTIONS                  */
        /* ----------------------------------------------------------- */
        // When the next client button is clicked, we play the bell ring sound effect and 
        // send out an event that will only send in the next client if we are in the 
        // appropriate, workspace empty, game state
        private void HandleNextClientBtnClick() {
            bellSound.Play();
            Sequence sequence = DOTween.Sequence();
            sequence.Append(bellPlunger.transform.DOLocalMoveY(plungerEndYValue, plungerAnimationDuration, false));
            sequence.SetLoops(2, LoopType.Yoyo);
            sequence.Play();
            bellDome.transform.DOShakeRotation(duration, strength, vibrato, randomness, fadeOut, ShakeRandomnessMode.Harmonic);

            if (tutorialManager.TutorialActive && tutorialManager.GetRequiredContinueAction() == TutorialActionRequiredContinueType.RingBell) {
                tutorialManager.ContinueTutorialSequence();
            }

            OnNextClientBellRung?.Invoke();
        }

        private void SetToolRollColliderStatus(bool status) {
            foreach (Tool tool in tools) {
                tool.GetComponent<BoxCollider2D>().enabled = status;
            }
        }

        /* Public Properties */
        public Tool CurrentTool { get => currentTool; }
        public ToolType CurrentToolType { get => currentToolType; }
    }
}
