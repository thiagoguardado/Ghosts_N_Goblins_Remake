using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXSpriteAnimationProjectileBlocked : VFXSpriteAnimation {


    void Awake()
    {
        GameEvents.Weapons.ProjectileBlocked.SafeCall();
    }

}
