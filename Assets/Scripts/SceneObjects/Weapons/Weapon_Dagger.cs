using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Dagger : WeaponBehavior {

    private Vector2 direction;

    protected override void Shoot(float speed, LookingDirection direction)
    {
        //setup direction
        spriteDirection.FaceDirection(direction);

        // setup velocity
        rigidbody2d.velocity = spriteDirection.WorldLookingDirection * speed;

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


    protected override void TriggeredWith(Collider2D collider)
    {
        return;
    }
}
