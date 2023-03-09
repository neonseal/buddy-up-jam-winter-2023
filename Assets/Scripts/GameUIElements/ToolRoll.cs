using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameUI;
using GameData;

public class ToolRoll : MonoBehaviour
{
    private Tool[] tools;

    private void Awake() {
        tools = GetComponentsInChildren<Tool>();
    }

    public void ResetOtherToolSprite(Tool selectedTool) {
        foreach (Tool tool in tools) {
            if (!tool.toolScriptableObject.Equals(selectedTool.toolScriptableObject)) {
                // Check if resetting scissors or not
                if (tool.toolScriptableObject.toolType != ToolType.Scissors) {
                    tool.image.sprite = tool.toolScriptableObject.standardToolImage;
                } else {
                    Color visible = tool.image.color;
                    visible.a = 255;
                    tool.image.color = visible;
                }
            }
        }
    }
}
