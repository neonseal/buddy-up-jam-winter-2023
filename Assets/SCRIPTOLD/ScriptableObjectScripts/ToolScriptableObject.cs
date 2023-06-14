#pragma warning disable S1104 // Fields should not have public accessibilityusing System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* User-defined Namespaces */
using GameData;

[CreateAssetMenu(fileName = "Tool", menuName = "Scriptable Objects/Tool")]
public class ToolScriptableObject : ScriptableObject {
    public Sprite toolSlotSprite;
    public Texture2D toolCursorTexture;
    public Sprite standardToolImage;
    public Sprite selectedToolImage;
    public ToolType toolType;
}

#pragma warning restore S1104 // Fields should not have public accessibility

