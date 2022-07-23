using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ThrowAction : BaseAction
{
    private enum State
    {
        Aiming,
        Shooting,
        Cooloff
    }

    public event EventHandler OnThrowStart;

    private State state;
    [SerializeField] private Animator animator;
    private Unit targetUnit;
    private int maxThrowDistance = 4;
    private bool canThrowBall;

    Vector3 aimDir;
    float stateTimer;

    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        stateTimer -= Time.deltaTime;

        switch (state)
        {
            case State.Aiming:
                aimDir = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
                float rotateSpeed = 10f;
                transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotateSpeed);
                break;
            case State.Shooting:
                if (canThrowBall)
                {
                    ThrowBall();
                    canThrowBall = false;
                }
                break;
            case State.Cooloff:
                break;
        }
        if (stateTimer <= 0)
        {
            NextState();
        }
    }
    private void NextState()
    {
        switch (state)
        {
            case State.Aiming:
                if(stateTimer <= 0f)
                {
                    state = State.Shooting;
                    float timeAimingState = 0.4f;
                    stateTimer = timeAimingState;
                
                }
                break;
            case State.Shooting:
                if (stateTimer <= 0f)
                {
                    state = State.Cooloff;
                    float coolOffStateTime = 0.6f;
                    stateTimer = coolOffStateTime;
                }
                break;
            case State.Cooloff:
                ActionComplete();
                break;
        }
    }

    private void ThrowBall()
    {
        OnThrowStart?.Invoke(this, EventArgs.Empty);
        targetUnit.Damage();
    }

    public override void TakeAction(GridPosition gridPosition, Action OnActionComplete)
    {
        ActionStart(OnActionComplete);
        targetUnit = LevelGrid.Instance.GetUnitAtGridposition(gridPosition);
        canThrowBall = true;
       

        float timeAimingState = 1f;
        stateTimer = timeAimingState;
        state = State.Aiming;

    }
    public void EndThrowAnimation()
    {
        isActive = false;
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
        List<GridPosition> validGridPositions = new();

        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxThrowDistance; x <= maxThrowDistance; x++)
        {
            for (int z = -maxThrowDistance; z <= maxThrowDistance; z++)
            {
                GridPosition offsetGridPosition = new(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    // If tile position is outside of declared boundaries
                    continue;
                }

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > maxThrowDistance)
                {
                    continue;
                }

                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    // Grid position is empty, no Unit
                    continue;
                }
                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridposition(testGridPosition);

                if (targetUnit.IsEnemy() == unit.IsEnemy())
                {
                    // Both units are on the same team
                    continue;
                }
                validGridPositions.Add(testGridPosition);
            }
        }
        return validGridPositions;
    }
    public override int GetActionPointsCost()
    {
        return 2;
    }
    public Vector3 GetThrowDirection()
    {
        return aimDir;
    }
}
