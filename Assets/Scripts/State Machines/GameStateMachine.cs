using UnityEngine;

/// <summary>
/// Game State Manager
/// 
/// Primary game state manager controlling flow of control between all game states 
/// throughout the course of the game
/// </summary>

namespace GameState {
    public class GameStateMachine : MonoBehaviour {
        /* Private Member Variables */
        [Header("Game State Concrete Implementations")]
        private GameState currentState;
        private MainMenuState mainMenuState;
        private WorkspaceEmptyState workspaceEmptyState;
        private PlushieActiveState plushieActiveState;

        private void Awake() {
            InitializeGameManager();
        }

        private void Start() {
            InitializeStartingState();
        }

        private void Update() {
            currentState.UpdateState();
        }

        public void InitializeGameManager() {
            SetupGameStates();
        }

        public void InitializeStartingState() {
            // Set and enter initial state
            currentState = mainMenuState;
            currentState.EnterState();
        }

        public void SwitchGameState(GameState newState) {
            currentState.ExitState();
            // Set new state
            currentState = newState;
            currentState.EnterState();
        }

        // Instantiate all game states
        private void SetupGameStates() {
            mainMenuState = new MainMenuState(this);
            workspaceEmptyState = new WorkspaceEmptyState(this);
            plushieActiveState = new PlushieActiveState(this);
        }

        /* Public Properties */
        public GameState CurrentState { get => currentState; }
        public GameState MainMenuState { get => mainMenuState; }
        public GameState WorkspaceEmptyState { get => workspaceEmptyState; }
        public GameState PlushieActiveState { get => plushieActiveState; }
    }
}

