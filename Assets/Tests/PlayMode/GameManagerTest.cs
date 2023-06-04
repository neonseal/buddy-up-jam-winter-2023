using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
/* User-defined Namespaces */
using GameState;

namespace Tests {
    public class GameManagerTest {
        private readonly HelperFunctions helper = new HelperFunctions();

        // Test switching through all game states
        [Test]
        public void GameStateSwitching() {
            GameManager gameManager = helper.GetGameManager();

            // We expect to start in the main menu state
            Assert.AreEqual(gameManager.CurrentState, gameManager.MainMenuState);

            // Test all game states to ensure proper switching and setup logic occurs
            gameManager.SwitchGameState(gameManager.WorkspaceEmptyState);
            Assert.AreEqual(gameManager.CurrentState, gameManager.WorkspaceEmptyState);

            gameManager.SwitchGameState(gameManager.PlushieActiveState);
            Assert.AreEqual(gameManager.CurrentState, gameManager.PlushieActiveState);
        }
    }
}

