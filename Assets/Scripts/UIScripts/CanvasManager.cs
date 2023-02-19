using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GameUI
{
    public class CanvasManager : MonoBehaviour
    {
        internal GameObject currentTool;

        internal void Awake()
        {
            this.currentTool = null;
        }
    }
}

