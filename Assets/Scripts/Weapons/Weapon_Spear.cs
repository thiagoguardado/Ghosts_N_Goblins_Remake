using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Spear : WeaponBehavior {

    private float speed;
    private Vector2 direction;

    public override void Shoot(float speed, LookingDirection direction)
    {
        this.speed = speed;

        spriteDirection.FaceDirection(direction);

    }

    protected override void CollidedWith(GameObject go, Vector2 onPoint, Vector2 normal)
    {
        // look for hittable implementation
        var hitComponent = go.GetComponent<IWeaponHittable>();
        if (hitComponent != null)
        {
            hitComponent.Hit(damage, onPoint, SpriteDirection.ConvertVectorToLookingDirection(normal));
            Destroy(gameObject);
        }
    }

    protected override void Move()
    {
        return;
        
    }

    protected override void Setup()
    {

        // setup velocity
        rigidbody2d.velocity = spriteDirection.WorldLookingDirection * speed;
    }
}
