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
        CrouchedAttack,
        Hit,
        Died,
        FrogIdle,
        FrogRunning,
        FrogJumping
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
    private bool isFrog
    {
        get
        {
            return playerController.IsFrog;
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
    
    private Vector3 currentWorldDirection
    {
        get
        {
            return playerController.spriteDirection.WorldLookingDirection;
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

    [Header("Hit")]
    public Vector2 hitForceDirection;
    public float hitForce;
    private float hitXDirection;

    [Header("Frog")]
    public Collider2D frogCollider;

    private bool isDead
    {
        get
        {
            return playerController.currentArmorStatus == PlayerController.PlayerArmor.Dead;
        }
    }
    private float minTimeToExitHit { get { return minTimeToExitJump; } }

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
        statesDictionary.Add(PlayerFSMState.Hit, new PlayerState_Hit(this, PlayerFSMState.Hit));
        statesDictionary.Add(PlayerFSMState.Died, new PlayerState_Died(this, PlayerFSMState.Died));
        statesDictionary.Add(PlayerFSMState.FrogIdle, new PlayerState_FrogIdle(this, PlayerFSMState.FrogIdle));
        statesDictionary.Add(PlayerFSMState.FrogRunning, new PlayerState_FrogRunning(this, PlayerFSMState.FrogRunning));
        statesDictionary.Add(PlayerFSMState.FrogJumping, new PlayerState_FrogJumping(this, PlayerFSMState.FrogJumping));

        // assign initial state
        currentPlayerState = statesDictionary[PlayerFSMState.Idle];

        // save start y position of sprite object
        spriteObjectStartYPosition = spriteTransform.localPosition.y;


    }

    private void OnEnable()
    {
        // add listener to hit state
        GameEvents.Player.PlayerPushed += GetHit;

    }

    private void OnDisable()
    {
        // add listener to hit state
        GameEvents.Player.PlayerPushed -= GetHit;
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
        if (isTranslating && !playerController.movingPaused)
        {
            transform.localPosition += (fixedMoveAxis == TranslatingAxis.Horizontal ? Vector3.right : Vector3.up) * fixedMoveSpeed * Time.fixedDeltaTime;
            //transform.Translate((fixedMoveAxis==TranslatingAxis.Horizontal? Vector3.right:Vector3.up) * fixedMoveSpeed * Time.fixedDeltaTime);
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
        if (horizontalAxis > 0 && playerController.spriteDirection.lookingDirection == LookingDirection.Left)
        {
            // turn to left
            playerController.spriteDirection.FlipDirectionY();
        }
        else if (horizontalAxis < 0 && playerController.spriteDirection.lookingDirection == LookingDirection.Right)
        {
            // turn to left
            playerController.spriteDirection.FlipDirectionY();
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
        bool shot = isCrouched ? playerController.ShootCrouched() : playerController.ShootStanding();

        // update animator
        if (shot)
            animationController.throwSomething = true;
    }

    // hit
    private void GetHit(float xDirection)
    {
        hitXDirection = xDirection;
        ChangeState(PlayerFSMState.Hit);
    }


    // collider
    private void ChangeColliders(bool standingCollider, bool crouchedCollider, bool frogCollider)
    {
        this.standingCollider.enabled = standingCollider;
        this.crouchedCollider.enabled = crouchedCollider;
        this.frogCollider.enabled = frogCollider;
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

            // check if was hit
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

            // if frog
            if (owner.isFrog)
            {
                owner.ChangeState(PlayerFSMState.FrogIdle);
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

            // set running parameter
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
                owner.ChangeState(PlayerFSMState.Idle);
                return;
            }

            // if jump
            if (owner.jumpButton)
            {
                owner.ChangeState(PlayerFSMState.Jumping);
                return;
            }

            // if frog
            if (owner.isFrog)
            {
                owner.ChangeState(PlayerFSMState.FrogIdle);
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
            return;
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
            
            // if frog
            if (owner.isFrog)
            {
                owner.ChangeState(PlayerFSMState.FrogIdle);
                return;
            }

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
                    // notify event
                    GameEvents.Player.PlayerLanded.SafeCall();

                    // transition
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
            timer = 0f;

            // notify event
            GameEvents.Player.PlayerJumped.SafeCall();
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
                return;
            }

            // if frog
            if (owner.isFrog)
            {
                owner.ChangeState(PlayerFSMState.FrogIdle);
                return;
            }

            // if get up
            if (notHoldingDownTimer >= owner.waitToExitCrouched)
            {
                owner.ChangeState(PlayerFSMState.Idle);
                return;
            }

            //// if start running
            //if (owner.verticalAxis >= 0 && owner.horizontalAxis != 0)
            //{
            //    owner.ChangeState(PlayerFSMState.Running);
            //    return;
            //}
        }

        public override void DoBeforeEntering()
        {
            //set parameter
            owner.isCrouched = true;

            // update controller
            owner.animationController.isCrouched = true;

            // set timer
            notHoldingDownTimer = 0f;

            // change collliders
            owner.ChangeColliders(false,true,false);

            // set running parameter
            owner.isRunning = false;
            // update animator
            owner.animationController.isRunning = false;

        }



        public override void DoBeforeLeave()
        {
            // update animator
            owner.animationController.isCrouched = false;


            // change collliders
            owner.ChangeColliders(true, false, false);
          

        }

        public override void UpdateState()
        {
            UpdateTimer();

            //check throw
            if (owner.throwButton)
            {
                owner.Throw(true);
                notHoldingDownTimer = 0f; // resets timer

            }

            owner.CheckDirection();

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
                return;
            }

            // if frog
            if (owner.isFrog)
            {
                owner.ChangeState(PlayerFSMState.FrogIdle);
                return;
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

            // reset rigidbody velocity
            owner.playerRigidbody.velocity = Vector2.zero;

            // turn off ladder upperfloorCollision
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Floor"), true);

            // update position on ladder
            CheckPositionOnLadder();

            // align player on center
            owner.transform.position = new Vector3(ladder.transform.position.x, owner.transform.position.y, owner.transform.position.z);

            // change animator
            owner.animationController.StartClimbingLadder(isOnEndOfLadder);

            // set running parameter
            owner.isRunning = false;
            // update animator
            owner.animationController.isRunning = false;

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
            owner.ChangeColliders(true, false, false);

            // set sprite direction
            owner.spriteTransform.localScale = enterStateSpriteScale;

            // change animator
            owner.animationController.ResetClimbingAnimationVariables();
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
                owner.ChangeColliders(false, true, false);

            }
            else
            {

                isOnEndOfLadder = false;
                isLeavingLadder = false;

                owner.spriteTransform.localPosition = new Vector3(owner.spriteTransform.localPosition.x, owner.spriteObjectStartYPosition, owner.spriteTransform.localPosition.z);

                //change colliders
                owner.ChangeColliders(true, false, false);

            }

            // uodate animator
            owner.animationController.isOnEndOfLadder = isOnEndOfLadder;
            owner.animationController.isLeavingLadder = isLeavingLadder;

        }

        private void CheckSpriteFlipping()
        {

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

    public class PlayerState_Hit : PlayerState
    {

        private float timer = 0f;

        public PlayerState_Hit(PlayerFSM owner, PlayerFSMState state) : base(owner, state)
        {
        }

        public override void CheckTransition()
        {

            if (owner.isGrounded && timer >= owner.minTimeToExitHit)
            {
                if (owner.isDead)
                {
                    owner.ChangeState(PlayerFSMState.Died);
                }
                else
                {
                    owner.ChangeState(PlayerFSMState.Idle);
                }

            }

        }

        public override void DoBeforeEntering()
        {
            // set running parameter
            owner.isRunning = false;
            // update animator
            owner.animationController.isRunning = false;

            // change animator
            owner.animationController.TriggerHit();

            // apply force
            owner.playerRigidbody.velocity = Vector2.zero;

            owner.playerRigidbody.AddForce(new Vector3(owner.hitXDirection * owner.hitForceDirection.normalized.x, owner.hitForceDirection.normalized.y, 0) * owner.hitForce,ForceMode2D.Impulse);

            // set timer
            timer = 0f;
        }

        public override void DoBeforeLeave()
        {
            // change animator
            owner.animationController.LeaveHitState();
            return;
        }

        public override void UpdateState()
        {
            timer += Time.deltaTime;
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
            return;
        }

        public override void DoBeforeLeave()
        {
            return;
        }

        public override void UpdateState()
        {
            return;
        }


    }

    public class PlayerState_FrogIdle : PlayerState
    {
        public PlayerState_FrogIdle(PlayerFSM owner, PlayerFSMState state) : base(owner, state)
        {
        }

        public override void CheckTransition()
        {
            // if run
            if (owner.horizontalAxis != 0)
            {
                owner.ChangeState(PlayerFSMState.FrogRunning);
                return;
            }

            // if leaves frog
            if (!owner.isFrog)
            {
                owner.ChangeState(PlayerFSMState.Idle);
                return;
            }

            // if jump
            if (owner.jumpButton)
            {
                owner.ChangeState(PlayerFSMState.FrogJumping);
                return;
            }

        }

        public override void DoBeforeEntering()
        {
            // set running parameter
            owner.isRunning = false;
            // update animator
            owner.animationController.isRunning = false;

            // change animator
            owner.animationController.ChangeBetweenHumanAndFrog(false);

            // change coliders
            owner.ChangeColliders(false, false, true);
        }

        public override void DoBeforeLeave()
        {
            // change animator
            owner.animationController.ChangeBetweenHumanAndFrog(true);

            // change coliders
            owner.ChangeColliders(true, false, false);
        }

        public override void UpdateState()
        {
            return;
        }
    }

    public class PlayerState_FrogRunning : PlayerState
    {
        public PlayerState_FrogRunning(PlayerFSM owner, PlayerFSMState state) : base(owner, state)
        {
        }

        public override void CheckTransition()
        {
            // if stop walking
            if (owner.horizontalAxis == 0)
            {
                owner.ChangeState(PlayerFSMState.Idle);
                return;
            }

            // if leaves frog
            if (!owner.isFrog)
            {
                owner.ChangeState(PlayerFSMState.Idle);
                return;
            }

            // if jump
            if (owner.jumpButton)
            {
                owner.ChangeState(PlayerFSMState.FrogJumping);
                return;
            }
        }

        public override void DoBeforeEntering()
        {

            // set running parameter
            owner.isRunning = true;
            // update animator
            owner.animationController.isRunning = true;
        }

        public override void DoBeforeLeave()
        {
            return;
        }

        public override void UpdateState()
        {
            // move and change direction
            owner.TranslateIntention(TranslatingAxis.Horizontal, owner.horizontalAxis * owner.horizontalSpeed);
            owner.CheckDirection();
        }
    }

    public class PlayerState_FrogJumping : PlayerState
    {

        private float timer = 0f;
        private float runningHorizontalAxisValue;

        public PlayerState_FrogJumping(PlayerFSM owner, PlayerFSMState state) : base(owner, state)
        {
        }

        public override void CheckTransition()
        {
            // if leaves frog
            if (!owner.isFrog)
            {
                owner.ChangeState(PlayerFSM.PlayerFSMState.Idle);
                return;
            }

            // check if touches ground (consider a minimum time to not leave immediately
            if (timer >= owner.minTimeToExitJump)
            {
                if ((owner.isGrounded && owner.playerRigidbody.velocity.y <= 0) || owner.playerRigidbody.velocity.y == 0)
                {
                    // notify event
                    GameEvents.Player.PlayerLanded.SafeCall();

                    // transition
                    owner.ChangeState(PlayerFSMState.FrogIdle);
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
            timer = 0f;

            // notify event
            GameEvents.Player.PlayerJumped.SafeCall();
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
            
        }
    }

    #endregion
}






