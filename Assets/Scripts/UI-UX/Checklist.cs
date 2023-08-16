using Dialogue;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using MendingGames;
using Scriptables.DamageInstructions;

namespace PlayArea {
    public class Checklist : MonoBehaviour {
        /* Private Member Variables */
        [Header("Primary Checklist Elements")]
        [SerializeField] private GameObject focusedChecklist;
        [SerializeField] private GameObject checklistItemArea;
        [SerializeField] private Button notepadBtn;
        [SerializeField] private Button completeRepairBtn;
        [SerializeField] private ChecklistLineItem checklistStepPrefab;

        [Header("Dialogue/Tutorial Elements")]
        [SerializeField] private TutorialManager tutorialManager;

        [Header("Checklist Status Elements")]
        private int checklistLineItemCount;

        private void Awake() {
            // Buttons start disabled => Enabled during play states
            notepadBtn.interactable = false;
            completeRepairBtn.interactable = false;

            checklistLineItemCount = 0;

            // Setup checklist UI interaction events
            notepadBtn.onClick.AddListener(HandleChecklistClick);
            MendingGameManager.OnMendingGameComplete += HandleCompletedMendingGameEvent;
        }

        private void Update() {
            // Check if player clicks anywhere outside of the checklist, and hide the checklist
            RaycastHit2D hit = Physics2D.Raycast(Input.mousePosition, Vector2.zero);

            if (
                Input.GetMouseButtonDown(0) &&
                focusedChecklist.activeInHierarchy &&
                (hit.collider == null || hit.collider.name != "Checklist") &&
                // If a tutorial is active, we don't want to hide the checklist prematurely
                !tutorialManager.TutorialActive
            ) {
                ShowHideChecklist(false);
            }
        }

        public void EnableNotepad() {
            notepadBtn.interactable = true;
        }

        public void DisableNotepad() {
            notepadBtn.interactable = false;
        }

        public void ShowHideChecklist(bool showChecklist) {
            focusedChecklist.SetActive(showChecklist);
        }
        public void InitializeChecklistForPlushie(PlushieDamageGO[] plushieDamages) {
            if (plushieDamages.Length == 0) {
                return;
            }
            // Set high-level checklist item count
            checklistLineItemCount = plushieDamages.Length;

            // Count up each type of damage present on plushie
            foreach (PlushieDamageType damageType in Enum.GetValues(typeof(PlushieDamageType))) {
                PlushieDamageGO[] subset = plushieDamages.Where(d => d.GetInitialDamageType() == damageType).ToArray();

                if (subset.Length > 0) {
                    ChecklistLineItem checklistLineItem = Instantiate(checklistStepPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                    checklistLineItem.transform.SetParent(checklistItemArea.transform);

                    string lineItemText = subset[0].GenerateChecklistLineItem(subset.Length);
                    checklistLineItem.SetParameters(lineItemText, subset);
                }
            }
        }

        private void HandleChecklistClick() {
            if (!focusedChecklist.activeInHierarchy) {
                ShowHideChecklist(true);
            }
        }

        private void HandleCompletedMendingGameEvent(DamageInstructrionsScriptableObject[] damageInstructions) {
            // Check initial damage type for instructions
            // Find corresponding checklist line item by type
            // If damage set contains more than one item, check all items for completion
            // If all items in set complete, check line item on checklist
            // If all line items checked, enable "Complete Repair" button
        }
    }
}
