using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Zombie))]
public class ZombieFSM : FSM<Zombie>{



    private void Awake()
    {
        Init(new System.Type[] { typeof(Zombi_Emerging), typeof(Zombi_Walking), typeof(Zombi_Sinking) }, GetComponent<Zombie>());
    }

    class Zombi_Emerging : FSMState<Zombie>
    {
        private float timer = 0f;

        public Zombi_Emerging(FSM<Zombie> owner) : base(owner)
        {
        }

        public override void CheckTransition()
        {
            if (timer >= owner.holder.timeEmerging)
            {
                owner.ChangeState(typeof(Zombi_Walking));
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
            timer += Time.deltaTime;
        }
    }


    class Zombi_Walking : FSMState<Zombie>
    {
        private float timer = 0f;

        public Zombi_Walking(FSM<Zombie> owner) : base(owner)
        {
        }

        public override void CheckTransition()
        {
            if (timer >= owner.holder.timeWalking)
            {
                owner.ChangeState(typeof(Zombi_Sinking));
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
            timer += Time.deltaTime;


            owner.holder.Move();

        }
    }

    class Zombi_Sinking : FSMState<Zombie>
    {
        public Zombi_Sinking(FSM<Zombie> owner) : base(owner)
        {
        }

        private float timer = 0f;

        public override void CheckTransition()
        {
            if (timer >= owner.holder.timeSinking)
            {
                owner.holder.Destroy();
            }
        }

        public override void DoBeforeEntering()
        {

            timer = 0f;

            owner.holder.StartSink();
        }

        public override void DoBeforeLeave()
        {
            return;
        }

        public override void UpdateState()
        {
            timer += Time.deltaTime;
        }

    }

}
