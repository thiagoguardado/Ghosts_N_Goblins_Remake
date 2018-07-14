using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Unicorn))]
public class UnicornFSM : FSM<Unicorn> {

	private void Awake()
    {
        Init(new System.Type[] { typeof(Unicorn_Grounded),
        typeof(Unicorn_Jumping),
        typeof(Unicorn_Dash),
        typeof(Unicorn_JumpingDiagonal)},
             GetComponent<Unicorn>());
	}


    public class Unicorn_Grounded : FSMState<Unicorn>
    {

        private float timer = 0f;
        private bool alreadyShot = false;

        public Unicorn_Grounded(FSM<Unicorn> fsm, Unicorn fsm_holder) : base(fsm, fsm_holder)
        {
        }

        public override void CheckTransition()
        {
            // if player far away : jump to player or dash
            if (Mathf.Abs(PlayerController.Instance.transform.position.x - fsm_holder.transform.position.x) > fsm_holder.minDistanceToDahsAndJump)
            {
                if (Random.value < 0.5f)
                {
                    fsm.ChangeState(typeof(Unicorn_JumpingDiagonal));
                    return;
                }
                else {
                    fsm.ChangeState(typeof(Unicorn_Dash));
                    return;
                }
            }
            

            // jump or walk
            if (timer >= fsm_holder.timeGroundedBeforeJump)
            {
                fsm.ChangeState(typeof(Unicorn_Jumping));
                return;
            }

        }

        public override void DoBeforeEntering()
        {
            timer = 0f;
            alreadyShot = false;
        }

        public override void DoBeforeLeave()
        {
            return;
        }

        public override void UpdateState()
        {
            fsm_holder.Walk();

            timer += Time.deltaTime;

            if (!alreadyShot && timer >= fsm_holder.timeGroundedToShoot)
            {
                alreadyShot = true;

                if (Random.value < fsm_holder.shotProbability)
                {
                    fsm_holder.Shoot();
                }
            }

            fsm_holder.spriteDirection.TurnToPlayer();

        }
    }

    public class Unicorn_Jumping : FSMState<Unicorn>
    {
        private float timer = 0f;

        public Unicorn_Jumping(FSM<Unicorn> fsm, Unicorn fsm_holder) : base(fsm, fsm_holder)
        {
        }

        public override void CheckTransition()
        {
            if (fsm_holder.isGrounded && timer >= fsm_holder.minJumpDuration)
            {
                fsm_holder.Ground(fsm_holder.groundedCheck.groundindPoint);
                fsm.ChangeState(typeof(Unicorn_Grounded));
                return;
            }
        }

        public override void DoBeforeEntering()
        {
            timer = 0f;

            fsm_holder.animator.SetBool("isJumping", true);

            fsm_holder.Jump();
        }

        public override void DoBeforeLeave()
        {
            fsm_holder.animator.SetBool("isJumping", false);
        }

        public override void UpdateState()
        {
            timer += Time.deltaTime;

            fsm_holder.AddGravity();
        }
    }

    public class Unicorn_JumpingDiagonal : FSMState<Unicorn>
    {
        private float timer = 0f;

        public Unicorn_JumpingDiagonal(FSM<Unicorn> fsm, Unicorn fsm_holder) : base(fsm, fsm_holder)
        {
        }

        public override void CheckTransition()
        {
            if (fsm_holder.isGrounded && timer >= fsm_holder.minJumpDuration)
            {
                fsm_holder.Ground(fsm_holder.groundedCheck.groundindPoint);
                fsm.ChangeState(typeof(Unicorn_Grounded));
                return;
            }
        }

        public override void DoBeforeEntering()
        {
            timer = 0f;

            fsm_holder.animator.SetBool("isJumping", true);

            fsm_holder.JumpDiagonal(Mathf.Abs(PlayerController.Instance.transform.position.x - fsm_holder.transform.position.x));
        }

        public override void DoBeforeLeave()
        {
            fsm_holder.animator.SetBool("isJumping", false);
        }

        public override void UpdateState()
        {
            timer += Time.deltaTime;

            fsm_holder.AddGravity();
        }
    }

    public class Unicorn_Dash : FSMState<Unicorn>
    {
        float timer = 0f;

        public Unicorn_Dash(FSM<Unicorn> fsm, Unicorn fsm_holder) : base(fsm, fsm_holder)
        {
        }

        public override void CheckTransition()
        {
            if (fsm_holder.hitSomething || timer >= fsm_holder.dashMaxDuration)
            {
                fsm.ChangeState(typeof(Unicorn_Grounded));
                return;
            }
        }

        public override void DoBeforeEntering()
        {
            timer = 0f;

            fsm_holder.ResetHitSomethingFlag();
        }

        public override void DoBeforeLeave()
        {
            return;
        }

        public override void UpdateState()
        {
            fsm_holder.Dash();

            timer += Time.deltaTime;
        }
    }


}


