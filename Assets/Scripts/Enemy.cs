using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour {

    public int damageOnHit;
    public int maxNumberOfCollisionsPerFrame = 10;
    public Collider2D collider;
    private Collider2D[] overlapingColliders;
    private ContactFilter2D contactFilter;
    public LayerMask collidingLayers;

    protected virtual void Awake()
    {
        contactFilter = new ContactFilter2D();
        contactFilter.useLayerMask = true;
        contactFilter.layerMask = collidingLayers;

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


}
