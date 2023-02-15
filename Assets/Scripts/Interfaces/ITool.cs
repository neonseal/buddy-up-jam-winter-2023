using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tool {
    public enum ToolType {
        Scissors,
        Needle,
        Stuffing,
        Cleaning
    }

    public interface ITool {
        public void ApplyTool();
    }
}
