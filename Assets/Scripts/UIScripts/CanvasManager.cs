using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tool;

namespace GameUI
{
    public class CanvasManager : MonoBehaviour
    {
        internal GameObject currentTool;

        private void Awake()
        {
            this.currentTool = null;
        }
    }
}

