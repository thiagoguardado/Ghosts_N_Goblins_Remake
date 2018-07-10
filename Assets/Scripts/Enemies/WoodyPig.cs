using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodyPig : Enemy {

    [Header("Woody Pig Movement")]
    public Animator animator;
    public float horizontalSpeed = 0.5f;
    public float minDistanceToBoundsToTurn = 0.5f;
    public bool turn = false;
    private bool isTurningYMov;
    public float turningYMovSpeed;
    public float turningYMovDuration;

    [Header("Throwing")]
    public WoodyPigProjectile projectilePrefab;
    public Transform projectileShootingAnchor;
    private bool isPlayerUnder = false;
    public float rayCastLenght;
    public float shootingCooldown;
    public float minYDistanceFromPlayerToShootVertically = 1f;
    private bool canShoot = true;
    private bool shootVertically = true;

    private RaycastHit2D hit2D;

    protected override void Start()
    {
        // Start Looking To Player
        spriteDirection.FaceDirection(LookAtPlayer.LookToPlayerDirection(transform.position));

        // wait for cooldown before starts shooting
        canShoot = false;
        MyExtensions.WaitAndAct(this, shootingCooldown, () => canShoot = true);
    }
    

	protected override void Update()
	{
		base.Update();


        if(turn && !isTurningYMov)
        {
            Turn();
            turn = false;
        }

        UpdateShootingConditions();

        HandleShooting();

	}

    private void HandleShooting()
    {
        if (canShoot)
        {
            if (!shootVertically)
            {
                canShoot = false;
                ShootHorizontal();
                MyExtensions.WaitAndAct(this, shootingCooldown, () => canShoot = true);
            }
            else if (shootVertically && isPlayerUnder)
            {
                canShoot = false;
                ShootVertical();
                MyExtensions.WaitAndAct(this, shootingCooldown, () => canShoot = true);
            }
        }

    }

	public void Move()
    {
        transform.position += (spriteDirection.WorldLookingDirection) * horizontalSpeed * Time.deltaTime;
    }


    public void Turn()
    {
        animator.SetTrigger("Turn");

        isTurningYMov = true;
        StartCoroutine(MoveDownOnY());
    }

    private void UpdateShootingConditions()
    {

        // check if shoot vertically
        if (Mathf.Abs(transform.position.y - PlayerController.Instance.transform.position.y) >= minYDistanceFromPlayerToShootVertically)
        { shootVertically = true; }
        else { shootVertically = false; }

        // check if player underneath
        hit2D = Physics2D.Raycast(transform.position, Vector2.down, rayCastLenght, 1 << LayerMask.NameToLayer("Player"));
        isPlayerUnder = hit2D.collider != null;

    }

    private IEnumerator MoveDownOnY()
    {
        float timer = 0f;

        while (timer <= turningYMovDuration)
        {
            transform.position += Vector3.down * turningYMovSpeed * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }

        isTurningYMov = false;
    }

    public void ShootHorizontal()
    {
        WoodyPigProjectile projectil = Instantiate(projectilePrefab, projectileShootingAnchor.position, Quaternion.identity);
        projectil.Init(spriteDirection.lookingDirection);
        animator.SetTrigger("Shoot");
    }

    public void ShootVertical()
    {
        WoodyPigProjectile projectil = Instantiate(projectilePrefab, projectileShootingAnchor.position, Quaternion.identity);
        projectil.Init(LookingDirection.Down);
        animator.SetTrigger("Shoot");
    }

	private void OnDrawGizmos()
	{
        // raycast to player collider
        Debug.DrawRay(transform.position, Vector3.down * rayCastLenght);

        // min distance from player transform to shoot vertically
        Debug.DrawRay(transform.position, Vector3.down * minYDistanceFromPlayerToShootVertically, Color.red);
	}

    public bool CheckIfNearBounds()
    {

        if(!CameraController.Instance.cameraBounds.Bounds.Contains(new Vector2(transform.position.x + spriteDirection.WorldLookingDirection.x * minDistanceToBoundsToTurn, transform.position.y)))
        {
            return true;
        }
        return false;
    }
}
