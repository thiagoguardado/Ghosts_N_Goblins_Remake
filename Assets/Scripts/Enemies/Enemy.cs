using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectScore))]
public class Enemy : MonoBehaviour, IWeaponHittable {

    public bool displayScore = false;

    [Header("Health and Direction")]
    public SpriteDirection spriteDirection;
    public int health;

    [Header("Check Overlapping Collision")]
    public int damageOnHit;
    public int maxNumberOfCollisionsPerFrame = 10;
    public Collider2D enemyCollider;
    private Collider2D[] overlapingColliders;
    private ContactFilter2D contactFilter;
    public LayerMask overlappingCollidingLayers;

    [Header("VFX")]
    public VFXSpriteAnimation hitSpriteAnimation;
    public VFXSpriteAnimation destroySpriteAnimation;

    [Header("DropProbability")]
    public float dropPotProbability;


    protected virtual void Awake()
    {
        contactFilter = new ContactFilter2D();
        contactFilter.useLayerMask = true;
        contactFilter.layerMask = overlappingCollidingLayers;

        overlapingColliders = new Collider2D[maxNumberOfCollisionsPerFrame];
    }


    protected virtual void Start()
    {
        
    }


    protected virtual void Update()
    {

        CheckCollidersOverlap();

    }

    private void CheckCollidersOverlap()
    {
        int collisionsCount = enemyCollider.OverlapCollider(contactFilter, overlapingColliders);
        for (int i = 0; i < collisionsCount; i++)
        {
            if (overlapingColliders[i] != null)
            {
                IEnemyHittable enemyHittable = overlapingColliders[i].attachedRigidbody.GetComponent<IEnemyHittable>();
                if (enemyHittable != null)
                {
                    enemyHittable.Hit(damageOnHit);
                    HitSomething();
                }
                
            }
        }

    }

    protected virtual void HitSomething()
    {
        return;
    }

    protected virtual void PlayHitAnimation(VFXSpriteAnimation hitAnimation, Vector2 hitPoint, Vector3 enemyPosition)
    {
        // Instantiate kill effect
        if (hitAnimation != null)
        {
            switch (hitAnimation.instantiationPoint)
            {
                case VFXSpriteAnimation.InstantiationPoint.WeaponHit:
                    Instantiate(hitAnimation, hitPoint, Quaternion.identity);
                    break;
                case VFXSpriteAnimation.InstantiationPoint.EnemyTransform:
                    Instantiate(hitAnimation, enemyPosition, Quaternion.identity);
                    break;
                default:
                    break;
            }
           
        }
    }

    protected void Kill()
    {
        // add score to game controller
        IncrementScore();

        // drop pot
        DropPot();

        // kill object
        Destroy(gameObject);

    }

    private void DropPot()
    {
        if (UnityEngine.Random.value <= dropPotProbability)
        {
            DroppablesController.Instance.DropPot(transform.position);
        }
    }


    protected virtual void IncrementScore()
    {
        // add score to game controller
        if (displayScore)
        {
            GetComponent<ObjectScore>().IncrementGameScore(enemyCollider.bounds.center);
        }
        else {
            GetComponent<ObjectScore>().IncrementGameScore();
        }
        

    }


    public virtual void Hit(int damageTaken, Vector2 hitPoint, LookingDirection hitDirection)
    {

        // decrease life
        health -= damageTaken;


        // kill object if life less than zero
        if (health > 0)
        {
            PlayHitAnimation(hitSpriteAnimation, hitPoint, transform.position);
        }
        else
        {
            PlayHitAnimation(destroySpriteAnimation, hitPoint,transform.position);
            Kill();
        }


    }


}
