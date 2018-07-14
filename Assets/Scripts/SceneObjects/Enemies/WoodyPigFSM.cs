using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WoodyPig))]
public class WoodyPigFSM : FSM<WoodyPig> {


    private void Awake()
    {
        Init(new System.Type[] { typeof(WoodyPig_Spawn),
            typeof(WoodyPig_Fly),
            typeof(WoodyPig_Turning)},
             GetComponent<WoodyPig>());
    }


    public class WoodyPig_Spawn : FSMState<WoodyPig>
    {
        public WoodyPig_Spawn(FSM<WoodyPig> fsm, WoodyPig fsm_holder) : base(fsm, fsm_holder)
        {
        }

        public override void CheckTransition()
        {
            if (fsm_holder.animator.GetCurrentAnimatorStateInfo(0).IsName("Fly"))
            {
                fsm.ChangeState(typeof(WoodyPig_Fly));
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

    public class WoodyPig_Fly : FSMState<WoodyPig>
    {
        public WoodyPig_Fly(FSM<WoodyPig> fsm, WoodyPig fsm_holder) : base(fsm, fsm_holder)
        {
        }

        public override void CheckTransition()
        {
            if (fsm_holder.CheckIfNearBounds() && CameraController.Instance.cameraBounds.Bounds.Contains(fsm_holder.transform.position))
            {
                fsm.ChangeState(typeof(WoodyPig_Turning));
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
            fsm_holder.Move();

        }
    }

    public class WoodyPig_Turning : FSMState<WoodyPig>
    {
        public WoodyPig_Turning(FSM<WoodyPig> fsm, WoodyPig fsm_holder) : base(fsm, fsm_holder)
        {
        }

        public override void CheckTransition()
        {
            if(fsm_holder.animator.GetCurrentAnimatorStateInfo(0).IsName("Fly"))
            {
                fsm.ChangeState(typeof(WoodyPig_Fly));
            }
        }

        public override void DoBeforeEntering()
        {
            fsm_holder.Turn();
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

}
