using System;
using UnityEngine;

public class CheckEnemyOverlapCollision : MonoBehaviour {

    public Collider2D thisCollider;
    private ContactFilter2D contactFilter = new ContactFilter2D();
    public LayerMask overlappingCollidingLayers;
    public int maxNumberOfCollisionsPerFrame = 5;
    private Collider2D[] overlapingColliders;

    public delegate void EnemyHittableFound(IEnemyHittable hitObject);
    public event EnemyHittableFound HitSomething;

    private void Start()
    {
        contactFilter.useLayerMask = true;
        contactFilter.layerMask = overlappingCollidingLayers;

        overlapingColliders = new Collider2D[maxNumberOfCollisionsPerFrame];
    }

    void Update()
    {
        CheckCollidersOverlap();
    }

    private void CheckCollidersOverlap()
    {
        int collisionsCount = thisCollider.OverlapCollider(contactFilter, overlapingColliders);
        for (int i = 0; i < collisionsCount; i++)
        {
            if (overlapingColliders[i] != null)
            {
                IEnemyHittable enemyHittable = overlapingColliders[i].attachedRigidbody.GetComponent<IEnemyHittable>();
                if (enemyHittable != null)
                {
                    if(HitSomething!=null)
                        HitSomething(enemyHittable);
                }

            }
        }

    }
}
