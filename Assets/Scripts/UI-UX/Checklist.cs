using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PlayArea {
    public class Checklist : MonoBehaviour {
        /* Private Member Variables */
        private Button notepadBtn;
        [SerializeField]
        private GameObject focusedChecklist;
        [SerializeField]
        private GameObject checklistItemArea;
        private Button completeRepairBtn;

        private void Awake() {
            
        }
    }
}
