using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteDirection))]
public abstract class WeaponBehavior : MonoBehaviour
{

    [HideInInspector] public int damage;

    protected Rigidbody2D rigidbody2d;
    protected SpriteDirection spriteDirection;
    private PlayerController owner;

    protected abstract void Shoot(float shootSpeed, LookingDirection direction);
    protected abstract void CollidedWith(GameObject go, Vector2 onPoint, Vector2 normal);
    protected abstract void Move();

    protected virtual void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        spriteDirection = GetComponentInChildren<SpriteDirection>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.contacts.Length>0)
            CollidedWith(collision.gameObject, collision.contacts[0].point, collision.contacts[0].normal);
    }

    private void Update()
    {
        Move();
    }

    public void Init(float shootSpeed, LookingDirection direction, PlayerController owner)
    {
        this.owner = owner;

        Shoot(shootSpeed, direction);
    }

    private void OnDestroy()
    {
        owner.WeaponDestroyed(this);
    }

}
