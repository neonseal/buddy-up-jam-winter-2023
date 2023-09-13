using Dialogue;
using GameState;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace PlayArea {
    public class Checklist : MonoBehaviour {
        /* Private Member Variables */
        [Header("Primary Checklist Elements")]
        [SerializeField] private GameObject focusedChecklist;
        [SerializeField] private GameObject checklistItemArea;
        [SerializeField] private Button notepadBtn;
        [SerializeField] private Button completeRepairBtn;
        [SerializeField] private ChecklistLineItem checklistStepPrefab;
        [SerializeField] private AudioSource repairCompleteSound;
        private List<ChecklistLineItem> checklistLineItems;

        [Header("Dialogue/Tutorial Elements")]
        [SerializeField] private TutorialManager tutorialManager;

        [Header("Checklist Status Elements")]
        private int checklistLineItemCount;


        private void Awake() {
            // Buttons start disabled => Enabled during play states
            notepadBtn.interactable = false;
            completeRepairBtn.interactable = false;
            checklistLineItems = new List<ChecklistLineItem>();
            checklistLineItemCount = 0;

            // Setup checklist UI interaction events
            notepadBtn.onClick.AddListener(HandleChecklistClick);
            completeRepairBtn.onClick.AddListener(SendOffPlushie);
            PlushieDamageGO.OnPlushieDamageComplete += HandleDamageCompleteEvent;
            PlushieActiveState.OnPlushieCompleteEvent += EnableSendOff;
        }

        private void Update() {
            // Check if player clicks anywhere outside of the checklist, and hide the checklist
            RaycastHit2D hit = Physics2D.Raycast(Input.mousePosition, Vector2.zero);

            if (
                Input.GetMouseButtonDown(0) &&
                focusedChecklist.activeInHierarchy &&
                (hit.collider == null || hit.collider.tag != "Checklist") &&
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

            // Reset checklist
            foreach (ChecklistLineItem checklistLineItem in checklistLineItems) {
                DestroyImmediate(checklistLineItem.gameObject);
            }
            checklistLineItems.Clear();

            // Set high-level checklist item count
            checklistLineItemCount = plushieDamages.Length;

            // Count up each type of damage present on plushie
            foreach (PlushieDamageType damageType in Enum.GetValues(typeof(PlushieDamageType))) {
                PlushieDamageGO[] subset = plushieDamages.Where(d => d.DamageType == damageType).ToArray();

                if (subset.Length > 0) {
                    ChecklistLineItem checklistLineItem = Instantiate(checklistStepPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                    checklistLineItem.transform.SetParent(checklistItemArea.transform);

                    string lineItemText = subset[0].GenerateChecklistLineItem(subset.Length);
                    checklistLineItem.SetParameters(lineItemText, subset);
                    checklistLineItems.Add(checklistLineItem);
                }
            }
        }

        private void HandleChecklistClick() {
            if (!focusedChecklist.activeInHierarchy) {
                ShowHideChecklist(true);
            }
        }

        private void HandleDamageCompleteEvent(PlushieDamageGO plushieDamage) {
            // Get damage type
            PlushieDamageType damageType = plushieDamage.DamageType;
            // Check all other similar damage types on plushie
            PlushieDamageGO[] damageList = PlushieActiveState.CurrentPlushie.PlushieDamageList;
            PlushieDamageGO[] matchingDamageList = damageList.Where(d => d.DamageType == damageType).ToArray();
            int completedCount = matchingDamageList.Count(d => d.DamageRepairComplete);

            // If all similar damage types are complete, check box
            if (completedCount == matchingDamageList.Count()) {
                ChecklistLineItem lineItem = checklistLineItems.Find(lineItem => lineItem.PlushieDamageType == damageType);
                lineItem.CompleteLineItem();
            }
        }

        private void EnableSendOff(Plushie plushie) {
            completeRepairBtn.interactable = true;
            ShowHideChecklist(true);
        }

        private void SendOffPlushie() {
            repairCompleteSound.Play();
            if (tutorialManager.TutorialActive) {
                tutorialManager.ContinueTutorialSequence();
            }
            PlushieActiveState.CurrentPlushie.SendOffPlushie();
        }
    }
}
