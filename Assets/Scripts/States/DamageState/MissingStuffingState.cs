public class MissingStuffingState : PlushieDamageBaseState {
    public MissingStuffingState(PlushieDamageSM stateMachine) : base("Missing stuffing plushie damage state", stateMachine) {
    }

    public override void CompleteRepair() {
        this.stateMachine.ChangeState(((PlushieDamageSM)this.stateMachine).largeRipState);
    }
}
