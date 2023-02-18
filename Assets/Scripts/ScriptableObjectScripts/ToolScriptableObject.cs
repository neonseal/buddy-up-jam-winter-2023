using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tool;

[CreateAssetMenu (fileName = "Tool", menuName = "Scriptable Objects/Tool")]
public class ToolScriptableObject : ScriptableObject
{
    public Sprite defaultSprite;
    public Sprite cursorSprite;
    public ToolType toolType;
}
