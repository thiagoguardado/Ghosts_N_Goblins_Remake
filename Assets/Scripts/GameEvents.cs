using System;

public static class GameEvents
{

    public static class Player
    {
        public static Action PlayerPickedWeapon;
        public static Action PlayerTookDamage;
        public static Action PlayerDied;
        public static Action PlayerJumped;
        public static Action PlayerLanded;
        public static Action PlayerShot;
    }


    public static class Level
    {
        public static Action LevelStarted;
        public static Action PlayerReachedEnd;
        public static Action TimeEnded;

    }

    public static class Score
    {
        public static Action ScoreIncremented;
        public static Action TopScoreChanged;
    }

    public static class Collectibles
    {
        public static Action CollectedCoin;
        public static Action CollectedArmor;
        public static Action CollectedMoneyBag;
        public static Action CollectedDoll;
        public static Action CollectedAccessory;

    }

}
