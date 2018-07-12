using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gravestone : MonoBehaviour, IWeaponHittable
{
    private int hitCount = 0;
    public VFXSpriteAnimation hitSpriteVFXPrefab;
    public Magician magicianPrefab;
    public int hitsToSummonMagician = 15;
    private bool magicianSummoned = false;

    public void Hit(int damageTaken, Vector2 hitPoint, LookingDirection hitDirection)
    { 
        hitCount += 1;

        // play vfx
        VFXSpriteAnimation vfx = Instantiate(hitSpriteVFXPrefab, hitPoint, Quaternion.identity);
        SpriteDirection dir = vfx.GetComponent<SpriteDirection>();
        if (dir != null)
        {
            dir.FaceDirection(SpriteDirection.OtherDirectionY(hitDirection));
        }

        if (!magicianSummoned && hitCount >= hitsToSummonMagician)
        {
            magicianSummoned = true;
            SummonMagician();
        }
    }

    private void SummonMagician()
    {
        Vector3 instantiationPoint = new Vector3(transform.position.x, GetComponentInChildren<BoxCollider2D>().bounds.max.y, transform.position.z);

        Instantiate(magicianPrefab, instantiationPoint, magicianPrefab.transform.rotation);
    }
}
