using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Torch : WeaponBehavior {

    public float throwingAngle = 45f;


    protected override void Shoot(float shootSpeed, LookingDirection direction)
    {

        rigidbody2d.velocity = new Vector2(SpriteDirection.LookingDirectionToVector2(direction).x * Mathf.Sin(throwingAngle), Mathf.Cos(throwingAngle)) * shootSpeed;

        spriteDirection.FaceDirection(direction);
    }

    protected override void CollidedWith(GameObject go, Vector2 onPoint, Vector2 normal)
    {
        // look for hittable implementation
        var hitComponent = go.GetComponent<IWeaponHittable>();
        if (hitComponent != null)
        {
            hitComponent.Hit(damage, onPoint, SpriteDirection.ConvertVectorToLookingDirection(normal));
            
        }

        Destroy(gameObject);
    }

    protected override void Move()
    {
        return;
    }


}
