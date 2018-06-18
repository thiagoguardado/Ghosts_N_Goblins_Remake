using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Spear : WeaponBehavior {

    private float speed;
    private Vector2 direction;

    public override void Shoot(float speed, LookingDirection direction)
    {
        this.speed = speed;

        switch (direction)
        {
            case LookingDirection.Left:
                transform.localScale = new Vector3(-1, 1, 1);
                this.direction = new Vector2(-1, 0) ;
                break;
            case LookingDirection.Right:
                transform.localScale = new Vector3(1, 1, 1);
                this.direction = new Vector2(1, 0);
                break;
            default:
                break;
        }

    }

    protected override void CollidedWith(GameObject go, Vector2 onPoint)
    {
        // look for hittable implementation
        var hitComponent = go.GetComponent<IWeaponHittable>();
        if (hitComponent != null)
        {
            hitComponent.Hit(damage, onPoint);
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
        rigidbody2d.velocity = direction * speed;
    }
}
