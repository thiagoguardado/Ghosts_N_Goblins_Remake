using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour, IEnemyHittable
{

    public enum PlayerArmor
    {
        Armored,
        Naked,
        Dead
    }

    // singleton
    private static PlayerController instance;
    public static PlayerController Instance
    {
        get
        {
            return instance;
        }
    }

    // references
    private Rigidbody2D playerRigidbody;

    [Header("Grounding")]
    public float groundingRayLength = 0.1f;


    [Header("Weapon Throwing")]
    [SerializeField] private PlayerWeapon currentWeapon;
    public PlayerWeapon CurrentWeapon
    {
        get
        {
            return currentWeapon;
        }
    }
    public Transform weaponStandingSpawnPoint;
    public Transform weaponCrouchedSpawnPoint;

    [Header("Life and Armor")]
    public PlayerArmor currentArmorStatus = PlayerArmor.Armored;
    public float afterDamageInvincibleDuration = 0.5f;
    public bool isReceivingDamage { get; private set; }
    public BreakableArmor breakableArmor;


    // control variables
    private float horizontalAxis;
    private float verticalAxis;
    private bool throwButton;
    private bool jumpButton;
    private bool isGrounded = false;
    private bool wasGrounded = false;
    public SpriteDirection spriteDirection;
    //public LookingDirection currentDirection = LookingDirection.Right;

    // encapsulated variables
    public float HorizontalAxis
    {
        get
        {
            return horizontalAxis;
        }
    }
    public float VerticalAxis
    {
        get
        {
            return verticalAxis;
        }
    }
    public bool ThrowButton
    {
        get
        {
            return throwButton;
        }
    }
    public bool JumpButton
    {
        get
        {
            return jumpButton;
        }
    }
    public bool IsGrounded
    {
        get
        {
            return isGrounded;
        }
    }


    private void OnEnable()
    {
        GameEvents.TimeEnded += Die;
    }


    private void OnDisable()
    {
        GameEvents.TimeEnded -= Die;
    }

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();

        instance = this;

        isReceivingDamage = true;
    }


    private void Update()
    {
        // get inputs
        ResolveInputs();

        // check if grounded
        CheckIfGrounded();

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        IPlayerTouchable playerTouchable = collision.GetComponentInParent<IPlayerTouchable>();
        if (playerTouchable != null)
            playerTouchable.WasTouchesByPlayer();

    }

    public void ResetAxis() {

        horizontalAxis = 0f;
        verticalAxis = 0f;

    }

    private void ResetButtons()
    {
        throwButton = false;
        jumpButton = false;
    }

    private void ResolveInputs()
    {
        if (GameController.Instance.InLevel)
        {
            horizontalAxis = Input.GetAxisRaw("Horizontal");
            verticalAxis = Input.GetAxisRaw("Vertical");
            throwButton = Input.GetButtonDown("Fire1");
            jumpButton = Input.GetButtonDown("Jump");
        }
        else {
            ResetAxis();
            ResetButtons();
        }
    }



    private void CheckIfGrounded()
    {

        int layerMask = ~LayerMask.GetMask( new string[] {"Player","Ladder"});
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundingRayLength, layerMask);
        Debug.DrawRay(transform.position, Vector3.down * groundingRayLength);

        if (hit.transform != null)
        {
            Floor floor = hit.transform.gameObject.GetComponent<Floor>();
            if (floor != null && playerRigidbody.velocity.y <= 0)
            {

                if (!wasGrounded)
                    Ground(floor);

                isGrounded = true;
            }
        }
        else
        {

            if (wasGrounded)
                UnGround();

            isGrounded = false;
        }

        wasGrounded = isGrounded;

    }

    private void UnGround()
    {
        transform.parent = null;
    }

    private void Ground(Floor floor)
    {

        transform.parent = floor.transform;

    }

    public void ShootStanding()
    {
        Shoot(weaponStandingSpawnPoint);
    }

    public void ShootCrouched()
    {
        Shoot(weaponCrouchedSpawnPoint);
    }

    private void Shoot(Transform spawnPoint)
    {
        currentWeapon.ShootWeapon(spawnPoint.position, spriteDirection.lookingDirection);
    }

    public void ReceiveDamage() {

        isReceivingDamage = false;

        switch (currentArmorStatus)
        {
            case PlayerArmor.Armored:
                LoseArmor();
                break;
            case PlayerArmor.Naked:
                Die();
                break;
            default:
                break;
        }

        

    }

    private void LoseArmor()
    {
        // remove armor
        currentArmorStatus = PlayerArmor.Naked;
        breakableArmor.Break();

        // call event
        GameEvents.PlayerTookDamage.SafeCall();

        // make invincible
        this.WaitAndAct(afterDamageInvincibleDuration, () => isReceivingDamage = true);
    }

    private void Die()
    {
        // change sprites
        currentArmorStatus = PlayerArmor.Dead;

        // call events
        GameEvents.PlayerTookDamage.SafeCall();
        GameEvents.PlayerDied.SafeCall();
    }

    public void Hit(int hitDamage)
    {

        if(isReceivingDamage)
        {

            ReceiveDamage();

            Debug.Log("Player Hit");

        }

    }

}
