using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TestTools;
/* User-defined Namespaces */
using GameState;
using UserInterface;
using PlayArea; 

namespace Tests {
    public class PlayAreaUITest {
        private readonly HelperFunctions helper = new HelperFunctions();
        private readonly GameObject playAreaCanvas = GameObject.FindGameObjectWithTag("PlayAreaCanvas");

        [Test]
        public void NextClientButton() {
            GameManager gameManager = helper.GetGameManager();
            Assert.IsNotNull(playAreaCanvas);

            // Setup play workspace
            GameObject gameObject = new GameObject();
            Workspace workspace = gameObject.AddComponent<Workspace>();
            // Capture starting plushie index
            int startingIndex = workspace.CurrentPlushieIndex;

            // Ensure client bell only sends next client event when in workspace empty state
            Button nextClientBtn = helper.FindComponentInChildWithTag<Button>(playAreaCanvas, "Bell");
            Assert.AreNotEqual(default(Button), nextClientBtn);

            // Currently in Main Menu State => ensure nothing happens
            nextClientBtn.onClick.Invoke();
            // Plushie index should not change
            Assert.Equals(workspace.CurrentPlushieIndex, startingIndex);
        }
    }
}
