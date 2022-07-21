using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowAction : BaseAction
{
    private Animator animator;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
    }
    public void Throw()
    {
        animator.SetBool("Throw", true);
    }
}
