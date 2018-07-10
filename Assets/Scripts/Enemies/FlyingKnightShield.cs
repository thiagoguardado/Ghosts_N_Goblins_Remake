using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingKnightShield : MonoBehaviour, IWeaponHittable {

    private FlyingKnight flyingKnight;

    private void Awake()
    {
        flyingKnight = GetComponentInParent<FlyingKnight>();
    }

    public void Hit(int damageTaken, Vector2 hitPoint, LookingDirection hitDirection)
    {
        flyingKnight.ShieldHit(hitPoint, transform.position);
    }
}
