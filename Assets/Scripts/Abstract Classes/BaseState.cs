public abstract class BaseState {
    public string name;
    private protected BaseStateMachine stateMachine;

    public BaseState(string name, BaseStateMachine stateMachine) {
        this.name = name;
        this.stateMachine = stateMachine;
    }

    public virtual void EnterState() { }

    public virtual void UpdateFrame() { }

    public virtual void UpdatePhysics() { }

    public virtual void ExitState() { }
}
