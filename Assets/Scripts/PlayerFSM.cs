﻿using System;
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
        CrouchedAttack,
        Died
    }

    private enum LookingDirection
    {
        Left,
        Right
    }

    private enum TranslatingAxis
    {
        Horizontal,
        Vertical
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
    private float spriteObjectStartYPosition;

    [Header("FSM")]
    private PlayerState currentPlayerState;
    public Dictionary<PlayerFSMState, PlayerState> statesDictionary = new Dictionary<PlayerFSMState, PlayerState>();

    [Header("Horizontal Translation")]
    public Transform spriteTransform;
    public float horizontalSpeed = 5;
    private bool isRunning;
    private LookingDirection currentDirection = LookingDirection.Right;
    private Vector3 currentWorldDirection
    {
        get
        {
            return currentDirection == LookingDirection.Right ? Vector3.right : Vector3.left;
        }
    }
    private bool walkPaused = false;
    private bool isTranslating = false;
    private float fixedMoveSpeed;
    private TranslatingAxis fixedMoveAxis;

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

    [Header("ClimbingLadder")]
    public float flippingInterval;
    private bool isInsideLadder = false;
    private Ladder currentLadder;

    [Header("Die")]
    public Vector3 dieForceDirection;
    public float dieForce;
    public float timeBeforeDismount = 1f;
    private bool isDead
    {
        get
        {
            return playerController.currentArmorStatus == PlayerController.PlayerArmor.Dead;
        }
    }

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
        statesDictionary.Add(PlayerFSMState.Climbing, new PlayerState_Climbing(this, PlayerFSMState.Climbing));
        statesDictionary.Add(PlayerFSMState.Died, new PlayerState_Died(this, PlayerFSMState.Died));

        // assign initial state
        currentPlayerState = statesDictionary[PlayerFSMState.Idle];

        // save start y position of sprite object
        spriteObjectStartYPosition = spriteTransform.localPosition.y;

    }


    private void Update()
    {

        // update fsm state
        currentPlayerState.UpdateState();
        currentPlayerState.CheckTransition();

    }

    private void FixedUpdate()
    {
        // move horizontally
        FixedTranslate();
    }


    private void OnTriggerEnter2D(Collider2D collider)
    {
        // check if enter ladder
        Ladder ladder = collider.GetComponent<Ladder>();
        if (ladder != null)
        {
            isInsideLadder = true;
            currentLadder = ladder;
        }

    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        // check if enter ladder
        Ladder ladder = collider.GetComponent<Ladder>();
        if (ladder != null)
        {
            isInsideLadder = false;
            currentLadder = null;
        }

    }


    private void FixedTranslate()
    {
        if (isTranslating)
        {
            transform.Translate((fixedMoveAxis==TranslatingAxis.Horizontal? Vector3.right:Vector3.up) * fixedMoveSpeed * Time.fixedDeltaTime);
            isTranslating = false;
        }
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
    private void TranslateIntention(TranslatingAxis axis, float speed)
    {

        if (!walkPaused)
        {
            // translate
            isTranslating = true;
            fixedMoveSpeed = speed;
            fixedMoveAxis = axis;
            return;
        }

        isTranslating = false;
        fixedMoveSpeed = 0f;

    }

    // Check Turn Direction
    private void CheckDirection()
    {
        // turn
        if (horizontalAxis > 0 && currentDirection == LookingDirection.Left)
        {
            // turn to right
            Vector3 scale = spriteTransform.localScale;
            scale.x = 1;
            spriteTransform.localScale = scale;
            currentDirection = LookingDirection.Right;

        }
        else if (horizontalAxis < 0 && currentDirection == LookingDirection.Right)
        {
            // turn to left
            Vector3 scale = spriteTransform.localScale;
            scale.x = -1;
            spriteTransform.localScale = scale;
            currentDirection = LookingDirection.Left;
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


    // collider
    private void ChangeColliders(bool standingCollider, bool crouchedCollider)
    {
        this.standingCollider.enabled = standingCollider;
        this.crouchedCollider.enabled = crouchedCollider;
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

            // check if is dead
            if (owner.isDead)
            {
                owner.ChangeState(PlayerFSMState.Died);
            }

            // if run
            if (owner.horizontalAxis != 0)
            {
                owner.ChangeState(PlayerFSM.PlayerFSMState.Running);
                return;
            }

            // if jump
            if (owner.jumpButton)
            {
                owner.ChangeState(PlayerFSMState.Jumping);
                return;
            }

            // if start climbing ladder
            if (owner.isInsideLadder)
            {
                
                if (owner.verticalAxis < 0f)    // climbin down
                {
                    if (owner.transform.position.y >= owner.currentLadder.startEndOfLadder.position.y)
                    {
                        owner.ChangeState(PlayerFSMState.Climbing);
                        return;
                    }
                }
                else if (owner.verticalAxis > 0f)   // climbing up
                {
                    if (owner.transform.position.y <= owner.currentLadder.startEndOfLadder.position.y)
                    {
                        owner.ChangeState(PlayerFSMState.Climbing);
                        return;
                    }
                }
            }

            // if crouch
            if (owner.verticalAxis < 0f && owner.isGrounded)
            {
                owner.ChangeState(PlayerFSMState.Crouched);
                return;
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

            // check if is dead
            if (owner.isDead)
            {
                owner.ChangeState(PlayerFSMState.Died);
            }

            // if stop walking
            if (owner.horizontalAxis == 0)
            {
                owner.ChangeState(PlayerFSM.PlayerFSMState.Idle);
                return;
            }

            // if jump
            if (owner.jumpButton)
            {
                owner.ChangeState(PlayerFSMState.Jumping);
                return;
            }

            // if climb ladder
            if (owner.isInsideLadder)
            {
                if (owner.verticalAxis > 0f)    // climbing up
                {
                    if (owner.transform.position.y <= owner.currentLadder.startEndOfLadder.position.y)
                    {
                        owner.ChangeState(PlayerFSMState.Climbing);
                        return;
                    }
                }
                else if (owner.verticalAxis < 0f)   // climbing down
                {
                    if (owner.transform.position.y >= owner.currentLadder.startEndOfLadder.position.y)
                    {
                        owner.ChangeState(PlayerFSMState.Climbing);
                        return;
                    }
                }
            }

            // if crouch
            if (owner.verticalAxis < 0f && owner.isGrounded)
            {
                owner.ChangeState(PlayerFSMState.Crouched);
                return;
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
            owner.TranslateIntention(TranslatingAxis.Horizontal, owner.horizontalAxis * owner.horizontalSpeed);
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

            // check if is dead
            if (owner.isDead)
            {
                owner.ChangeState(PlayerFSMState.Died);
            }

            // check if touches ground (consider a minimum time to not leave immediately
            if (timer >= owner.minTimeToExitJump)
            {
                if ((owner.isGrounded && owner.playerRigidbody.velocity.y <= 0) || owner.playerRigidbody.velocity.y == 0)
                {
                    owner.ChangeState(PlayerFSMState.Idle);
                    return;
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
                owner.TranslateIntention(TranslatingAxis.Horizontal, runningHorizontalAxisValue * owner.horizontalSpeed);
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

            // check if is dead
            if (owner.isDead)
            {
                owner.ChangeState(PlayerFSMState.Died);
            }

            // if get up
            if (notHoldingDownTimer >= owner.waitToExitCrouched)
            {
                owner.ChangeState(PlayerFSMState.Idle);
                return;
            }

            // if start running
            if (owner.verticalAxis >= 0 && owner.horizontalAxis != 0)
            {
                owner.ChangeState(PlayerFSMState.Running);
                return;
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
            owner.ChangeColliders(false,true);

        }



        public override void DoBeforeLeave()
        {
            // update animator
            owner.animationController.isCrouched = false;


            // change collliders
            owner.ChangeColliders(true, false);
          

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


    public class PlayerState_Climbing : PlayerState
    {

        private float groundY;
        private Ladder ladder;
        private bool isOnEndOfLadder;
        private bool isLeavingLadder;
        private Vector3 enterStateSpriteScale;

        public PlayerState_Climbing(PlayerFSM owner, PlayerFSMState state) : base(owner, state)
        {
            
        }

        public override void CheckTransition()
        {

            // check if is dead
            if (owner.isDead)
            {
                owner.ChangeState(PlayerFSMState.Died);
            }

            // reached top of ladder
            if (CheckTopOfLadder())
            {
                owner.transform.position = ladder.finishOfLadder.position;
                owner.spriteTransform.localPosition = new Vector3(owner.spriteTransform.localPosition.x, owner.spriteObjectStartYPosition, owner.spriteTransform.localPosition.z);
                owner.animationController.FinishClimbingLadder(true);
                owner.ChangeState(PlayerFSMState.Idle);
                return;
            }

            // rechaed bottom
            if (owner.transform.position.y <= ladder.baseOfLadder.position.y && owner.verticalAxis < 0f)
            {
                owner.spriteTransform.localPosition = new Vector3(owner.spriteTransform.localPosition.x, owner.spriteObjectStartYPosition, owner.spriteTransform.localPosition.z);
                owner.animationController.FinishClimbingLadder(false);
                owner.ChangeState(PlayerFSMState.Idle);
                owner.playerController.ResetAxis();
                return;

            }

        }



        public override void DoBeforeEntering()
        {
            // save sprite direction
            enterStateSpriteScale = owner.spriteTransform.localScale;

            // set ladder and base height
            this.ladder = owner.currentLadder;
            groundY = ladder.baseOfLadder.position.y;

            // deactivate gravity
            owner.playerRigidbody.gravityScale = 0;

            // turn off ladder upperfloorCollision
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Floor"), true);

            // update position on ladder
            CheckPositionOnLadder();


            // align player on center
            owner.transform.position = new Vector3(ladder.transform.position.x, owner.transform.position.y, owner.transform.position.z);

            // change animator
            owner.animationController.StartClimbingLadder(isOnEndOfLadder);
        }

        public override void DoBeforeLeave()
        {

            // reactivate gravity
            owner.playerRigidbody.gravityScale = 1;

            // reset sprite object height
            owner.spriteTransform.localPosition = new Vector3(owner.spriteTransform.localPosition.x, owner.spriteObjectStartYPosition, owner.spriteTransform.localPosition.z);

            // turn on ladder upperfloorCollision
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Floor"), false);

            //change colliders
            owner.ChangeColliders(true, false);

            // set sprite direction
            owner.spriteTransform.localScale = enterStateSpriteScale;
        }

        public override void UpdateState()
        {
            CheckPositionOnLadder();

            if (!isOnEndOfLadder)
                CheckSpriteFlipping();

            owner.TranslateIntention(TranslatingAxis.Vertical, owner.verticalAxis * owner.verticalSpeed);

        }

        private void CheckPositionOnLadder()
        {
            if (owner.transform.position.y > ladder.startEndOfLadder.position.y)
            {

                if (owner.transform.position.y > ladder.startLeavingLadder.position.y)
                {
                    isLeavingLadder = true;
                    isOnEndOfLadder = true;
                }
                else
                {
                    isOnEndOfLadder = true;
                    isLeavingLadder = false;
                }

                // update position
                owner.spriteTransform.position = ladder.finishOfLadder.position;

                //change colliders
                owner.ChangeColliders(false, true);

            }
            else
            {

                isOnEndOfLadder = false;
                isLeavingLadder = false;

                owner.spriteTransform.localPosition = new Vector3(owner.spriteTransform.localPosition.x, owner.spriteObjectStartYPosition, owner.spriteTransform.localPosition.z);

                //change colliders
                owner.ChangeColliders(true, false);

            }

            // uodate animator
            owner.animationController.isOnEndOfLadder = isOnEndOfLadder;
            owner.animationController.isLeavingLadder = isLeavingLadder;

        }

        private void CheckSpriteFlipping()
        {
            Debug.Log(owner.transform.position.y - groundY);

            if (((owner.transform.position.y - groundY) / owner.flippingInterval) % 2 > 1)
            {
                owner.spriteTransform.localScale = new Vector3(1, 1, 1);
            } else {
                owner.spriteTransform.localScale = new Vector3(-1, 1, 1);
            }
        }

        private bool CheckTopOfLadder()
        {
            return owner.transform.position.y > ladder.finishOfLadder.position.y + 0.015f;
        }
    }


    public class PlayerState_Died : PlayerState
    {

        public PlayerState_Died(PlayerFSM owner, PlayerFSMState state) : base(owner, state)
        {
        }

        public override void CheckTransition()
        {
            return;
        }

        public override void DoBeforeEntering()
        {
            // change animator
            owner.animationController.TriggerDie();

            owner.playerRigidbody.AddForce(owner.dieForceDirection.normalized * owner.dieForce);

            // wait and change animator
            owner.WaitAndAct(owner.timeBeforeDismount, () => owner.animationController.TriggerDismount());
        }

        public override void DoBeforeLeave()
        {
            throw new NotImplementedException();
        }

        public override void UpdateState()
        {
            return;
        }


    }

    #endregion
}






