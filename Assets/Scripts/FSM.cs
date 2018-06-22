using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class FSM<T> : MonoBehaviour where T : MonoBehaviour
{

    protected FSMState<T> currentState;

    public Dictionary<Type, FSMState<T>> statesList = new Dictionary<Type, FSMState<T>>();

   
    public void Init(Type[] types, T holder)
    {

        for (int i = 0; i < types.Length; i++)
        {

            if (!types[i].IsSubclassOf(typeof(FSMState<T>)))
            {
                Debug.LogError("Could not create FSM beacause types in constructor not allowed.");
                return;
            }


            var obj = Activator.CreateInstance(types[i], new object[] {this, holder} );
            statesList.Add(types[i],  obj as FSMState<T>);
        }

        currentState = statesList[types[0]];
        currentState.DoBeforeEntering();

    }

    private void Update()
    {
        currentState.UpdateState();
        currentState.CheckTransition();

    }


    public void ChangeState(Type newState)
    {

        if (!newState.IsSubclassOf(typeof(FSMState<T>)))
        {
            Debug.LogError("Trying to transition to incorrect type state");
            return;
        }

        if (statesList.ContainsKey(newState))
        {
            currentState.DoBeforeLeave();
            currentState = statesList[newState];
            currentState.DoBeforeEntering();
        }
        else
        {
            Debug.LogError("There is no state " + newState.ToString() + " in dictionary");
        }

    }

}


public abstract class FSMState<T> where T : MonoBehaviour
{

    protected FSM<T> fsm;
    protected T fsm_holder;

    public FSMState(FSM<T> fsm, T fsm_holder)
    {
        this.fsm = fsm;
        this.fsm_holder = fsm_holder;
    }

    public abstract void UpdateState();
    public abstract void CheckTransition();
    public abstract void DoBeforeLeave();
    public abstract void DoBeforeEntering();




}

