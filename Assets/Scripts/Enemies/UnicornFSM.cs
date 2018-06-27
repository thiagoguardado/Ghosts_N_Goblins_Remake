using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Unicorn))]
public class UnicornFSM : FSM<Unicorn> {

	private void Awake()
    {
        Init(new System.Type[] { typeof(Unicorn_Grounded),
        typeof(Unicorn_Jumping)}, 
             GetComponent<Unicorn>());
	}


    public class Unicorn_Grounded : FSMState<Unicorn>
    {

        private float timer = 0f;

        public Unicorn_Grounded(FSM<Unicorn> fsm, Unicorn fsm_holder) : base(fsm, fsm_holder)
        {
        }

        public override void CheckTransition()
        {
            if (timer >= fsm_holder.timeGroundedBeforeJump)
            {
                fsm.ChangeState(typeof(Unicorn_Jumping));
            }
        }

        public override void DoBeforeEntering()
        {
            timer = 0f;
        }

        public override void DoBeforeLeave()
        {
            return;
        }

        public override void UpdateState()
        {
            fsm_holder.Walk();

            timer += Time.deltaTime;
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
                fsm.ChangeState(typeof(Unicorn_Grounded));
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
        }
    }

}


