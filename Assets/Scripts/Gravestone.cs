using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravestone : MonoBehaviour, IWeaponHittable
{
    public int hitCount;
    public Animator hitApriteAnimator;


    public void Hit(float damageTaken)
    { 
        hitCount += 1;
        Debug.Log("Gravestone hit!");
    }
}
