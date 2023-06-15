using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PlayArea {
    public class Checklist : MonoBehaviour {
        /* Private Member Variables */
        [SerializeField]
        private GameObject focusedChecklist;
        [SerializeField]
        private GameObject checklistItemArea;
        [SerializeField]
        private Button notepadBtn;
        [SerializeField]
        private Button completeRepairBtn;

        private void Awake() {
            // Buttons start disabled => Enabled during play states
            notepadBtn.interactable = false;
            completeRepairBtn.interactable = false;
        }

        public void EnableNotepad() {
            notepadBtn.interactable = true;
        }
    }
}
