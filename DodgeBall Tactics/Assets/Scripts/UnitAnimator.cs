using System;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform ballPrefabTransform;
    [SerializeField] private Transform throwPointTransform;

    private void Awake()
    {
        if(TryGetComponent(out MoveAction moveAction))
        {
            moveAction.OnStartMoving += MoveAction_OnStartMoving;
            moveAction.OnStopMoving += MoveAction_OnStopMoving;
        }

        if (TryGetComponent(out ThrowAction throwAction))
        {
            throwAction.OnThrowStart += ThrowAction_OnThrowStart;
        }
    }

    private void ThrowAction_OnThrowStart(object sender, EventArgs e)
    {
        animator.SetTrigger("Throw_T");
        Instantiate(ballPrefabTransform, throwPointTransform.position, Quaternion.identity, transform);

    }

    private void MoveAction_OnStartMoving(object sender, EventArgs e)
    {
        animator.SetBool("IsWalking", true);
    }
    private void MoveAction_OnStopMoving(object sender, EventArgs e)
    {
        animator.SetBool("IsWalking", false);
    }
}
