using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Game State Manager
/// 
/// Primary game state manager controlling flow of control between all game states 
/// throughout the course of the game
/// </summary>

namespace GameState {
    public class GameManager : MonoBehaviour {
        /* Private Member Variables */
        [Header("Game State Concrete Implementations")]
        private IGameState currentState;
        private MainMenuState mainMenuState;
        private WorkspaceEmptyState workspaceEmptyState;
        private PlushieActiveState plushieActiveState;

        private void Awake() {
            InitializeGameManager();
        }

        private void Update() {
            currentState.UpdateState();
        }

        public void InitializeGameManager() {
            SetupGameStates();

            // Set and enter initial state
            currentState = mainMenuState;
            currentState.EnterState();
        }

        public void SwitchGameState(IGameState newState) {
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
        public IGameState CurrentState { get => currentState; }
        public IGameState MainMenuState { get => mainMenuState; }
        public IGameState WorkspaceEmptyState { get => workspaceEmptyState; }
        public IGameState PlushieActiveState { get => plushieActiveState; }
    }
}

