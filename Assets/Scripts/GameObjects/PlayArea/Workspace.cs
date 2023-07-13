using System;
using System.Collections.Generic;
using UnityEngine;
/* User-defined Namespaces */
using GameState;

/// <summary>
/// Workspace Controller
/// 
/// This class maintains control of the client plushie list, listening for requests to load
/// in a new client/plushie and requests to send the plushie off after repairs are complete.
/// </summary>
namespace PlayArea {
    public class Workspace : MonoBehaviour {
        /* Private Member Variables */
        [SerializeField]
        private Sprite[] plushieList;
        private int currentPlushieIndex = -1;

        /* Public Event Actions */
        public static event Action OnClientloaded;

        private void Awake() {
            InitializeWorkspace();
        }

        public void InitializeWorkspace() {
            WorkspaceEmptyState.OnNextClientRequested += LoadNextClient;
        }

        private void LoadNextClient() {
            currentPlushieIndex++;

            // Load next plushie prefab
            // TODO: Load in new plushie with animation

            // Send task complete event
            OnClientloaded?.Invoke();
        }

        /* Public Properties */
        public int CurrentPlushieIndex { get => currentPlushieIndex; }
    }
}
