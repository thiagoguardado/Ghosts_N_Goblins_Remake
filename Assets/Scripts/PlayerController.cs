using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour {

    public enum PlayerArmor
    {
        Armored,
        Naked
    }


    // references
    private Rigidbody2D playerRigidbody;

    [Header("Grounding")]
    public float groundingRayLength = 0.1f;


    [Header("Weapon Throwing")]
    [SerializeField] private PlayerWeapon currentWeapon;
    public Transform weaponStandingSpawnPoint;
    public Transform weaponCrouchedSpawnPoint;

    [Header("Life and Armor")]
    public PlayerArmor currentArmorStatus = PlayerArmor.Armored;
    public int lifes = 3;

    // control variables
    private float horizontalAxis;
    private float verticalAxis;
    private bool throwButton;
    private bool jumpButton;
    private bool isGrounded;

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

    
    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
    }


    private void Update()
    {
        // get inputs
        ResolveInputs();

        // check if grounded
        CheckIfGrounded();


        // ChangeArmorStatus
    }

    public void ResetAxis() {

        horizontalAxis = 0f;
        verticalAxis = 0f;

    }

    private void ResolveInputs()
    {

        horizontalAxis = Input.GetAxisRaw("Horizontal");
        verticalAxis = Input.GetAxisRaw("Vertical");
        throwButton = Input.GetButtonDown("Fire1");
        jumpButton = Input.GetButtonDown("Jump");

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

    public void ShootStanding(Vector3 forward)
    {
        Shoot(weaponStandingSpawnPoint, forward);
    }

    public void ShootCrouched(Vector3 forward)
    {
        Shoot(weaponCrouchedSpawnPoint, forward);
    }

    private void Shoot(Transform spawnPoint, Vector3 forward)
    {

        var instantiated = Instantiate(currentWeapon.weaponPrefab, spawnPoint.position, Quaternion.identity);
        instantiated.transform.right = forward;
        instantiated.Shoot(currentWeapon.initialVelocity);
    }

}
