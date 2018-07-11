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
        public static Action PlayerDied;
        public static Action PlayerLoseLife;
        public static Action PlayerGameOver;
        public static Action PlayerJumped;
        public static Action PlayerLanded;
        public static Action PlayerShot;
    }

    public static class Weapons
    {
        public static Action TorchHitFloor;
        public static Action ProjectileBlocked;
    }

    public static class Level
    {
        public static Action LevelStarted;
        public static Action PlayerReachedEnd;
        public static Action TimerStarted;
        public static Action TimerExtended;
        public static Action TimeEnded;
        public static Action GameOver;
        public static Action BossReached;
        public static Action DoorOpen;
    }

    public static class Score
    {
        public static Action ScoreIncremented;
        public static Action TopScoreChanged;
        public static Action<Vector3,int> BonusScore;
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
