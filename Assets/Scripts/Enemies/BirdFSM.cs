using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Bird))]
public class BirdFSM : FSM<Bird> {

    private void Awake()
    {
        Init(new System.Type[] { typeof(Bird_Sit), typeof(Bird_Flying) }, GetComponent<Bird>());
    }


    public class Bird_Sit : FSMState<Bird>
    {
        public Bird_Sit(FSM<Bird> fsm, Bird fsm_holder) : base(fsm, fsm_holder)
        {
        }

        public override void CheckTransition()
        {

            if (fsm_holder.isFlying)
            {
                fsm.ChangeState(typeof(Bird_Flying));
                return;
            }

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

    public class Bird_Flying : FSMState<Bird>
    {

        private float timer = 0f;

        public Bird_Flying(FSM<Bird> fsm, Bird fsm_holder) : base(fsm, fsm_holder)
        {
        }

        public override void CheckTransition()
        {
            return;
        }

        public override void DoBeforeEntering()
        {
            fsm_holder.animator.SetTrigger("Fly");
        }

        public override void DoBeforeLeave()
        {
            return;
        }

        public override void UpdateState()
        {
            timer += Time.deltaTime;

            if (timer > fsm_holder.waitBeforeStartMovingAfterStartFlying)
            {
                fsm_holder.Move();
            }
        }
    }

}
