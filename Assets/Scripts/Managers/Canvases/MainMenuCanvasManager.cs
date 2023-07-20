using System;
using UnityEngine;
using UnityEngine.UI;


namespace UserInterface {
    public class MainMenuCanvasManager : MonoBehaviour {
        /* Private Member Variables */
        private Canvas canvas;

        /* Main Menu Events */
        public static event Action OnStartButtonPressed;

        private void Awake() {
            SetupMainMenuCanvasManager();
        }

        private void SetupMainMenuCanvasManager() {
            canvas = GetComponent<Canvas>();
            Button startButton = GetComponentInChildren<Button>();
            startButton.onClick.AddListener(HandleStartButtonPressed);
        }

        // Hide the main menu UI elements and kick off the game
        private void HandleStartButtonPressed() {
            canvas.gameObject.SetActive(false);
            OnStartButtonPressed?.Invoke();
        }
    }
}

