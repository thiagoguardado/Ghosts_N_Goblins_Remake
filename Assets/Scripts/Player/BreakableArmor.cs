using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BreakableArmor : MonoBehaviour {

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Break()
    {
        animator.SetTrigger("Break");
    }


}
