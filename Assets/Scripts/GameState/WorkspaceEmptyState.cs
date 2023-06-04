using System;
using System.Collections.Generic;
using UnityEngine;
/* User-defined Namespaces */
using UserInterface;
using PlayArea;

/// <summary>
/// Workspace Empty Game State
/// 
/// Initial play game state. Represents the workspace before a plushie has been loaded, and 
/// after a repair has been completed and the plushie has been returned to the client.
/// Handles requests to load the next customer and switch to the main play state.
/// </summary>
namespace GameState {
    public class WorkspaceEmptyState : IGameState {
        /* Private Member Variables */
        private readonly GameManager gameManager;

        /* Public Event Actions */
        public static event Action OnNextClientRequested;

        public WorkspaceEmptyState(GameManager gameManager) {
            this.gameManager = gameManager;
        }

        public void EnterState() {
            PlayAreaCanvasManager.OnNextClientBellRung += () => { OnNextClientRequested?.Invoke(); };
            Workspace.OnClientloaded += () => { gameManager.SwitchGameState(gameManager.PlushieActiveState); } ;
        }

        public void UpdateState() {
            // Not in use
        }
    }
}

