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
    private Button volumeKnob;

    private AudioSource music;

    private bool volumeDecreasing = true;
    private float currentVolume;

    private void Awake() {
        playPauseButton = GetRadioButton(ButtonTypes.PlayPause);
        volumeKnob = GetRadioButton(ButtonTypes.Volume);
        music = GetComponent<AudioSource>();
        currentVolume = music.volume;
    }

    private void Start() {
        playPauseButton.onClick.AddListener(TogglePlayPause);
        volumeKnob.onClick.AddListener(ChangeVolume);
    }


    private void TogglePlayPause() {
        if (music.isPlaying) {
            music.Pause();
        } else {
            music.Play();
        }

    }

    private void ChangeVolume() {
        // If still decreasing, decrement volume and rotate knob
        if (volumeDecreasing) {
            music.volume -= 0.25f;
            volumeKnob.transform.Rotate(Vector3.forward, 45f);

            if (music.volume <= 0f) {
                volumeDecreasing = false;
            }
        } else {
            // Once reduced to zero, begin increasing until at max volume
            music.volume += 0.25f;
            volumeKnob.transform.Rotate(Vector3.forward, -45f);

            if (music.volume >= 1f) {
                volumeDecreasing = true;
            }
        }

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
