using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue {

    [CreateAssetMenu(fileName = "Tutorial Sequence", menuName = "Scriptable Objects/Tutorial Sequence")]
    public class TutorialSequenceScriptableObject : ScriptableObject {
        public string sequenceName;
        public TutorialStep[] tutorialSteps;
    }
}

