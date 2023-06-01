using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton Game Manager
/// 
/// Primary game state manager controlling flow of control between all game states 
/// throughout the course of the case. Uses singleton setup to maintain one source of truth.
/// </summary>

namespace GameState {
    public class GameManager : MonoBehaviour {

        /* Singleton Game Manager */
        private static GameManager s_instance;
        public static GameManager Instance {
            get { return s_instance; }
        }

        /* Private Member Variables */
        [Header("Game State Concrete Implementations")]
        private IGameState currentState;

        private void Awake() {
            // Instantiate game manager, and ensure duplicates are not created
            if (s_instance == null) {
                SetManager(this);
                DontDestroyOnLoad(gameObject);
            } else {
                Destroy(gameObject);
            }
        }

        // Use a static setter to instantiate the new Game Mamager instance, 
        // to avoid setting static variable in non-static member function
        private static void SetManager(GameManager gameManager) {
            s_instance = gameManager;
        }

        /* Public Properties */
        public IGameState CurrentState { get => currentState; }
    }
}

