using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Weapon_Torch : WeaponBehavior {

    public float throwingAngle = 45f;

    public GameObject bigFire;
    public GameObject smallFire;
    public float bigFireDuration;
    public float smallFireDuration;

    private Coroutine fireCoroutine;

    protected override void Awake()
    {
        base.Awake();

        bigFire.SetActive(false);
        smallFire.SetActive(false);
        
    }
    

    protected override void Shoot(float shootSpeed, LookingDirection direction)
    {
        // set drirection
        spriteDirection.FaceDirection(direction);

        // setup velocity
        rigidbody2d.velocity = new Vector2(SpriteDirection.LookingDirectionToVector2(direction).x * Mathf.Sin(throwingAngle), Mathf.Cos(throwingAngle)) * shootSpeed;

        
    }

    protected override void CollidedWith(GameObject go, Vector2 onPoint, Vector2 normal)
    {
        // look for hittable implementation
        var hitComponent = go.GetComponent<IWeaponHittable>();
        if (hitComponent != null)
        {
            hitComponent.Hit(damage, onPoint, SpriteDirection.ConvertVectorToLookingDirection(normal));
  
        }

        if (go.layer == LayerMask.NameToLayer("Floor"))
        {
            transform.parent = go.transform;
            transform.position = new Vector3(onPoint.x, onPoint.y, 0);
            transform.rotation = Quaternion.identity;
            rigidbody2d.velocity = Vector2.zero;
            rigidbody2d.angularVelocity = 0f;
            rigidbody2d.gravityScale = 0f;
            fireCoroutine = StartCoroutine(FloorFireCoroutine());
            return;
        }

        if (fireCoroutine == null)
            Destroy(gameObject);

    }

    IEnumerator FloorFireCoroutine()
    {
        spriteDirection.gameObject.SetActive(false);
        bigFire.SetActive(true);

        yield return new WaitForSeconds(bigFireDuration);

        bigFire.SetActive(false);
        smallFire.SetActive(true);

        yield return new WaitForSeconds(smallFireDuration);

        Destroy(gameObject);

    }


    public void StopFire()
    {
        if (fireCoroutine != null)
        {
            StopCoroutine(fireCoroutine);
            Destroy(gameObject);
        }

    }

    protected override void TriggeredWith(Collider2D collider)
    {
        // look for hittable implementation
        var hitComponent = collider.GetComponentInParent<IWeaponHittable>();
        if (hitComponent != null)
        {
            hitComponent.Hit(damage, collider.transform.position, SpriteDirection.ConvertVectorToLookingDirection(MyExtensions.FromVector3(collider.transform.position - transform.position)));

        }
    }
}
