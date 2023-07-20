
using UserInterface;

/// <summary>
/// Main Menu State 
/// 
/// Initial starting game state. Handles start button input to kick off the game
/// </summary>
namespace GameState {
    public class MainMenuState : GameState {
        private readonly GameStateMachine gameManager;

        public MainMenuState(GameStateMachine gameManager) {
            this.gameManager = gameManager;
        }

        public override void EnterState() {
            MainMenuCanvasManager.OnStartButtonPressed += StartGame;
        }

        public override void UpdateState() {
            // Not In Use
        }

        public override void ExitState() {
            MainMenuCanvasManager.OnStartButtonPressed -= StartGame;
        }

        private void StartGame() {
            gameManager.SwitchGameState(gameManager.WorkspaceEmptyState);
        }
    }
}

