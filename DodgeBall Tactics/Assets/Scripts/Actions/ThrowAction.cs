using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ThrowAction : BaseAction
{
    [SerializeField] private Animator animator;

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
    // Gets tile position, which can be pressed to perform action. At the moment 
    // it only calls Throw action, if player presses same tile as the unit is standing on.
    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition unitGridposition = unit.GetGridPosition();        // TODO: Change to select enemy tile. Also maybe add distance.

        return new List<GridPosition>
        {
            unitGridposition
        };
    }
}
