using System;
using UnityEngine;

public static class GameEvents
{

    public static class Player
    {
        public static Action PlayerPickedArmor;
        public static Action PlayerPickedWeapon;
        public static Action PlayerPickedTreasure;
        public static Action PlayerTookDamage;
        public static Action<float> PlayerPushed;
        public static Action PlayerDied;
        public static Action PlayerLoseLife;
        public static Action PlayerGetsExtraLife;
        public static Action PlayerJumped;
        public static Action PlayerLanded;
        public static Action PlayerShot;
        public static Action PlayerTurnedIntoFrog;
    }

    public static class Enemies
    {
        public static Action EnemyHit;
        public static Action<Enemy.EnemySize> EnemyDeath;
        public static Action ZombieSpawned;
        public static Action MagicianSpawned;
        public static Action CrowStartedToFly;
        public static Action RedArremerDive;
        public static Action EnemyEnteredScreen;
        public static Action EnemyLeftScreen;
        public static Action WoodyPigSpawned;


        public delegate void EnemyOnScreen<T>();
        public static event EnemyOnScreen<FlyingKnight> FlyingKnightEnteredScreen;
        public static event EnemyOnScreen<FlyingKnight> FlyingKnightLeftScreen;
        

        public static void TriggerFlyingKnightEnter()
        {
            if (FlyingKnightEnteredScreen != null)
                FlyingKnightEnteredScreen.Invoke();
        }

        public static void TriggerFlyingKnightLeft()
        {
            if (FlyingKnightLeftScreen != null)
                FlyingKnightLeftScreen.Invoke();
        }

    }


    public static class Weapons
    {
        public static Action TorchHitFloor;
        public static Action ProjectileBlocked;
    }

    public static class Level
    {
        public static Action<PlayerID> PlayerStarted;
        public static Action<PlayerID> PlayerGameOver;
        public static Action LevelStarted;
        public static Action PlayerReachedEnd;
        public static Action TimerStarted;
        public static Action TimerStopped;
        public static Action TimerExtended;
        public static Action TimeEnded;
        public static Action BossReached;
        public static Action DoorOpen;
        public static Action OpeningStarted;
    }

    public static class Score
    {
        public static Action ScoreIncremented;
        public static Action TopScoreChanged;
        public static Action PlayerReachedHighScore;
        public static Action<Vector3,int> BonusScore;
    }

    public static class GameManager
    {
        public static Action NewGameStarted;
    }


    /*
    public static class Collectibles
    {
        public static Action CollectedCoin;
        public static Action CollectedArmor;
        public static Action CollectedMoneyBag;
        public static Action CollectedDoll;
        public static Action CollectedAccessory;

    }
    */

}
