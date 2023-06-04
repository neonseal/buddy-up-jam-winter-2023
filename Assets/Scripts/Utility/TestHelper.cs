using UnityEngine.Assertions;
using UnityEngine;
/* User-defined Namespaces */
using GameState;

namespace Utility {
    public class TestHelper {
        public GameManager GetGameManager() {
            // Initialize new GameManger game object
            GameObject gameObject = new GameObject();
            GameManager gameManager = gameObject.AddComponent<GameManager>();
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
