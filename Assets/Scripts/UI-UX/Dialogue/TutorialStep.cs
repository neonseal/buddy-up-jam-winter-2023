
using PlayArea;
using UnityEngine;

namespace Dialogue {
    // Some tutorial steps require a specifc action to progress the sequence
    public enum TutorialActionRequiredContinueType {
        None,
        RingBell,
        SelectDamage,
        SelectTool,
        DropTool,
        StartRepair,
        CompleteRepair,
        CompleteJob,
        SelectThankYou
    }

    [System.Serializable]
    public class TutorialStep {
        [SerializeField, TextArea(3, 10)] public string stepText;
        [SerializeField] public ToolType requiredToolType;
        [SerializeField] public TutorialActionRequiredContinueType requiredContinueAction;
        [SerializeField, Range(0, 10)] public int timer;

        [SerializeField] public Vector2 tutorialDialogueLocation;
        [SerializeField] public bool requiresArrow;
        [SerializeField] public Vector2 tutorialArrowLocation;
        [SerializeField] public int arrowZRotationValue;
    }
}

