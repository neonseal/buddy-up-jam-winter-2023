using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RadioController : MonoBehaviour {
    private enum ButtonTypes {
        Volume,
        PlayPause,
        Skip

    };

    private Button playPauseButton;
    private Button volumeButton;

    private AudioSource music;

    private void Awake() {
        playPauseButton = GetRadioButton(ButtonTypes.PlayPause);
        volumeButton = GetRadioButton(ButtonTypes.Volume);
        music = GetComponent<AudioSource>();
    }

    private void Start() {
        playPauseButton.onClick.AddListener(TogglePlayPause);
        volumeButton.onClick.AddListener(ChangeVolume);
    }


    private void TogglePlayPause() {
        Debug.Log("CLICK");

    }

    private void ChangeVolume() {
        Debug.Log("CLICKETY CLICK");

    }

    private Button GetRadioButton(ButtonTypes tag) {
        Button[] radioButtons = this.gameObject.GetComponentsInChildren<Button>();

        foreach (Button button in radioButtons) {
            if (button.tag == tag.ToString()) {
                return button;
            }
        }

        return null;
    }
}
