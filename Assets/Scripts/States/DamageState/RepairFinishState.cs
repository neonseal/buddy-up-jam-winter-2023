public class RepairFinishState : PlushieDamageBaseState {
    public RepairFinishState(PlushieDamageSM stateMachine) : base("Plushie damage repair complete state", stateMachine) {
    }

    public override void EnterState() {
        ((PlushieDamageSM) this.stateMachine).unsubscribeToMendingGame();
     }
}
