using DG.Tweening;
using System.Collections;
using UnityEngine;

public class RadioController : MonoBehaviour {
    [Header("Radio UI Elements")]
    [SerializeField] private RadioButton playPauseButton;
    [SerializeField] private RadioButton volumeButton;
    [SerializeField] private RadioButton skipButton;

    [Header("Track Controlling Elements")]
    [SerializeField] private AudioClip[] tracks;
    private AudioSource musicPlayer;
    private float currentVolume;
    private bool volumeDecreasing;
    [SerializeField] private float timeToFade;
    private int nextSongIndex;

    [Header("UI Animation Elements")]
    private readonly float volumeIncrementAngle = -50f;
    private readonly float playKnobPauseRotation = -50f;
    [SerializeField] private float rotationDuration;
    [SerializeField] private float clickScaleModifier;
    [SerializeField] private float clickDuration;

    private void Awake() {
        DontDestroyOnLoad(this.gameObject);
        DOTween.Init();

        musicPlayer = GetComponent<AudioSource>();
        currentVolume = musicPlayer.volume;
        volumeDecreasing = true;
        nextSongIndex = -1;

        RadioButton.OnRadioButtonClick += HandleRadioButtonClick;
    }

    private void HandleRadioButtonClick(RadioButton button) {
        if (button == this.playPauseButton) {
            PlayPauseMusic();
        } else if (button == this.volumeButton) {
            ChangeVolume();
        } else {
            SkipSong();
        }
    }

    private void PlayPauseMusic() {
        if (musicPlayer.isPlaying) {
            // Pause Music
            Vector3 rotated = new Vector3(0, 0, playKnobPauseRotation);
            playPauseButton.transform.DOLocalRotate(rotated, rotationDuration);
            musicPlayer.Pause();
        } else {
            // Play Music
            playPauseButton.transform.DOLocalRotate(Vector3.zero, rotationDuration);
            musicPlayer.Play();

        }
    }

    private void ChangeVolume() {
        if (volumeDecreasing) {
            float decrementAngle = volumeButton.transform.eulerAngles.z - volumeIncrementAngle - 360;

            if (musicPlayer.volume == 1.0f) {
                decrementAngle = volumeButton.transform.eulerAngles.z - (volumeIncrementAngle / 2) - 360;
                musicPlayer.volume -= 0.1f;
            } else if (musicPlayer.volume - 0.2f < 0.0f) {
                musicPlayer.volume = 0.0f;
                volumeDecreasing = false;
                decrementAngle = 95f;
            } else {
                musicPlayer.volume -= .2f;
            }

            Vector3 rotated = new Vector3(0, 0, decrementAngle);
            volumeButton.transform.DOLocalRotate(rotated, rotationDuration);

        } else {
            float incrementAngle = volumeButton.transform.eulerAngles.z + volumeIncrementAngle - 360;
            if (musicPlayer.volume == 0.0f) {
                incrementAngle = volumeButton.transform.eulerAngles.z + (volumeIncrementAngle / 2) - 360;
                musicPlayer.volume += 0.1f;
            } else if (musicPlayer.volume + 0.2f > 1.0f) {
                musicPlayer.volume = 1.0f;
                volumeDecreasing = true;
                incrementAngle = 210f;
            } else {
                musicPlayer.volume += .2f;
            }

            Vector3 rotated = new Vector3(0, 0, incrementAngle);
            volumeButton.transform.DOLocalRotate(rotated, rotationDuration);
        }

        // Update current volume
        currentVolume = musicPlayer.volume;
    }

    private void SkipSong() {
        Sequence sequence = DOTween.Sequence(); ;
        sequence.Append(skipButton.transform.DOScale(clickScaleModifier, clickDuration));
        sequence.SetLoops(2, LoopType.Yoyo);

        // Increment index
        if (nextSongIndex == 2) {
            nextSongIndex = 0;
        } else {
            nextSongIndex++;
        }

        StartCoroutine("SkipSongRoutine");
    }

    private IEnumerator SkipSongRoutine() {
        AudioClip upNext = tracks[nextSongIndex];
        float timer = 0.0f;

        // Fade currently playing track to zero, and swap tracks
        while (timer < timeToFade) {
            musicPlayer.volume = Mathf.Lerp(currentVolume, 0, timer / timeToFade);
            timer += Time.deltaTime;
            yield return null;
        }

        // Fade complete, swap tracks and bring volume back up
        musicPlayer.clip = upNext;
        musicPlayer.Play();
        timer = 0.0f;

        // Fade currently playing track to zero, and swap tracks
        while (timer < timeToFade) {
            musicPlayer.volume = Mathf.Lerp(0, currentVolume, timer / timeToFade);
            timer += Time.deltaTime;
            yield return null;
        }
    }
}
