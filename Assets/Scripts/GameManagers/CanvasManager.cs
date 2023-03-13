using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;

namespace GameUI
{
    public class CanvasManager : MonoBehaviour
    {
        internal static GameObject currentTool;
        internal static ToolType toolType;
        internal void Awake()
        {
            CanvasManager.currentTool = null;
        }
    }
}

