
using GameState;
using UnityEngine;
using UnityEngine.Assertions;

namespace Utility {
    public class Helper {
        public GameStateMachine GetGameManager() {
            // Initialize new GameManger game object
            GameObject gameObject = new GameObject();
            GameStateMachine gameManager = gameObject.AddComponent<GameStateMachine>();
            Assert.IsNotNull(gameManager);
            gameManager.InitializeGameManager();
            // We expect to start in the main menu state
            Assert.AreEqual(gameManager.CurrentState, gameManager.MainMenuState);
            return gameManager;
        }

        public T FindComponentInChildWithTag<T>(GameObject parent, string tag) where T : Component {
            Transform t = parent.transform;
            foreach (Transform tr in t) {
                if (tr.tag == tag) {
                    return tr.GetComponent<T>();
                }
            }

            return default(T);
        }
    }

}
