using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFSM : MonoBehaviour {

    public enum PlayerFSMState
    {
        Idle,
        Running,
        Jumping,
        Crouched,
        Climbing,
        StandupAttack,
        CrouchedAttack
    }

    private enum Direction
    {
        Left,
        Right
    }

    [Header("FSM")]
    private PlayerState currentPlayerState;
    public Dictionary<PlayerFSMState, PlayerState> statesDictionary = new Dictionary<PlayerFSMState, PlayerState>();

    private PlayerAnimationController animationController;
    private Rigidbody2D playerRigidbody;

    [Header("Horizontal Translation")]
    public Transform spriteTransform;
    public float horizontalSpeed = 5;
    private float horizontalAxis;
    private Direction currentDirection = Direction.Right;
    private bool walkPaused = false;


    [Header("Vertical Translation")]
    public float verticalSpeed = 5;
    private float verticalAxis;

    [Header("Jumping")]
    public float groundingRayLength = 0.1f;
    public float jumpForce = 1f;
    private bool isJumping;
    private bool isGrounded = false;
    private bool jumpButton = false;

    [Header("Throwing")]
    private bool throwButton;


    private void Awake()
    {

        // get references
        animationController = GetComponent<PlayerAnimationController>();
        playerRigidbody = GetComponent<Rigidbody2D>();


        // Create FSM States
        PlayerState idleState = new PlayerState_Idle(this, PlayerFSMState.Idle);
        PlayerState runningState = new PlayerState_Running(this, PlayerFSMState.Running);
        PlayerState jumpingState = new PlayerState_Jumping(this, PlayerFSMState.Jumping);


        // assign initial state
        currentPlayerState = idleState;

    }


    private void Update()
    {
        // check if grounded
        CheckIfGrounded();

        // Get Inputs
        ResolveInputs();

        // update fsm state
        currentPlayerState.UpdateState();
        currentPlayerState.CheckTransition();

    }




    private void ChangeState(PlayerFSMState state)
    {

        if (statesDictionary.ContainsKey(state))
        {

            currentPlayerState.DoBeforeLeave();
            currentPlayerState = statesDictionary[state];
            currentPlayerState.DoBeforeEntering();
        }
        else
        {
            Debug.LogError("There is no state " + state.ToString() + " in dictionary");
        }

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

        int layerMask = ~LayerMask.GetMask("Player");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundingRayLength, layerMask);
        Debug.DrawRay(transform.position, Vector3.down * groundingRayLength);

        if (hit.transform != null)
        {

            if (hit.transform.gameObject.GetComponent<Floor>() != null && playerRigidbody.velocity.y < 0)
            {
                isGrounded = true;
            }
        }
        else
        {
            isGrounded = false;
        }



    }


    // walk
    private void Translate(float horizontalAxis)
    {

        if (!walkPaused)
        {
            // translate
            transform.Translate(Vector3.right * horizontalAxis * horizontalSpeed * Time.deltaTime);

            // turn
            if (horizontalAxis > 0 && currentDirection == Direction.Left)
            {
                // turn to right
                Vector3 scale = spriteTransform.localScale;
                scale.x = 1;
                spriteTransform.localScale = scale;
                currentDirection = Direction.Right;

            }
            else if (horizontalAxis < 0 && currentDirection == Direction.Right)
            {
                // turn to left
                Vector3 scale = spriteTransform.localScale;
                scale.x = -1;
                spriteTransform.localScale = scale;
                currentDirection = Direction.Left;
            }

            
        }


    }

    // Jump
    private void Jump()
    {
        playerRigidbody.AddForce(Vector3.up * jumpForce); 
        isJumping = true;
    }



    #region PlayerState Classes
    public abstract class PlayerState
    {

        protected PlayerFSM owner;
        protected PlayerFSM.PlayerFSMState state;

        public PlayerState(PlayerFSM owner, PlayerFSM.PlayerFSMState state)
        {
            this.owner = owner;
            this.state = state;
            owner.statesDictionary.Add(state, this);
        }

        public abstract void UpdateState();
        public abstract void CheckTransition();
        public abstract void DoBeforeLeave();
        public abstract void DoBeforeEntering();

    }

    public class PlayerState_Idle : PlayerState
    {
        public PlayerState_Idle(PlayerFSM owner, PlayerFSM.PlayerFSMState state) : base(owner, state) { }


        public override void UpdateState()
        {
            return;
        }

        public override void CheckTransition()
        {
            if (owner.horizontalAxis != 0)
            {
                owner.ChangeState(PlayerFSM.PlayerFSMState.Running);
            }

            if (owner.jumpButton)
            {
                owner.ChangeState(PlayerFSMState.Jumping);
            }
        }

        public override void DoBeforeLeave()
        {
            return;
        }

        public override void DoBeforeEntering()
        {
            // update animator
            owner.animationController.isRunning = false;
        }
    }

    public class PlayerState_Running : PlayerState
    {
        public PlayerState_Running(PlayerFSM owner, PlayerFSM.PlayerFSMState state) : base(owner, state)
        {
        }

        public override void CheckTransition()
        {
            if (owner.horizontalAxis == 0)
            {
                owner.ChangeState(PlayerFSM.PlayerFSMState.Idle);
            }

            if (owner.jumpButton)
            {
                owner.ChangeState(PlayerFSMState.Jumping);
            }

        }

        public override void DoBeforeEntering()
        {
            // update animator
            owner.animationController.isRunning = true;
        }

        public override void DoBeforeLeave()
        {
            
        }

        public override void UpdateState()
        {
            owner.Translate(owner.horizontalAxis);
        }
    }

    public class PlayerState_Jumping : PlayerState
    {
        public PlayerState_Jumping(PlayerFSM owner, PlayerFSM.PlayerFSMState state) : base(owner, state)
        {
        }

        public override void CheckTransition()
        {
            if (owner.isGrounded && owner.playerRigidbody.velocity.y < 0)
            {
                owner.ChangeState(PlayerFSMState.Idle);
            }
    }

        public override void DoBeforeEntering()
        {
            // update animator
            owner.animationController.isJumping = true;

            // execute action
            owner.Jump();

            owner.isJumping = true;

        }

        public override void DoBeforeLeave()
        {
            owner.animationController.isJumping = false;

            owner.isJumping = false;
        }

        public override void UpdateState()
        {
            owner.Translate(owner.horizontalAxis);
        }
    }
    #endregion
}






