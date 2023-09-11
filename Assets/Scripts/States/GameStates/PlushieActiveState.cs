using PlayArea;
using System;
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
            ClientCard.OnClientCardInitialClick += FinishPlushieState;
        }

        public override void UpdateState() {
            // Not In Use
        }

        public override void ExitState() {
            Workspace.OnClientPlushieloaded -= HandlePlushieLoadEvent;
            PlushieDamageGO.OnPlushieDamageClicked -= HandleDamageClick;
            PlushieActiveState.CurrentPlushie = null;
        }

        public void FinishPlushieState(ClientCard clientCard) {
            this.gameManager.SwitchGameState(this.gameManager.WorkspaceEmptyState);
        }

        private void HandleDamageClick(PlushieDamageGO plushieDamage) {
            CurrentPlushieDamage = plushieDamage;

            // Send command to start mending repair mini-game
            MendingGameInitiated?.Invoke(plushieDamage);
        }

        private void HandlePlushieLoadEvent(Plushie plushie) {
            PlushieActiveState.CurrentPlushie = plushie;
        }

        public static void CheckPlushieCompletionState() {
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

