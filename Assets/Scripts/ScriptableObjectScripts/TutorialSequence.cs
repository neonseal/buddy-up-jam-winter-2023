using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "Tutorial Sequence", menuName = "Scriptable Objects/Tutorial Sequence")]
public class TutorialSequence : ScriptableObject
{
    public string sequenceName;

    [TextArea(3, 10)]
    public string[] sentences;

    public Vector3[] dialoguePositions;
    public Vector3[] arrowPositions;
    public Vector3[] arrowRotations;
}
