using System;
using System.Collections.Generic;
using UnityEngine;
/* User-defined Namespaces */
using PlayArea;

/// <summary>
/// Workspace Empty Game State
/// 
/// Initial play game state. Represents the workspace before a plushie has been loaded, and 
/// after a repair has been completed and the plushie has been returned to the client.
/// Handles requests to load the next customer and switch to the main play state.
/// </summary>
namespace GameState {
    public class WorkspaceEmptyState : GameState {
        /* Private Member Variables */
        private readonly GameStateMachine gameManager;

        /* Public Event Actions */
        public static event Action OnNextClientRequested;

        public WorkspaceEmptyState(GameStateMachine gameManager) {
            this.gameManager = gameManager;
        }

        public override void EnterState() {
            PlayAreaCanvasManager.OnNextClientBellRung += CallNextClient;
            Workspace.OnClientloaded += SwitchToPlushieActiveState;
            // TODO: Check if we need to play to welcomeSequence Tutorial 
        }

        public override void UpdateState() {
            // Not In Use
        }

        public override void ExitState() {
            PlayAreaCanvasManager.OnNextClientBellRung -= CallNextClient;
            Workspace.OnClientloaded -= SwitchToPlushieActiveState;
        }

        private void CallNextClient() {
            OnNextClientRequested?.Invoke();
        }


        private void SwitchToPlushieActiveState() {
            gameManager.SwitchGameState(gameManager.PlushieActiveState);
        }
    }
}

