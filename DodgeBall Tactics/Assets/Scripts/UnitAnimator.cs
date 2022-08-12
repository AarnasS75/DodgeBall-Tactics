using System;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform ballPrefabTransform;
    [SerializeField] private Transform throwPointTransform;

    Unit unit;

    private void Awake()
    {
        unit = GetComponent<Unit>();

        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;

        if (TryGetComponent(out MoveAction moveAction))
        {
            moveAction.OnStartMoving += MoveAction_OnStartMoving;
            moveAction.OnStopMoving += MoveAction_OnStopMoving;
        }

        if (TryGetComponent(out ThrowAction throwAction))
        {
            throwAction.OnThrowStart += ThrowAction_OnThrowStart;
        }
    }

    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
    {
        if (UnitActionSystem.Instance.GetSelectedUnit() == unit)
        {
            if (UnitActionSystem.Instance.GetSelectedAction() == unit.GetAction<ThrowAction>())
            {
                animator.SetBool("Attacking", true);
            }
            else
            {
                animator.SetBool("Attacking", false);
            }
        }
    }
    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs e)
    {
       
            animator.SetBool("Attacking", false);
        
    }

    private void ThrowAction_OnThrowStart(object sender, EventArgs e)
    {
        Instantiate(ballPrefabTransform, throwPointTransform.position, Quaternion.identity, transform);

    }

    private void MoveAction_OnStartMoving(object sender, EventArgs e)
    {
        //animator.SetBool("IsWalking", true);
    }
    private void MoveAction_OnStopMoving(object sender, EventArgs e)
    {
        //animator.SetBool("IsWalking", false);
    }
}
