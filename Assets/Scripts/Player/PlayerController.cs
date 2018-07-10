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
    public float sideRaysDistanceFromCenter = 0.05f;


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
    public List<WeaponBehavior> instantiatedWeapons = new List<WeaponBehavior>();

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
        GameEvents.Level.TimeEnded += Die;
    }


    private void OnDisable()
    {
        GameEvents.Level.TimeEnded -= Die;
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
            playerTouchable.WasTouchedByPlayer();

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
        if (LevelController.Instance.InLevel)
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

        // serach center raycast
        if (SearchFloorFromPosition(transform.position) || SearchFloorFromPosition(transform.position + Vector3.left * sideRaysDistanceFromCenter) || SearchFloorFromPosition(transform.position + Vector3.right * sideRaysDistanceFromCenter))
        {

            isGrounded = true;

        } else
        {
            if (wasGrounded)
                UnGround();

            isGrounded = false;
        }

        wasGrounded = isGrounded;

    }

    private bool SearchFloorFromPosition(Vector3 position)
    {
        int layerMask = ~LayerMask.GetMask(new string[] { "Player", "Ladder", "IgnoreAll" });
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.down, groundingRayLength, layerMask);

        // check central raycast
        if (hit.transform != null)
        {
            Floor floor = hit.transform.gameObject.GetComponent<Floor>();
            if (floor != null && playerRigidbody.velocity.y <= 0)
            {

                Ground(floor, transform.position);
                return true;

            }
        }

        return false;
    }

    private void UnGround()
    {
        transform.parent = null;
    }

    private void Ground(Floor floor, Vector2 attachingPoint)
    {

        transform.parent = floor.transform;
        Vector3 pos = transform.position;
        pos.y = attachingPoint.y;
        transform.position = pos;

    }

    public bool ShootStanding()
    {
        return Shoot(weaponStandingSpawnPoint);
    }

    public bool ShootCrouched()
    {
        return Shoot(weaponCrouchedSpawnPoint);
    }

    private bool Shoot(Transform spawnPoint)
    {

        if (currentWeapon.maxWeaponsOnScreen > instantiatedWeapons.Count)
        {

            // notify event
            GameEvents.Player.PlayerShot.SafeCall();

            // instantiate weapon
            instantiatedWeapons.Add(currentWeapon.ShootWeapon(spawnPoint.position, spriteDirection.lookingDirection,this));
            return true;
        }

        return false;
    }


    internal void WeaponDestroyed(WeaponBehavior weapon)
    {
        instantiatedWeapons.Remove(weapon);
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

        // call events
        GameEvents.Player.PlayerTookDamage.SafeCall();

        // make invincible
        this.WaitAndAct(afterDamageInvincibleDuration, () => isReceivingDamage = true);
    }


    public void GetArmor()
    {
        currentArmorStatus = PlayerArmor.Armored;

        // notify event
        GameEvents.Player.PlayerPickedArmor.SafeCall();
    }


    public void Fall()
    {
        // change sprites
        currentArmorStatus = PlayerArmor.Dead;

        // call events
        GameEvents.Player.PlayerDied.SafeCall();
    }


    private void Die()
    {
        // change sprites
        currentArmorStatus = PlayerArmor.Dead;

        // call events
        GameEvents.Player.PlayerTookDamage.SafeCall();
        GameEvents.Player.PlayerDied.SafeCall();


    }

    public void Hit(int hitDamage)
    {

        if(isReceivingDamage)
        {

            ReceiveDamage();

            Debug.Log("Player Hit");

        }

    }

    public void ChangeWeapon(PlayerWeapon playerWeapon)
    {

        if (currentWeapon != playerWeapon)
        {
            currentWeapon = playerWeapon;
            GameEvents.Player.PlayerPickedWeapon.SafeCall();
        }
    }

    void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, Vector3.down * groundingRayLength);
        Debug.DrawRay(transform.position + Vector3.left * sideRaysDistanceFromCenter, Vector3.down * groundingRayLength);
        Debug.DrawRay(transform.position + Vector3.right * sideRaysDistanceFromCenter, Vector3.down * groundingRayLength);

    }
}
