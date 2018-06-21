using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public abstract class WeaponBehavior : MonoBehaviour
{

    public int damage;

    protected Rigidbody2D rigidbody2d;

    public abstract void Shoot(float shootSpeed, LookingDirection direction);
    protected abstract void CollidedWith(GameObject go, Vector2 onPoint);
    protected abstract void Move();
    protected abstract void Setup();

    private void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Setup();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.contacts.Length>0)
            CollidedWith(collision.gameObject, collision.contacts[0].point);
    }

    private void Update()
    {
        Move();
    }


}