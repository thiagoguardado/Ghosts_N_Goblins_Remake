using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gravestone : MonoBehaviour, IWeaponHittable
{
    public int hitCount;
    public VFXSpriteAnimation hitSpriteVFXPrefab;
    public Text hitCountText;

    public void Hit(int damageTaken, Vector2 hitPoint, LookingDirection hitDirection)
    { 
        hitCount += 1;
        Debug.Log("Gravestone hit!");

        if(hitCountText!=null)
            hitCountText.text = "Hits: " + hitCount.ToString();

        // play vfx
        VFXSpriteAnimation vfx = Instantiate(hitSpriteVFXPrefab, hitPoint, Quaternion.identity);
        SpriteDirection dir = vfx.GetComponent<SpriteDirection>();
        if (dir != null)
        {
            dir.FaceDirection(SpriteDirection.OtherDirectionY(hitDirection));
        }

    }
}
