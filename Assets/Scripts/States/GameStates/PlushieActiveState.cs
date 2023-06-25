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
    public class PlushieActiveState : GameState {
        /* Private Member Variables */
        private readonly GameStateMachine gameManager;

        public PlushieActiveState(GameStateMachine gameManager) {
            this.gameManager = gameManager;
        }

        public override void EnterState() {
            // Not In Use
        }

        public override void UpdateState() {
            // Not In Use
        }

        public override void ExitState() {
            // Not In Use
        }

    }
}

