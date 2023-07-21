public class WornStuffingState : PlushieDamageBaseState {
    public WornStuffingState(PlushieDamageSM stateMachine) : base("Worn stuffing plushie damage state", stateMachine) {
    }

    public override void CompleteRepair() {
        this.stateMachine.ChangeState(((PlushieDamageSM)this.stateMachine).largeRipState);
    }
}
