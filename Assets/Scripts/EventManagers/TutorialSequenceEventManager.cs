using System.Collections;
using UnityEngine;
using System;
using GameData;

public class TutorialSequenceEventManager {
    //Singleton setup
    private static readonly TutorialSequenceEventManager instance = new TutorialSequenceEventManager();
    static TutorialSequenceEventManager() {
    }
    private TutorialSequenceEventManager() {
    }
    public static TutorialSequenceEventManager Current {
        get {
            return TutorialSequenceEventManager.instance;
        }
    }

    public IEnumerator HandleTutorialRequiredActionCompletion(float waitTime = .25f) {
        yield return new WaitForSeconds(waitTime);
        ContinueTutorialSequence();
    }

    // Start New Tutorial Sequence
    public event Action<TutorialSequenceScriptableObject> onStartTutorialSequence;
    public void StartTutorialSequence(TutorialSequenceScriptableObject TutorialSequenceScriptableObject) {
        if (this.onStartTutorialSequence != null) {
            this.onStartTutorialSequence(TutorialSequenceScriptableObject);
        }
    }

    public event Action onContinueTutorialSequence;
    public void ContinueTutorialSequence() {
        if (this.onContinueTutorialSequence != null) {
            this.onContinueTutorialSequence();
        }
    }

    // Set up Required Continue Actions
    public event Action onRequireDamageSelectContinueAction;
    public void RequireDamageSelectContinueAction() {
        if (this.onRequireDamageSelectContinueAction != null) {
            this.onRequireDamageSelectContinueAction();
        }
    }

    public event Action<ToolType> onRequireToolPickupContinueAction;
    public void RequireToolPickupContinueAction(ToolType toolType) {
        if (this.onRequireToolPickupContinueAction != null) {
            this.onRequireToolPickupContinueAction(toolType);
        }
    }

    public event Action<ToolType> onRequireToolDropContinueAction;
    public void RequireToolDropContinueAction(ToolType toolType) {
        if (this.onRequireToolDropContinueAction != null) {
            this.onRequireToolDropContinueAction(toolType);
        }
    }

    public event Action onRequiredRepairCompletionAction;
    public void RequireRepairCompletionAction() {
        if (this.onRequiredRepairCompletionAction != null) {
            this.onRequiredRepairCompletionAction();
        }
    }

    public event Action onRequireJobCompletionAction;
    public void RequireJobCompletionAction() {
        if (this.onRequireJobCompletionAction != null) {
            this.onRequireJobCompletionAction();
        }
    }
}
