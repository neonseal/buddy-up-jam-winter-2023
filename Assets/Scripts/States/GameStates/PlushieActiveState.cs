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
        private Plushie currentPlushie;


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
        }

        private void HandleDamageClick(DamageInstructrionsScriptableObject[] damageInstructions) {
            // Send command to start mending repair mini-game
            MendingGameInitiated?.Invoke(damageInstructions);
        }

        private void HandlePlushieLoadEvent(Plushie plushie)
        {
            this.currentPlushie = plushie;
            // TODO: Send event to play plushie client dialogue to be received by ClientDialogueManager
            // TODO: Setup listener for ClientDialogueManager.OnClientDialogueComplete event to play tutorial if needed
            // TODO: Fire start tutorial event if needed
        }
    }
}

