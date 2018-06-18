using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum LookingDirection
{
    Left,
    Right
}


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
    public float afterDamageInvincibleDUration = 0.5f;
    private bool isReceivingDamage = true;
    

    // control variables
    private float horizontalAxis;
    private float verticalAxis;
    private bool throwButton;
    private bool jumpButton;
    private bool isGrounded;
    public LookingDirection currentDirection = LookingDirection.Right;

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
    }


    private void Update()
    {
        // get inputs
        ResolveInputs();

        // check if grounded
        CheckIfGrounded();

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

            if (hit.transform.gameObject.GetComponent<Floor>() != null && playerRigidbody.velocity.y <= 0)
            {
                isGrounded = true;
            }
        }
        else
        {
            isGrounded = false;
        }

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
        currentWeapon.ShootWeapon(spawnPoint.position, currentDirection);
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
        currentArmorStatus = PlayerArmor.Naked;
        GameEvents.PlayerTookDamage.SafeCall();
        StartCoroutine(WaitAndAct(afterDamageInvincibleDUration, () => isReceivingDamage = true));
    }

    private void Die()
    {
        currentArmorStatus = PlayerArmor.Dead;
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

    private IEnumerator WaitAndAct(float wait, Action action)
    {
        yield return new WaitForSeconds(wait);

        action.Invoke();
        
    }

}
