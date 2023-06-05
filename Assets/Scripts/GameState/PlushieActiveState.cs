using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* User-defined Namespaces */
using UserInterface;

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
        private PlayAreaCanvasManager playAreaCanvas;

        public PlushieActiveState(GameManager gameManager) {
            this.gameManager = gameManager;
        }

        public void EnterState() {
            playAreaCanvas = GameObject.FindGameObjectWithTag("PlayAreaCanvas").GetComponent<PlayAreaCanvasManager>();
        }

        public void UpdateState() {
            // Not In Use
        }
    }
}

