using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : Enemy
{

    [Header("Projectile")]
    public float rangeRadious;
    public Projectile projectilePrefab;
    public Transform projectileInstantiationAnchor;
    public float timeBetweenSpits;
    private float timer;

    [Header("Animation")]
    public Animator animator;

    private Collider2D playerInRange;

    protected override void Awake()
    {
        base.Awake();

        timer = timeBetweenSpits;
    }

    protected override void Update()
    {
        base.Update();

        CheckPlayerInRange();

        timer += Time.deltaTime;

        if (timer >= timeBetweenSpits && playerInRange != null)
        {
            SpitProjectile();
            timer = 0f;
        }

    }

    private void CheckPlayerInRange()
    {
        playerInRange = Physics2D.OverlapCircle(transform.position, rangeRadious, 1 << LayerMask.NameToLayer("Player"));
    }

    private void SpitProjectile()
    {
        Projectile projectile = Instantiate(projectilePrefab, projectileInstantiationAnchor.position, Quaternion.identity);
        Vector2 shootDirection = playerInRange.bounds.center - projectileInstantiationAnchor.position;
        projectile.Init(shootDirection, spriteDirection.lookingDirection);

        animator.SetTrigger("Spit");
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, rangeRadious);
    }
}


