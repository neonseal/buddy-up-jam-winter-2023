using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TestTools;
/* User-defined Namespaces */
using GameState;
using PlayArea;
using Utility;
using UserInterface;

namespace Tests {
    public class PlayAreaTest: MonoBehaviour {
        private readonly Helper helper = new Helper();
        private readonly PlayAreaCanvasManager playAreaCanvas = GameObject.FindGameObjectWithTag("PlayAreaCanvas").GetComponent<PlayAreaCanvasManager>();
        private bool clientLoaded = false;

        [Test]
        public void NextClientButton() {
            GameManager gameManager = helper.GetGameManager();
            Assert.IsNotNull(playAreaCanvas);
            playAreaCanvas.InitializeCanvasManager();

            // Setup play workspace
            GameObject gameObject = new GameObject();
            Workspace workspace = gameObject.AddComponent<Workspace>();
            workspace.InitializeWorkspace();

            // Capture starting plushie index
            int startingIndex = workspace.CurrentPlushieIndex;

            // Ensure client bell only sends next client event when in workspace empty state
            Button nextClientBtn = helper.FindComponentInChildWithTag<Button>(playAreaCanvas.gameObject, "Bell");
            Assert.AreNotEqual(default(Button), nextClientBtn);

            // Main Menu State => Should do nothing
            nextClientBtn.onClick.Invoke();
            // Plushie index should not change
            Assert.AreEqual(startingIndex, workspace.CurrentPlushieIndex);

            // Workspace Empty State => Should increment index and switch to play state
            gameManager.SwitchGameState(gameManager.WorkspaceEmptyState);
            Assert.AreEqual(gameManager.WorkspaceEmptyState, gameManager.CurrentState);

            // Set up client loaded event listener
            Workspace.OnClientloaded += () => { clientLoaded = true; };

            // Invoke next client button and ensure client/plushie loaded
            nextClientBtn.onClick.Invoke();
            Assert.AreEqual(true, clientLoaded);
            Assert.AreEqual(startingIndex + 1, workspace.CurrentPlushieIndex);
        }

    }
}
