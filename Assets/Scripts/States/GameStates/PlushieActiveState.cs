using System;
using System.Collections.Generic;
using UnityEngine;
/* User-defined Namespaces */
using PlayArea;
using Scriptables.DamageInstructions;

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
        
        /* Public Properties */
        public static Plushie CurrentPlushie { get; private set; }


        public static event Action<DamageInstructrionsScriptableObject[]> MendingGameInitiated;

        public PlushieActiveState(GameStateMachine gameManager) {
            this.gameManager = gameManager;
        }

        public override void EnterState() {
            PlushieDamageGO.OnPlushieDamageClicked += HandleDamageClick;
            Workspace.OnClientPlushieloaded += HandlePlushieLoadEvent;
        }

        public override void UpdateState() {
            // Not In Use
        }

        public override void ExitState() {
            PlushieDamageGO.OnPlushieDamageClicked -= HandleDamageClick;
            Workspace.OnClientPlushieloaded -= HandlePlushieLoadEvent;
            PlushieActiveState.CurrentPlushie = null;
        }

        private void HandleDamageClick(DamageInstructrionsScriptableObject[] damageInstructions) {
            // Send command to start mending repair mini-game
            MendingGameInitiated?.Invoke(damageInstructions);
        }

        private void HandlePlushieLoadEvent(Plushie plushie)
        {
            PlushieActiveState.CurrentPlushie = plushie;
        }
    }
}

