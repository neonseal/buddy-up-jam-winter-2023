using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "Tutorial Sequence", menuName = "Scriptable Objects/Tutorial Sequence")]
public class TutorialDialogueScriptableObject : ScriptableObject
{
    public string title;
    [TextArea(3, 10)]
    public string[] stepTextSet;
    public Vector3[] stepBoxLocations;
    public (Vector3, Vector3)[] arrowPositionAndRotation;
}
