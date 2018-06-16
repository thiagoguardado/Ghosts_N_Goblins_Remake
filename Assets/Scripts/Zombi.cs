using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombi : Enemy, IWeaponHittable
{

    public void Hit(float damageTaken)
    {
        // kill object
        Kill();
    }

}
