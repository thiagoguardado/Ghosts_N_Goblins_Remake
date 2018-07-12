using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public static AudioManager Instance { get; private set; }

    public bool autoStartPlaying = true;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmAudioSource;
    [SerializeField] private AudioSource sfxAudioSource;

    [Header("Music")]
    public AudioClip map;
    public AudioClip opening;
    public AudioClip intro;
    public AudioClip bgm;
    public AudioClip boss;
    public AudioClip timer;

    [Header("Level SFX")]
    public AudioClip playerDied;
    public AudioClip gameOver;
    public AudioClip stageClear;
    public AudioClip timerExtended;
    public AudioClip doorOpen;

    [Header("SFX - Player")]
    public AudioClip jump;
    public AudioClip land;
    public AudioClip shot;
    public AudioClip hurt;
    public AudioClip pickupArmor;
    public AudioClip pickupWeapon;
    public AudioClip pickupTreasure;
    public AudioClip projectileBlocked;
    public AudioClip turnedIntoFrog;

    [Header("SFX Enemies")]
    public AudioClip enemyDeath;
    public AudioClip enemyHit;
    public AudioClip zombieSpawned;
    public AudioClip magicianSpawned;
    public AudioClip crow;

    [Header("SFX - GameManager")]
    public AudioClip extraLife;
    public AudioClip highScore;
    public AudioClip credit;


    private Coroutine playingBGMCoroutine;

    private void OnEnable()
    {
       
        GameEvents.Player.PlayerJumped += PlayJumpSFX;
        GameEvents.Player.PlayerLanded += PlayLandSFX;
        GameEvents.Player.PlayerShot += PlayShotSFX;
        GameEvents.Player.PlayerTookDamage += PlayHurtSFX;
        GameEvents.Player.PlayerPickedArmor += PlayPickedArmorSFX;
        GameEvents.Player.PlayerPickedWeapon += PlayPickedWeaponSFX;
        GameEvents.Player.PlayerPickedTreasure += PlayPickedTreasureSFX;
        GameEvents.Player.PlayerLoseLife += PlayDyingAudio;
        GameEvents.Player.PlayerGameOver += PlayGameOverAudio;
        GameEvents.Player.PlayerTurnedIntoFrog += PlayPlayerTurnedIntoFrogSFX;

        GameEvents.Enemies.EnemyDeath += PlayEnemyDeathSFX;
        GameEvents.Enemies.EnemyHit += PlayEnemyHitSFX;
        GameEvents.Enemies.ZombieSpawned += PlayZombieSpawnedSFX;
        GameEvents.Enemies.MagicianSpawned += PlayMagicianSpawnedSFX;
        GameEvents.Enemies.CrowStartedToFly += PlayCrowSFX;

        GameEvents.Weapons.ProjectileBlocked += PlayProjectileBlockedSFX;

        GameEvents.Level.LevelStarted += StartBGMWithoutOpening;
        GameEvents.Level.GameOver += PlayGameOverAudio;
        GameEvents.Level.TimerStarted += PlayTimerStarted;
        GameEvents.Level.TimerExtended += PlayTimerExtended;
        GameEvents.Level.PlayerReachedEnd += PlayStageClear;
        GameEvents.Level.BossReached += PlayBossAudio;
        GameEvents.Level.DoorOpen += PlayDoorOpenSFX;

        GameEvents.Score.PlayerReachedHighScore += PlayHighScoreSFX;

        GameEvents.GameManager.NewGameStarted += PlayNewGameSFX;
    }

    private void OnDisable()
    {
        GameEvents.Player.PlayerJumped -= PlayJumpSFX;
        GameEvents.Player.PlayerLanded -= PlayLandSFX;
        GameEvents.Player.PlayerShot -= PlayShotSFX;
        GameEvents.Player.PlayerTookDamage -= PlayHurtSFX;
        GameEvents.Player.PlayerPickedArmor -= PlayPickedArmorSFX;
        GameEvents.Player.PlayerPickedWeapon -= PlayPickedWeaponSFX;
        GameEvents.Player.PlayerPickedTreasure -= PlayPickedTreasureSFX;
        GameEvents.Player.PlayerLoseLife -= PlayDyingAudio;
        GameEvents.Player.PlayerGameOver -= PlayGameOverAudio;
        GameEvents.Player.PlayerTurnedIntoFrog -= PlayPlayerTurnedIntoFrogSFX;

        GameEvents.Enemies.EnemyDeath -= PlayEnemyDeathSFX;
        GameEvents.Enemies.EnemyHit -= PlayEnemyHitSFX;
        GameEvents.Enemies.ZombieSpawned -= PlayZombieSpawnedSFX;
        GameEvents.Enemies.MagicianSpawned -= PlayMagicianSpawnedSFX;
        GameEvents.Enemies.CrowStartedToFly -= PlayCrowSFX;

        GameEvents.Weapons.ProjectileBlocked -= PlayProjectileBlockedSFX;

        GameEvents.Level.LevelStarted -= StartBGMWithoutOpening;
        GameEvents.Level.GameOver -= PlayGameOverAudio;
        GameEvents.Level.TimerStarted -= PlayTimerStarted;
        GameEvents.Level.TimerExtended -= PlayTimerExtended;
        GameEvents.Level.PlayerReachedEnd -= PlayStageClear;
        GameEvents.Level.BossReached -= PlayBossAudio;
        GameEvents.Level.DoorOpen -= PlayDoorOpenSFX;

        GameEvents.Score.PlayerReachedHighScore -= PlayHighScoreSFX;

        GameEvents.GameManager.NewGameStarted -= PlayNewGameSFX;
    }

    public void Awake()
    {
        // singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            DestroyImmediate(gameObject);
        }


        if (autoStartPlaying)
            StartPlayingBGM(false);
            
    }

    private void PlayJumpSFX() { sfxAudioSource.PlayOneShot(jump); }
    private void PlayLandSFX() { sfxAudioSource.PlayOneShot(land); }
    private void PlayShotSFX() { sfxAudioSource.PlayOneShot(shot); }
    private void PlayHurtSFX() { sfxAudioSource.PlayOneShot(hurt); }
    private void PlayPickedArmorSFX() { sfxAudioSource.PlayOneShot(pickupArmor); }
    private void PlayPickedWeaponSFX() { sfxAudioSource.PlayOneShot(pickupWeapon); }
    private void PlayPickedTreasureSFX() { sfxAudioSource.PlayOneShot(pickupTreasure); }
    private void PlayProjectileBlockedSFX() { sfxAudioSource.PlayOneShot(projectileBlocked); }
    private void PlayDoorOpenSFX() { sfxAudioSource.PlayOneShot(doorOpen); }
    private void PlayHighScoreSFX() { sfxAudioSource.PlayOneShot(highScore); }
    private void PlayNewGameSFX() { sfxAudioSource.PlayOneShot(credit); }
    private void PlayEnemyDeathSFX() { sfxAudioSource.PlayOneShot(enemyDeath); }
    private void PlayEnemyHitSFX() { sfxAudioSource.PlayOneShot(enemyHit); }
    private void PlayZombieSpawnedSFX() { sfxAudioSource.PlayOneShot(zombieSpawned); }
    private void PlayMagicianSpawnedSFX() { sfxAudioSource.PlayOneShot(magicianSpawned); }
    private void PlayPlayerTurnedIntoFrogSFX() { sfxAudioSource.PlayOneShot(turnedIntoFrog); }
    private void PlayCrowSFX() { sfxAudioSource.PlayOneShot(crow); }

    private void StartPlayingBGM(bool includeOpening)
    {

        if (includeOpening)
        {
            StartBGMWithOpening();
        }
        else {
            StartBGMWithoutOpening();
        }
        
    }

    private void StartBGMWithOpening()
    {
        SubstitutePlayingSequenceOnBGM(new AudioClip[] { opening, intro, bgm }, true);
    }

    private void StartBGMWithoutOpening()
    {
        SubstitutePlayingSequenceOnBGM(new AudioClip[] { intro, bgm }, true);
    }

    private void PlayDyingAudio()
    {
        SubstitutePlayingSequenceOnBGM(new AudioClip[] { playerDied }, false);
    }

    private void PlayGameOverAudio()
    {
        SubstitutePlayingSequenceOnBGM(new AudioClip[] { gameOver }, false);
    }

    private void PlayTimerStarted()
    {
        SubstitutePlayingSequenceOnBGM(new AudioClip[] { timer }, true);
    }
    

    private void PlayTimerExtended()
    {
        SubstitutePlayingSequenceOnBGM(new AudioClip[] { timerExtended, bgm }, true);
    }

    private void PlayBossAudio()
    {
        SubstitutePlayingSequenceOnBGM(new AudioClip[] { boss }, true);
    }

    private void PlayStageClear()
    {
        SubstitutePlayingSequenceOnBGM(new AudioClip[] { stageClear }, false);
    }


    private void SubstitutePlayingSequenceOnBGM(AudioClip[] audios, bool isLastLoop)
    {
        if (playingBGMCoroutine != null)
        {
            StopCoroutine(playingBGMCoroutine);
        }

        playingBGMCoroutine = StartCoroutine(PlayAudioSequence(bgmAudioSource, audios, isLastLoop));
    }

    private IEnumerator PlayAudioSequence(AudioSource source, AudioClip[] audios, bool isLastLoop)
    {
        source.Stop();

        for (int i = 0; i < audios.Length; i++)
        {
            while (source.isPlaying)
            {
                yield return null;
            }

            source.clip = audios[i];
            source.loop = i != (audios.Length - 1) ? false : isLastLoop;
            source.Play();

        }

    }

}
