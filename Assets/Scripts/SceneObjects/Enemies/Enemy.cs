using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectScore))]
[RequireComponent(typeof(CheckEnemyOverlapCollision))]
public class Enemy : MonoBehaviour, IWeaponHittable
{

    public enum EnemySize { Big, Normal, Small }

    [Header("Config")]
    public SpriteDirection spriteDirection;
    public int health;
    public bool displayScore = false;
    public EnemySize size;

    [Header("Check Overlapping Collision")]
    public int damageOnHit;
    protected CheckEnemyOverlapCollision checkCollision;


    [Header("VFX")]
    public VFXSpriteAnimation hitSpriteAnimation;
    public VFXSpriteAnimation destroySpriteAnimation;

    [Header("DropProbability")]
    public float dropPotProbability;


    protected virtual void Awake()
    {
        checkCollision = GetComponent<CheckEnemyOverlapCollision>();

        checkCollision.HitSomething += HitSomething;
    }


    protected virtual void OnDestroy()
    {
        checkCollision.HitSomething -= HitSomething;
    }


    protected virtual void Start()
    {
        return;
    }


    protected virtual void Update()
    {
        return;
    }


    protected virtual void HitSomething(IEnemyHittable objectHit)
    {
        objectHit.Hit(damageOnHit, transform.position);
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
            GetComponent<ObjectScore>().IncrementGameScore(checkCollision.thisCollider.bounds.center);
        }
        else
        {
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

            // notify event
            GameEvents.Enemies.EnemyHit.SafeCall();

        }
        else
        {
            PlayHitAnimation(destroySpriteAnimation, hitPoint, transform.position);
            Kill();

            // notify event
            GameEvents.Enemies.EnemyDeath.SafeCall(size);
        }


    }


}
