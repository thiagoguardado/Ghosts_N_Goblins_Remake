using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WoodyPig))]
public class WoodyPigFSM : FSM<WoodyPig> {


    private void Awake()
    {
        Init(new System.Type[] { typeof(WoodyPig_Fly),
            typeof(WoodyPig_Turning)},
             GetComponent<WoodyPig>());
    }


    public class WoodyPig_Fly : FSMState<WoodyPig>
    {
        public WoodyPig_Fly(FSM<WoodyPig> fsm, WoodyPig fsm_holder) : base(fsm, fsm_holder)
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

}
