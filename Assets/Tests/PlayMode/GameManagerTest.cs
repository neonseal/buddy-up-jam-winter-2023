using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

using GameState;
using Utility;

namespace Tests {
    public class GameManagerTest {
        private readonly Helper helper = new Helper();

        // Test switching through all game states
        [Test]
        public void GameStateSwitching() {
            GameStateMachine gameManager = helper.GetGameManager();

            // Test all game states to ensure proper switching and setup logic occurs
            gameManager.SwitchGameState(gameManager.WorkspaceEmptyState);
            Assert.AreEqual(gameManager.CurrentState, gameManager.WorkspaceEmptyState);

            gameManager.SwitchGameState(gameManager.PlushieActiveState);
            Assert.AreEqual(gameManager.CurrentState, gameManager.PlushieActiveState);
        }
    }
}

