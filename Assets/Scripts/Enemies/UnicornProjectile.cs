using UnityEngine;

public class UnicornProjectile : Enemy
{

    public float speed;
    public Vector3 direction;

    protected override void Update()
    {
        base.Update();

        transform.position += spriteDirection.WorldLookingDirection * (speed * Time.deltaTime);
    }

    public void Init(Vector2 shootDirection)
    {
        direction = shootDirection;
    }

}

