using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* User-defined Namespaces */
using PlayArea;

/// <summary>
/// Plushie Active State
/// 
/// Primary gameplay state when a client/plushie has been loaded into the scene
/// Controls transition to mending mini-game and interaction with plushie elements
/// </summary>
namespace GameState {
    public class PlushieActiveState : IGameState {
        /* Private Member Variables */
        private readonly GameManager gameManager;

        public PlushieActiveState(GameManager gameManager) {
            this.gameManager = gameManager;
        }

        public void EnterState() {
        }

        public void UpdateState() {
            // Not In Use
        }

        public void ExitState() {
            // Not In Use
        }
    }
}

