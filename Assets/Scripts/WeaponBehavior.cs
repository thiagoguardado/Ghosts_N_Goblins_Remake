using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class WeaponBehavior : MonoBehaviour
{

    public abstract void Shoot(float shootSpeed);
    protected abstract void CollidedWith(GameObject go);
    protected abstract void Move();

    private void OnTriggerEnter2D(Collider2D collider)
    {
        CollidedWith(collider.gameObject);
    }

    private void Update()
    {
        Move();
    }


}