using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(PlayerAnimationController))]
[RequireComponent(typeof(Rigidbody2D))]
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

    // input
    private float horizontalAxis
    {
        get
        {
            return playerController.HorizontalAxis;
        }
    }
    private float verticalAxis
    {
        get
        {
            return playerController.VerticalAxis;
        }
    }
    private bool isGrounded
    {
        get
        {
            return playerController.IsGrounded;
        }
    }
    private bool jumpButton
    {
        get
        {
            return playerController.JumpButton;
        }
    }
    private bool throwButton
    {
        get
        {
            return playerController.ThrowButton;
        }
    }

    [Header("References")]
    private PlayerController playerController;
    private PlayerAnimationController animationController;
    private Rigidbody2D playerRigidbody;

    [Header("FSM")]
    private PlayerState currentPlayerState;
    public Dictionary<PlayerFSMState, PlayerState> statesDictionary = new Dictionary<PlayerFSMState, PlayerState>();

    [Header("Horizontal Translation")]
    public Transform spriteTransform;
    public float horizontalSpeed = 5;
    private bool isRunning;
    private Direction currentDirection = Direction.Right;
    private Vector3 currentWorldDirection
    {
        get
        {
            return currentDirection == Direction.Right ? Vector3.right : Vector3.left;
        }
    }
    private bool walkPaused = false;

    [Header("Vertical Translation")]
    public float verticalSpeed = 5;


    [Header("Crouching")]
    public float waitToExitCrouched = 0.5f;
    public Collider2D standingCollider;
    public Collider2D crouchedCollider;
    private bool isCrouched = false;

    [Header("Jumping")]
    public float jumpForce = 1f;
    public float minTimeToExitJump = 0.02f;
    private bool isJumping;


    private void Awake()
    {

        // get references
        playerController = GetComponent<PlayerController>();
        animationController = GetComponent<PlayerAnimationController>();
        playerRigidbody = GetComponent<Rigidbody2D>();


        // Create FSM States
        statesDictionary.Add(PlayerFSMState.Idle, new PlayerState_Idle(this, PlayerFSMState.Idle));
        statesDictionary.Add(PlayerFSMState.Running, new PlayerState_Running(this, PlayerFSMState.Running));
        statesDictionary.Add(PlayerFSMState.Jumping, new PlayerState_Jumping(this, PlayerFSMState.Jumping));
        statesDictionary.Add(PlayerFSMState.Crouched, new PlayerState_Crouched(this, PlayerFSMState.Crouched));

        // assign initial state
        currentPlayerState = statesDictionary[PlayerFSMState.Idle];

    }


    private void LateUpdate()
    {

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





    // walk
    private void Translate(float horizontalAxis)
    {

        if (!walkPaused)
        {
            // translate
            transform.Translate(Vector3.right * horizontalAxis * horizontalSpeed * Time.deltaTime);
            
        }


    }

    // Check Turn Direction
    private void CheckDirection()
    {
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


    // Jump
    private void Jump()
    {
        playerRigidbody.AddForce(Vector3.up * jumpForce); 
        isJumping = true;
    }

    // Throw
    private void Throw(bool isCrouched)
    {
        // make controller shoot
        if (isCrouched)
        {
            playerController.ShootCrouched(currentWorldDirection);
        }
        else
        {
            playerController.ShootStanding(currentWorldDirection);
        } 

        // update animator
        animationController.throwSomething = true;
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

            //check throw
            if (owner.throwButton)
            {
                owner.Throw(false);

            }
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

            if (owner.verticalAxis < 0f)
            {
                owner.ChangeState(PlayerFSMState.Crouched);
            }


        }

        public override void DoBeforeLeave()
        {
            return;
        }

        public override void DoBeforeEntering()
        {
            // set parameter
            owner.isRunning = false;

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

            if (owner.verticalAxis < 0f)
            {
                owner.ChangeState(PlayerFSMState.Crouched);
            }

        }

        public override void DoBeforeEntering()
        {

            // set parameter
            owner.isRunning = true;

            // update animator
            owner.animationController.isRunning = true;
        }

        public override void DoBeforeLeave()
        {
            
        }

        public override void UpdateState()
        {
            // move and change direction
            owner.Translate(owner.horizontalAxis);
            owner.CheckDirection();

            //check throw
            if (owner.throwButton)
            {
                owner.Throw(false);

            }
        }
    }

    public class PlayerState_Jumping : PlayerState
    {

        private float timer = 0f;
        private bool wasRunning;
        private float runningHorizontalAxisValue;

        public PlayerState_Jumping(PlayerFSM owner, PlayerFSM.PlayerFSMState state) : base(owner, state)
        {
        }

        public override void CheckTransition()
        {
            if (timer >= owner.minTimeToExitJump)
            {
                if ((owner.isGrounded && owner.playerRigidbody.velocity.y <= 0) || owner.playerRigidbody.velocity.y == 0)
                {
                    owner.ChangeState(PlayerFSMState.Idle);
                }
            }
    }

        public override void DoBeforeEntering()
        {
            // update animator
            owner.animationController.isJumping = true;

            // execute action
            owner.Jump();

            // save horizontal axis value to use if jump running
            runningHorizontalAxisValue = owner.horizontalAxis;
            wasRunning = owner.isRunning;
            timer = 0f;
        }

        public override void DoBeforeLeave()
        {
            owner.animationController.isJumping = false;

            owner.isJumping = false;
        }

        public override void UpdateState()
        {
            // increment timer
            timer += Time.deltaTime;

            // move and change direction
            if (owner.isRunning)
            {
                owner.Translate(runningHorizontalAxisValue);
            }
            owner.CheckDirection();


            //check throw
            if (owner.throwButton)
            {
                owner.Throw(false);

            }
        }
    }


    public class PlayerState_Crouched : PlayerState
    {

        private bool isCountingToLeave;
        private float notHoldingDownTimer;

        public PlayerState_Crouched(PlayerFSM owner, PlayerFSMState state) : base(owner, state)
        {
        }

        public override void CheckTransition()
        {
            if (notHoldingDownTimer >= owner.waitToExitCrouched)
            {
                owner.ChangeState(PlayerFSMState.Idle);
            }


            if (owner.verticalAxis >= 0 && owner.horizontalAxis != 0)
            {
                owner.ChangeState(PlayerFSMState.Running);
            }
        }

        public override void DoBeforeEntering()
        {
            //set parameter
            owner.isCrouched = true;
            owner.isRunning = false;

            // update controller
            owner.animationController.isCrouched = true;
            owner.animationController.isRunning = false;

            // set timer
            notHoldingDownTimer = 0f;

            // change collliders
            owner.standingCollider.enabled = false;
            owner.crouchedCollider.enabled = true;

        }

        public override void DoBeforeLeave()
        {
            // update animator
            owner.animationController.isCrouched = false;


            // change collliders
            owner.standingCollider.enabled = true;
            owner.crouchedCollider.enabled = false;

        }

        public override void UpdateState()
        {
            UpdateTimer();

            //check throw
            if (owner.throwButton)
            {
                owner.Throw(true);

            }

        }


        private void UpdateTimer()
        {
            if (owner.verticalAxis >= 0)
            {
                if (!isCountingToLeave)
                {
                    isCountingToLeave = true;
                    notHoldingDownTimer = 0f;
                }
                else
                {
                    notHoldingDownTimer += Time.deltaTime;
                }
            }
            else
            {
                isCountingToLeave = false;
            }
        }
    }



    #endregion
}






