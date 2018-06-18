using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectScore))]
public class Enemy : MonoBehaviour, IWeaponHittable {

    public int health;
    public int damageOnHit;
    public int maxNumberOfCollisionsPerFrame = 10;
    public Collider2D collider;
    private Collider2D[] overlapingColliders;
    private ContactFilter2D contactFilter;
    public LayerMask overlappingCollidingLayers;
    public VFXSpriteAnimation hitSpriteAnimation;
    public VFXSpriteAnimation destroySpriteAnimation;

    protected virtual void Awake()
    {
        contactFilter = new ContactFilter2D();
        contactFilter.useLayerMask = true;
        contactFilter.layerMask = overlappingCollidingLayers;

        overlapingColliders = new Collider2D[maxNumberOfCollisionsPerFrame];
    }


    protected virtual void Update()
    {

        CheckCollidersOverlap();

    }

    private void CheckCollidersOverlap()
    {
        int collisionsCount = collider.OverlapCollider(contactFilter, overlapingColliders);
        for (int i = 0; i < collisionsCount; i++)
        {
            if (overlapingColliders[i] != null)
            {
                IEnemyHittable enemyHittable = overlapingColliders[i].attachedRigidbody.GetComponent<IEnemyHittable>();
                if (enemyHittable != null)
                    enemyHittable.Hit(damageOnHit);
            }
        }

    }


    protected virtual void PlayHitAnimation(VFXSpriteAnimation hitAnimation, Vector2 hitPoint)
    {
        // Instantiate kill effect
        if(hitAnimation!=null)
            Instantiate(hitAnimation, hitPoint, Quaternion.identity);
    }

    protected virtual void Kill()
    {
        // icrement score
        GameController.Instance.IncrementScore(GetComponent<ObjectScore>().score);

        // kill object
        Destroy(gameObject);

    }

    public void Hit(int damageTaken, Vector2 hitPoint)
    {

        // decrease life
        health -= damageTaken;


        // kill object if life less than zero
        if (health > 0)
        {
            PlayHitAnimation(hitSpriteAnimation, hitPoint);
        }
        else
        {
            PlayHitAnimation(destroySpriteAnimation, hitPoint);
            Kill();
        }


    }


}
