using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : Enemy
{

    [Header("Projectile")]
    public PlantProjectile projectilePrefab;
    public Transform projectileInstantiationAnchor;
    public float timeBetweenSpits;
    private float timer;

    [Header("Animation")]
    public Animator animator;

    protected override void Update()
    {
        base.Update();

        timer += Time.deltaTime;

        if (timer >= timeBetweenSpits)
        {
            SpitProjectile();
            timer = 0f;
        }

    }

    private void SpitProjectile()
    {
        PlantProjectile projectile = Instantiate(projectilePrefab, projectileInstantiationAnchor.position, Quaternion.identity);
        projectile.Init(spriteDirection.lookingDirection);

        animator.SetTrigger("Spit");
    }
}


