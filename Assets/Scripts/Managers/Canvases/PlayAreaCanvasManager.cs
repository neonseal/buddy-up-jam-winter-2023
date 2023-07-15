using System;
using UnityEngine;
using UnityEngine.UI;
/* User-defined Namespaces */
using GameState;

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
        [SerializeField]
        private Button nextClientBtn;
        [SerializeField]
        private Checklist checklist;
        [SerializeField]
        private Tool[] tools;
        private Button clickableNotepad;

        [Header("Mending Tool Elements")]
        private AudioSource bellSound;
        private Tool currentTool;
        private ToolType currentToolType;

        /* UI Interaction Event Actions */
        public static event Action OnNextClientBellRung;

        private void Awake() {
            InitializeCanvasManager();
        }

        /*                       PLAY AREA SETUP                       */
        /* ----------------------------------------------------------- */
        public void InitializeCanvasManager() {
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
            } else {
                // If currently selected tool matched clicked tool, drop tool
                SetCurrentTool(null, ToolType.None);

                // Reset cursor
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
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
        }


        /*                   PRIVATE MEMBER FUNCTIONS                  */
        /* ----------------------------------------------------------- */
        // When the next client button is clicked, we play the bell ring sound effect and 
        // send out an event that will only send in the next client if we are in the 
        // appropriate, workspace empty, game state
        private void HandleNextClientBtnClick() {
            bellSound.Play();
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
