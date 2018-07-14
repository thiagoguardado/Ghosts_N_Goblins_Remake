using System;
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
    //public AudioClip timerExtended;
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
    public AudioClip bigEnemyDeath;
    public AudioClip smallEnemyDeath;
    public AudioClip enemyHit;
    public AudioClip zombieSpawned;
    public AudioClip magicianSpawned;
    public AudioClip crow;
    public AudioClip redArremer;
    public AudioClip flyingKnight;
    public AudioClip woodyPig;
    private int flyingKnightCount = 0;
    private Coroutine flyingKnightSFXCoroutine;

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
        GameEvents.Player.PlayerTurnedIntoFrog += PlayPlayerTurnedIntoFrogSFX;
        GameEvents.Player.PlayerGetsExtraLife += PlayExtraLifeSFX;

        GameEvents.Enemies.EnemyDeath += PlayEnemyDeathSFX;
        GameEvents.Enemies.EnemyHit += PlayEnemyHitSFX;
        GameEvents.Enemies.ZombieSpawned += PlayZombieSpawnedSFX;
        GameEvents.Enemies.MagicianSpawned += PlayMagicianSpawnedSFX;
        GameEvents.Enemies.CrowStartedToFly += PlayCrowSFX;
        GameEvents.Enemies.RedArremerDive += PlayRedArremerDiveSFX;
        GameEvents.Enemies.FlyingKnightEnteredScreen += PlayFlyingKnightSFX;
        GameEvents.Enemies.FlyingKnightLeftScreen += StopFlyingKnightSFX;
        GameEvents.Enemies.WoodyPigSpawned += PlayWoodyPigSpawnedSFX;

        GameEvents.Weapons.ProjectileBlocked += PlayProjectileBlockedSFX;

        GameEvents.Level.OpeningStarted += StartOpening;
        GameEvents.Level.LevelStarted += StartBGMWithoutOpening;
        GameEvents.Level.TimerStarted += PlayTimerStarted;
        GameEvents.Level.TimerExtended += PlayTimerExtended;
        GameEvents.Level.PlayerReachedEnd += PlayStageClear;
        GameEvents.Level.BossReached += PlayBossAudio;
        GameEvents.Level.DoorOpen += PlayDoorOpenSFX;
        GameEvents.Level.PlayerGameOver += PlayGameOverAudio;

        //GameEvents.Score.PlayerReachedHighScore += PlayHighScoreSFX;

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
        GameEvents.Player.PlayerTurnedIntoFrog -= PlayPlayerTurnedIntoFrogSFX;
        GameEvents.Player.PlayerGetsExtraLife -= PlayExtraLifeSFX;

        GameEvents.Enemies.EnemyDeath -= PlayEnemyDeathSFX;
        GameEvents.Enemies.EnemyHit -= PlayEnemyHitSFX;
        GameEvents.Enemies.ZombieSpawned -= PlayZombieSpawnedSFX;
        GameEvents.Enemies.MagicianSpawned -= PlayMagicianSpawnedSFX;
        GameEvents.Enemies.CrowStartedToFly -= PlayCrowSFX;
        GameEvents.Enemies.RedArremerDive -= PlayRedArremerDiveSFX;
        GameEvents.Enemies.FlyingKnightEnteredScreen -= PlayFlyingKnightSFX;
        GameEvents.Enemies.FlyingKnightLeftScreen -= StopFlyingKnightSFX;
        GameEvents.Enemies.WoodyPigSpawned -= PlayWoodyPigSpawnedSFX;

        GameEvents.Weapons.ProjectileBlocked -= PlayProjectileBlockedSFX;

        GameEvents.Level.OpeningStarted -= StartOpening;
        GameEvents.Level.LevelStarted -= StartBGMWithoutOpening;
        GameEvents.Level.TimerStarted -= PlayTimerStarted;
        GameEvents.Level.TimerExtended -= PlayTimerExtended;
        GameEvents.Level.PlayerReachedEnd -= PlayStageClear;
        GameEvents.Level.BossReached -= PlayBossAudio;
        GameEvents.Level.DoorOpen -= PlayDoorOpenSFX;
        GameEvents.Level.PlayerGameOver -= PlayGameOverAudio;

        //GameEvents.Score.PlayerReachedHighScore -= PlayHighScoreSFX;

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
    private void PlayEnemyDeathSFX(Enemy.EnemySize size)
    {
        switch (size)
        {
            case Enemy.EnemySize.Big:
                sfxAudioSource.PlayOneShot(bigEnemyDeath);
                break;
            case Enemy.EnemySize.Normal:
                sfxAudioSource.PlayOneShot(enemyDeath);
                break;
            case Enemy.EnemySize.Small:
                sfxAudioSource.PlayOneShot(smallEnemyDeath);
                break;
        }

    }
    private void PlayEnemyHitSFX() { sfxAudioSource.PlayOneShot(enemyHit); }
    private void PlayZombieSpawnedSFX() { sfxAudioSource.PlayOneShot(zombieSpawned); }
    private void PlayMagicianSpawnedSFX() { sfxAudioSource.PlayOneShot(magicianSpawned); }
    private void PlayPlayerTurnedIntoFrogSFX() { sfxAudioSource.PlayOneShot(turnedIntoFrog); }
    private void PlayCrowSFX() { sfxAudioSource.PlayOneShot(crow); }
    private void PlayRedArremerDiveSFX() { sfxAudioSource.PlayOneShot(redArremer); }
    private void PlayWoodyPigSpawnedSFX() { sfxAudioSource.PlayOneShot(woodyPig); }
    private void PlayExtraLifeSFX() { sfxAudioSource.PlayOneShot(extraLife); }



    private void StartOpening()
    {
        SubstitutePlayingSequenceOnBGM(new AudioClip[] { opening}, false);
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

    private void PlayGameOverAudio(PlayerID playerID)
    {
        SubstitutePlayingSequenceOnBGM(new AudioClip[] { gameOver }, false);
    }

    private void PlayTimerStarted()
    {
        SubstitutePlayingSequenceOnBGM(new AudioClip[] { timer }, true);
    }
    

    private void PlayTimerExtended()
    {
        //SubstitutePlayingSequenceOnBGM(new AudioClip[] { bgm }, true);
        //SubstitutePlayingSequenceOnBGM(new AudioClip[] { timerExtended, bgm }, true);
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


    private void PlayFlyingKnightSFX()
    {
        flyingKnightCount += 1;

        if (flyingKnightCount == 1)
        {
            if (flyingKnightSFXCoroutine != null)
                StopCoroutine(flyingKnightSFXCoroutine);

            flyingKnightSFXCoroutine = StartCoroutine(FlyingKnightSFX());
        }
    }

    private void StopFlyingKnightSFX()
    {
        flyingKnightCount -= 1;
        if (flyingKnightCount <= 0)
        {
            flyingKnightCount = 0;
            if (flyingKnightSFXCoroutine != null)
                StopCoroutine(flyingKnightSFXCoroutine);
        }
        
    }
    private IEnumerator FlyingKnightSFX()
    {

        while (flyingKnightCount > 0)
        {
            sfxAudioSource.PlayOneShot(flyingKnight);

            yield return new WaitForSeconds(flyingKnight.length + 0.5f);
        }

    }

}
