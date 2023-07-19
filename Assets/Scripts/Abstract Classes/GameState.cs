namespace GameState {
    public abstract class GameState {
        /* EnterState is called at the beginning of a state change to set up the state, 
        * including setting event listeners */
        public abstract void EnterState();

        /* UpdateState is called every frame by the Game Manager and execute runtime state logic */
        public abstract void UpdateState();

        public abstract void ExitState();
    }
}