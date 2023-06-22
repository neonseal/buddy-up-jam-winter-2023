using UnityEngine;
/* User-defined Namespaces */
using UserInterface;

/// <summary>
/// Main Menu State 
/// 
/// Initial starting game state. Handles start button input to kick off the game
/// </summary>
namespace GameState {
    public class MainMenuState : IGameState {
        private readonly GameManager gameManager;

        public MainMenuState(GameManager gameManager) {
            this.gameManager = gameManager;
        }

        public void EnterState() {
            MainMenuCanvasManager.OnStartButtonPressed += StartGame;
        }

        public void UpdateState() {
            // Not in use
        }
        public void ExitState() {
            // Not In Use
        }

        private void StartGame() {
            // Unsubscribe from event handlers and switch to play state
            MainMenuCanvasManager.OnStartButtonPressed -= StartGame;
            gameManager.SwitchGameState(gameManager.WorkspaceEmptyState);
        }
    }
}

