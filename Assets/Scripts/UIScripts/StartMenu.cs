using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StartMenu : MonoBehaviour
{
    private Button startButton;
   
    private void Awake() {
        startButton = GetComponentInChildren<Button>();
    }

    private void Start() {
        startButton.onClick.AddListener(StartGame);
    }

    private void StartGame() {
        startButton.gameObject.SetActive(false);

        CustomEventManager.Current.GameStart();
    }
}
