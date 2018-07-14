using UnityEngine;

public class WoodyPigProjectile : Enemy {

    public float speed;

    protected override void Update()
    {
        base.Update();

        transform.position += spriteDirection.WorldLookingDirection * (speed * Time.deltaTime);
    }

    public void Init(LookingDirection lookingDirection)
    {
        if(lookingDirection == LookingDirection.Down)
        {
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }

        spriteDirection.FaceDirection(lookingDirection);
    }

	protected override void HitSomething(IEnemyHittable objectHit)
	{
		base.HitSomething(objectHit);

        // kill object
        Destroy(gameObject);
	}

}
