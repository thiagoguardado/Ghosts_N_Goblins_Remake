﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RedArremer))]
public class RedArremerFSM : FSM<RedArremer> {


    private void Awake()
    {
        Init(new System.Type[] { typeof(RedArremer_Sat), 
            typeof(RedArremer_PreparingToFly),
            typeof(RedArremer_Fly), 
            typeof(RedArremer_Grounded),
            typeof(RedArremer_Ascending),
            typeof(RedArremer_Descending),
            typeof(RedArremer_Walking),
        }, GetComponent<RedArremer>());
    }

    class RedArremer_Sat : FSMState<RedArremer>
    {
        public RedArremer_Sat(FSM<RedArremer> fsm, RedArremer fsm_holder) : base(fsm, fsm_holder)
        {
        }

        public override void CheckTransition()
        {
            if(fsm_holder.isFlying)
            {
                fsm.ChangeState(typeof(RedArremer_PreparingToFly));

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

    class RedArremer_PreparingToFly : FSMState<RedArremer>
    {

        private float timer = 0f;

        public RedArremer_PreparingToFly(FSM<RedArremer> fsm, RedArremer fsm_holder) : base(fsm, fsm_holder)
        {
        }

        public override void CheckTransition()
        {
            if(timer >= fsm_holder.timePreparingToFly)
            {
                fsm.ChangeState(typeof(RedArremer_Ascending));
            }
        }

        public override void DoBeforeEntering()
        {
            fsm_holder.animator.SetBool("isPreparingToFly", true);
            timer = 0f;
        }

        public override void DoBeforeLeave()
        {
            fsm_holder.animator.SetBool("isPreparingToFly", false);
        }

        public override void UpdateState()
        {
            timer += Time.deltaTime;
        }
    }

    class RedArremer_Ascending : FSMState<RedArremer>
    {

        private float initialHeight;
        private float timer = 0f;

        public RedArremer_Ascending(FSM<RedArremer> fsm, RedArremer fsm_holder) : base(fsm, fsm_holder)
        {
        }

        public override void CheckTransition()
        {
            if(fsm_holder.transform.position.y > (initialHeight + fsm_holder.maxHeightWhenAscending))
            {
                fsm.ChangeState(typeof(RedArremer_Fly));
            }
        }

        public override void DoBeforeEntering()
        {
            fsm_holder.animator.SetBool("isFlying", true);
            fsm_holder.animator.SetBool("isAscending", true);

            initialHeight = fsm_holder.transform.position.y;

            timer = 0f;
        }

        public override void DoBeforeLeave()
        {
            fsm_holder.animator.SetBool("isAscending", false);
            fsm_holder.animator.SetBool("isFlying", false);
        }

        public override void UpdateState()
        {
            fsm_holder.Ascend();
        }
    }

    class RedArremer_Fly : FSMState<RedArremer>
    {
        public RedArremer_Fly(FSM<RedArremer> fsm, RedArremer fsm_holder) : base(fsm, fsm_holder)
        {
        }

        public override void CheckTransition()
        {
            if(!fsm_holder.isFlying)
            {
                fsm.ChangeState(typeof(RedArremer_Descending));
            }
        }

        public override void DoBeforeEntering()
        {
            fsm_holder.animator.SetBool("isFlying", true);
        }

        public override void DoBeforeLeave()
        {
            fsm_holder.animator.SetBool("isFlying", false);
        }

        public override void UpdateState()
        {
            // fly
        }
    }

    class RedArremer_Descending : FSMState<RedArremer>
    {
        public RedArremer_Descending(FSM<RedArremer> fsm, RedArremer fsm_holder) : base(fsm, fsm_holder)
        {
        }

        public override void CheckTransition()
        {
            if(fsm_holder.isGrounded)
            {
                fsm.ChangeState(typeof(RedArremer_Grounded));
            }
        }

        public override void DoBeforeEntering()
        {
            fsm_holder.animator.SetBool("isFlying", false);
        }

        public override void DoBeforeLeave()
        {
            return;
        }

        public override void UpdateState()
        {
            fsm_holder.Descend();
        }
    }


    class RedArremer_Grounded : FSMState<RedArremer>
    {
        public RedArremer_Grounded(FSM<RedArremer> fsm, RedArremer fsm_holder) : base(fsm, fsm_holder)
        {
        }

        public override void CheckTransition()
        {
            if(fsm_holder.isWalking)
            {
                fsm.ChangeState(typeof(RedArremer_Walking));
            }

            if(fsm_holder.isFlying)
            {
                fsm.ChangeState(typeof(RedArremer_Ascending));
            }
        }

        public override void DoBeforeEntering()
        {
            fsm_holder.animator.SetBool("isGrounded", true);
        }

        public override void DoBeforeLeave()
        {
            fsm_holder.animator.SetBool("isGrounded", false);
        }

        public override void UpdateState()
        {
            return;
        }
    }

    class RedArremer_Walking : FSMState<RedArremer>
    {
        public RedArremer_Walking(FSM<RedArremer> fsm, RedArremer fsm_holder) : base(fsm, fsm_holder)
        {
        }

        public override void CheckTransition()
        {
            if (!fsm_holder.isWalking)
            {
                fsm.ChangeState(typeof(RedArremer_Grounded));
            }

            if (fsm_holder.isFlying)
            {
                fsm.ChangeState(typeof(RedArremer_Ascending));
            }
        }

        public override void DoBeforeEntering()
        {
            fsm_holder.animator.SetBool("isGrounded", true);
            fsm_holder.animator.SetBool("isWalking", true);
        }

        public override void DoBeforeLeave()
        {
            fsm_holder.animator.SetBool("isGrounded", false);
            fsm_holder.animator.SetBool("isWalking", false);
        }

        public override void UpdateState()
        {
            return;
        }
    }

}
