using NUnit.Framework;
using UnityEngine;
/* User-defined Namespaces */
using GameState;

namespace Tests {
    public class HelperFunctions {
        public GameManager GetGameManager() {
            // Initialize new GameManger game object
            GameObject gameObject = new GameObject();
            GameManager gameManager = gameObject.AddComponent<GameManager>();
            Assert.IsNotNull(gameManager);
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
