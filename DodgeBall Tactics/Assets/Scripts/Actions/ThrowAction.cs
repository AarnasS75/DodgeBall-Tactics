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

    [SerializeField] private int damageAmmount = 30;

    public event EventHandler OnThrowStart;

    private State state;
    [SerializeField] private Animator animator;
    private Unit targetUnit;
    [SerializeField] private int maxThrowDistance = 4;
    private bool canThrowBall;

    private Vector3 aimDir;
    private float stateTimer;

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
                aimDir = targetUnit.GetWorldPosition() - unit.GetWorldPosition();
                //float rotateSpeed = 10f;
                //transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotateSpeed);
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
        aimDir.Normalize();
        OnThrowStart?.Invoke(this, EventArgs.Empty);
    }

    public override void TakeAction(GridPosition gridPosition, Action OnActionComplete)
    {
        targetUnit = LevelGrid.Instance.GetUnitAtGridposition(gridPosition);
        canThrowBall = true;
       
        float timeAimingState = 1f;
        stateTimer = timeAimingState;
        state = State.Aiming;

        ActionStart(OnActionComplete);
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

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();     // Getting units current position

        return GetValidActionGridPositionList(unitGridPosition);    // Checking if units position + action range are compatible
    }

    public List<GridPosition> GetValidActionGridPositionList(GridPosition unitPosition)
    {
        List<GridPosition> validGridPositions = new();

        for (int x = -maxThrowDistance; x <= maxThrowDistance; x++)
        {
            for (int z = -maxThrowDistance; z <= maxThrowDistance; z++)
            {
                GridPosition offsetGridPosition = new(x, z);
                GridPosition testGridPosition = unitPosition + offsetGridPosition;  // Get throw tile range

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition, unit, true))
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
    public Unit GetTargetUnit() => targetUnit;
    public int GetMaxThrowDistance() => maxThrowDistance;

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        Unit targetUnit = LevelGrid.Instance.GetUnitAtGridposition(gridPosition);

        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 100 + Mathf.RoundToInt((1 - targetUnit.GetHealthNormalized()) * 100f) // Targets unit with least health
        };
    }
    public int GetTargetCountAtPosition(GridPosition gridPosition)
    {
       return GetValidActionGridPositionList(gridPosition).Count;
    }
}
