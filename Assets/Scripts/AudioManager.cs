using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public bool autoStartPlaying = true;

    public AudioSource bgmAudioSource;
    public AudioSource sfxAudioSource;
    public AudioClip bgm;
    public AudioClip playerDied;

    private void OnEnable()
    {
        GameEvents.PlayerDied += PlayDyingAudio;
    }

    private void OnDisable()
    {
        GameEvents.PlayerDied -= PlayDyingAudio;
    }

    public void Awake()
    {
        if(autoStartPlaying)
            StartPlayingBGM(bgm);
    }

    private void StartPlayingBGM(AudioClip bgm)
    {
        bgmAudioSource.loop = true;
        bgmAudioSource.clip = bgm;
        bgmAudioSource.Play();
    }

    private void PlayDyingAudio()
    {
        bgmAudioSource.Stop();
        bgmAudioSource.loop = false;
        bgmAudioSource.clip = playerDied;
        bgmAudioSource.Play();
    }
}
