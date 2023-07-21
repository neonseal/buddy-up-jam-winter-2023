using UnityEngine;

public abstract class BaseStateMachine : MonoBehaviour {
    // Current state of the state machine
    internal BaseState currentState;

    // Set currentState to the initial state of the implementing state machine (default is null)
    /*
    private protected void Start()
    {
        InitializeState();
    }
    */

    // Call update logic function of state
    private void Update() {
        if (currentState != null) {
            currentState.UpdateFrame();
        }
    }

    // Call update physics function of state
    private void FixedUpdate() {
        if (currentState != null) {
            currentState.UpdatePhysics();
        }
    }

    // Switch state to input state
    // Calls ExitState() of old state and EnterState() of new state
    public void ChangeState(BaseState newState) {
        currentState.ExitState();

        currentState = newState;
        currentState.EnterState();
    }

    private protected void InitializeState() {
        currentState = GetInitialState();
        if (currentState != null) {
            currentState.EnterState();
        }
    }

    // Default implementation of function for getting initial state
    // Default: returns null
    protected virtual BaseState GetInitialState() {
        return null;
    }

    // For debugging - print info of state machine to console
    protected void printStateNameToConsole() {
        Debug.Log("State machine " + this.GetType().Name + " state: " + currentState.name);
    }

    // For debugging - display name of current state on GUI
    protected void DisplayStateNameOnGUI() {
        GUILayout.BeginArea(new Rect(10f, 10f, 200f, 100f));
        string content = currentState != null ? currentState.name : "(No Current State)";
        GUILayout.Label($"<color='black'><size=40>{content}</size></color>");
        GUILayout.EndArea();
    }
}
