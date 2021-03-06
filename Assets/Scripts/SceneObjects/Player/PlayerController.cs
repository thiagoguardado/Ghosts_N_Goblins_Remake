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
    public float inputPauseWhenThrow;
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
    public float frogDuration = 5f;



    // control variables
    public bool movingPaused { get; private set;}
    private bool wasUpdatingInput = false;
    private float horizontalAxis;
    private float verticalAxis;
    private bool throwButton;
    private bool jumpButton;
    private bool isGrounded = false;
    private bool wasGrounded = false;
    public SpriteDirection spriteDirection;
    private bool isOnVictoryPose = false;
    private bool isFrog = false;
    [HideInInspector] public bool invincibleHack = false;
    

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
    public bool IsOnVictoryPose
    {
        get
        {
            return isOnVictoryPose;
        }
    }
    public bool IsFrog
    {
        get
        {
            return isFrog;
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
        isOnVictoryPose = false;
	movingPaused = false;

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

    public void SimulateHorizontalAxis(float simulatedValue)
    {
        horizontalAxis = simulatedValue;
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
            wasUpdatingInput = true;
        }
        else if(wasUpdatingInput){
            wasUpdatingInput = false;
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
            //Pause Input briefly
            movingPaused = true;
            this.WaitAndAct(inputPauseWhenThrow, () => movingPaused = false);

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

    public void ReceiveDamage(float direction) {

        isFrog = false;

        isReceivingDamage = false;

        switch (currentArmorStatus)
        {
            case PlayerArmor.Armored:
                PushPlayer(direction);
                LoseArmor();
                break;
            case PlayerArmor.Naked:
                PushPlayer(direction);
                Die();
                break;
            default:
                break;
        }

    }

    public void TurnFromHumanToFrog()
    {
        // Turn into frog
        isFrog = true;

        // notify event
        GameEvents.Player.PlayerTurnedIntoFrog.SafeCall();

        // make invincible
        isReceivingDamage = false;
        this.WaitAndAct(afterDamageInvincibleDuration, () => isReceivingDamage = true);

        // schedule return to human form
        this.WaitAndAct(frogDuration, () => TurnFromFrogToHuman());

    }

    public void TurnFromFrogToHuman()
    {
        if (isFrog)
        {
            isFrog = false;

            isReceivingDamage = false;
            this.WaitAndAct(afterDamageInvincibleDuration, () => isReceivingDamage = true);

        }

    }

    private void PushPlayer(float xDirection)
    {
        // call events
        GameEvents.Player.PlayerPushed.SafeCall(xDirection);
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
        GameEvents.Player.PlayerDied.SafeCall();


    }

    public void Hit(int hitDamage, Vector3 objectPosition)
    {
        if (hitDamage > 0)
        {
            if (!invincibleHack)
            {
                if (isReceivingDamage && LevelController.Instance.InLevel)
                {
                    float pushDirection = objectPosition.x >= transform.position.x ? -1 : 1;
                    ReceiveDamage(pushDirection);

                }
            }
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



    public void StartVictoryPose()
    {
       isOnVictoryPose = true;
    }

    public void StopVictoryPose()
    {
        isOnVictoryPose = false;
    }

    void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, Vector3.down * groundingRayLength);
        Debug.DrawRay(transform.position + Vector3.left * sideRaysDistanceFromCenter, Vector3.down * groundingRayLength);
        Debug.DrawRay(transform.position + Vector3.right * sideRaysDistanceFromCenter, Vector3.down * groundingRayLength);

    }
}
