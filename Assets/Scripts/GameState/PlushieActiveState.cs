using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Plushie Active State
/// 
/// Primary gameplay state when a client/plushie has been loaded into the scene
/// Controls transition to mending mini-game and interaction with plushie elements
/// </summary>
namespace GameState {
    public class PlushieActiveState : IGameState {
        private GameManager gameManager;
        public PlushieActiveState(GameManager gameManager) {
            this.gameManager = gameManager;
        }

        public void EnterState() {
            Debug.Log("PLUSHIE ACTIVE STATE");
        }

        public void UpdateState() {
            // Not In Use
        }
    }
}

