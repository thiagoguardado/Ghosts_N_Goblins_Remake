using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public bool autoStartPlaying = true;

    public AudioSource bgmAudioSource;
    public AudioSource sfxAudioSource;
    public AudioClip bgm;
    public AudioClip gameOver;

    private void OnEnable()
    {
        GameEvents.PlayerDied += PlayGameOver;
    }

    private void OnDisable()
    {
        GameEvents.PlayerDied -= PlayGameOver;
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

    private void PlayGameOver()
    {
        bgmAudioSource.Stop();
        bgmAudioSource.loop = false;
        bgmAudioSource.clip = gameOver;
        bgmAudioSource.Play();
    }
}
