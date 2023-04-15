using UnityEngine;
using GameData;

[CreateAssetMenu (fileName = "Tutorial Sequence", menuName = "Scriptable Objects/Tutorial Sequence")]
public class TutorialSequenceScriptableObject : ScriptableObject
{
    public string sequenceName;

    public bool hasRequiredTool;
    public ToolType requiredToolType;

    [TextArea(3, 10)]
    public string[] stepTextSet;
    public Vector2[] tutorialDialogueLocations;
    public Vector2[] tutorialArrowLocations;
    public int[] arrowZRotationValue;
}
