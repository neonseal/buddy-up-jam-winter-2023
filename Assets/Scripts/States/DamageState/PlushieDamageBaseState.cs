public class PlushieDamageBaseState : BaseState {

    public PlushieDamageBaseState(string name, PlushieDamageSM stateMachine) : base(name, stateMachine) {
    }

    public virtual void CompleteRepair() {
    }
}
