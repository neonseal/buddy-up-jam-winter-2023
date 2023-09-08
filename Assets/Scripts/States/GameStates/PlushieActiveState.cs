
using MendingGames;
using PlayArea;
using System;
using System.Linq;

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
        public PlushieDamageGO CurrentPlushieDamage { get; private set; }

        public static event Action<PlushieDamageGO> MendingGameInitiated;
        public static event Action<Plushie> OnPlushieCompleteEvent;
        public PlushieActiveState(GameStateMachine gameManager) {
            this.gameManager = gameManager;
        }

        public override void EnterState() {
            PlushieDamageGO.OnPlushieDamageClicked += HandleDamageClick;
            Workspace.OnClientPlushieloaded += HandlePlushieLoadEvent;
            MendingGameManager.OnMendingGameComplete += HandleMendingGameCompleteEvent;
        }

        public override void UpdateState() {
            // Not In Use
        }

        public override void ExitState() {
            PlushieDamageGO.OnPlushieDamageClicked -= HandleDamageClick;
            Workspace.OnClientPlushieloaded -= HandlePlushieLoadEvent;
            PlushieActiveState.CurrentPlushie = null;
        }

        private void HandleDamageClick(PlushieDamageGO plushieDamage) {
            CurrentPlushieDamage = plushieDamage;

            // Send command to start mending repair mini-game
            MendingGameInitiated?.Invoke(plushieDamage);
        }

        private void HandlePlushieLoadEvent(Plushie plushie) {
            PlushieActiveState.CurrentPlushie = plushie;
        }

        private void HandleMendingGameCompleteEvent(PlushieDamageGO plushieDamage) {
            PlushieDamageGO[] damageList = CurrentPlushie.PlushieDamageList;
            PlushieDamageGO damage = damageList.Where(d => d == plushieDamage) as PlushieDamageGO;
            damage.DamageRepairComplete = true;

            // Check count of plushie damage elements
            PlushieDamageGO[] plushieDamages = CurrentPlushie.PlushieDamageList;


            // If only one, throw plushie complete event
            if (plushieDamages.Length == 1) {
                PlushieActiveState.OnPlushieCompleteEvent?.Invoke(CurrentPlushie);
            } else {
                // Else, check all of the plushie's damage elements for completeness
                foreach (PlushieDamageGO p in plushieDamages) {
                    if (!p.DamageRepairComplete) {
                        return;
                    }
                }
                // If DamageRepairComplete is true for all plushie damage elements, invoke event
                PlushieActiveState.OnPlushieCompleteEvent?.Invoke(CurrentPlushie);
            }
        }
    }
}

