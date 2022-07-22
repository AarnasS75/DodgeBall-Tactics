using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ThrowAction : BaseAction
{
    [SerializeField] private Animator animator;

    // Delegate to store reference to a function and call it when needed
    public override void TakeAction(GridPosition gridPosition, Action OnActionComplete)
    {
        this.OnActionComplete = OnActionComplete;
        isActive = true;
        animator.SetBool("Throw", true);
    }
    public void EndThrowAnimation()
    {
        OnActionComplete();
    }
    public override string GetActionName()
    {
        return "Throw";
    }
    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositions = new();
        GridPosition unitGridposition = unit.GetGridPosition();

        return new List<GridPosition>
        {
            unitGridposition
        };
    }
}
