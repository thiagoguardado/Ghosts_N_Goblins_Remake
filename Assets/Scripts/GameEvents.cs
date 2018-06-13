using System;

public static class GameEvents {

    public static Action PlayerPickedWeapon;
    public static Action PlayerTookDamage;
    public static Action PlayerDied;
    public static Action PlayerPickedItem;

    public static Action LevelStarted;
    public static Action PlayerReachedEnd;
    public static Action TimeEnded;

}

public static class MyExtensions
{
    public static void SafeCall(this Action action)
    {
        if (action != null)
        {
            action();
        }
    }

}
