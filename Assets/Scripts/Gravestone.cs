using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gravestone : MonoBehaviour, IWeaponHittable
{
    public int hitCount;
    public Animator hitApriteAnimator;
    public Text hitCountText;

    public void Hit(int damageTaken, Vector2 hitPoint)
    { 
        hitCount += 1;
        Debug.Log("Gravestone hit!");

        if(hitCountText!=null)
            hitCountText.text = "Hits: " + hitCount.ToString();
    }
}
