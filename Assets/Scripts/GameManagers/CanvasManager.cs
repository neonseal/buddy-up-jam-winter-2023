using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GameUI
{
    public class CanvasManager : MonoBehaviour
    {
        internal static GameObject currentTool;

        internal void Awake()
        {
            CanvasManager.currentTool = null;
        }
    }
}

