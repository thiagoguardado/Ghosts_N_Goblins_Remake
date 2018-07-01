using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : Enemy
{

    [Header("Projectile")]
    public float rangeRadious;
    public PlantProjectile projectilePrefab;
    public Transform projectileInstantiationAnchor;
    public float timeBetweenSpits;
    private float timer;

    [Header("Animation")]
    public Animator animator;

    private Collider2D playerInRange;

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
        PlantProjectile projectile = Instantiate(projectilePrefab, projectileInstantiationAnchor.position, Quaternion.identity);
        Vector2 shootDirection = playerInRange.bounds.center - projectileInstantiationAnchor.position;
        projectile.Init(shootDirection);

        animator.SetTrigger("Spit");
    }
}


