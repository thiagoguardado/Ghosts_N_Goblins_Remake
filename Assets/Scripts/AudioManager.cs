using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public bool autoStartPlaying = true;

    [Header("Audio Sources")]
    public AudioSource bgmAudioSource;
    public AudioSource sfxAudioSource;

    [Header("Music")]
    public AudioClip opening;
    public AudioClip intro;
    public AudioClip bgm;
    public AudioClip playerDied;

    [Header("SFX")]
    public AudioClip jump;
    public AudioClip land;
    public AudioClip shot;
    public AudioClip hurt;
    public AudioClip pickupArmor;


    private void OnEnable()
    {
        GameEvents.Player.PlayerDied += PlayDyingAudio;

        GameEvents.Player.PlayerJumped += PlayJumpSFX;
        GameEvents.Player.PlayerLanded += PlayLandSFX;
        GameEvents.Player.PlayerShot += PlayShotSFX;
        GameEvents.Player.PlayerTookDamage += PlayHurtSFX;
        GameEvents.Player.PlayerPickedWeapon += PlayPickedArmorSFX;
    }

    private void OnDisable()
    {
        GameEvents.Player.PlayerDied -= PlayDyingAudio;

        GameEvents.Player.PlayerJumped -= PlayJumpSFX;
        GameEvents.Player.PlayerLanded -= PlayLandSFX;
        GameEvents.Player.PlayerShot -= PlayShotSFX;
        GameEvents.Player.PlayerTookDamage -= PlayHurtSFX;
        GameEvents.Player.PlayerPickedWeapon -= PlayPickedArmorSFX;
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

    private void PlayJumpSFX() { sfxAudioSource.PlayOneShot(jump); }
    private void PlayLandSFX() { sfxAudioSource.PlayOneShot(land); }
    private void PlayShotSFX() { sfxAudioSource.PlayOneShot(shot); }
    private void PlayHurtSFX() { sfxAudioSource.PlayOneShot(hurt); }
    private void PlayPickedArmorSFX() { sfxAudioSource.PlayOneShot(pickupArmor); }
}
