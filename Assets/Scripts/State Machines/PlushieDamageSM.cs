using MendingGames;
using UnityEngine;

public class PlushieDamageSM : BaseStateMachine {
    // State of plushie damage as a small rip
    [HideInInspector]
    public SmallRipState smallRipState;

    // State of plushie damage as a large rip
    [HideInInspector]
    public LargeRipState largeRipState;

    // State of plushie damage as a worn stuffing
    [HideInInspector]
    public WornStuffingState wornStuffingState;

    // State of plushie damage as a large rip with missing stuffing
    [HideInInspector]
    public MissingStuffingState missingStuffingState;

    // Goal state of plushi damage - when repair on a damage is finished
    [HideInInspector]
    public RepairFinishState repairFinishState;

    // Reference to PlushieDamageGO component
    internal PlushieDamageGO _plushieDamageGO;


    // Initialize fields on load
    public void Awake() {
        InitializeFields();
    }

    // Initialize references before first frame update
    public void Start() {
        InitializeStateMachine();
    }

    // Initialize fields in this class
    private void InitializeFields() {
        InitializeStates();
        _plushieDamageGO = GetComponent<PlushieDamageGO>();
    }

    // Initialize states in this state machine
    private void InitializeStates() {
        smallRipState = new SmallRipState(this);
        largeRipState = new LargeRipState(this);
        wornStuffingState = new WornStuffingState(this);
        missingStuffingState = new MissingStuffingState(this);
        repairFinishState = new RepairFinishState(this);
    }

    // Assign blank grid state as the initial state
    protected override BaseState GetInitialState() {
        switch (_plushieDamageGO.GetInitialDamageType()) {
            case PlushieDamageType.SmallRip: return smallRipState;
            case PlushieDamageType.LargeRip: return largeRipState;
            case PlushieDamageType.WornStuffing: return wornStuffingState;
            case PlushieDamageType.MissingStuffing: return missingStuffingState;
            // This error means the plushie damage game object associated with this state machine has initialPlushieDamageType set to null
            default: throw new System.Exception("Invalid initial plushie dmamage of type " + _plushieDamageGO.GetInitialDamageType().ToString());
        }
    }

    internal void SubscribeToMendingGame() {
        MendingGameManager.OnMendingGameComplete += ProgressMendingSteps;
    }

    internal void UnsubscribeToMendingGame() {
        MendingGameManager.OnMendingGameComplete -= ProgressMendingSteps;
    }

    private void ProgressMendingSteps(PlushieDamageGO plushieDamage) {
        ((PlushieDamageBaseState)currentState).CompleteRepair();
    }
}
