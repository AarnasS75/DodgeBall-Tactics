using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{
    public event EventHandler OnStartMoving, OnStopMoving;

    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float rotateSpeed = 20f;

    [SerializeField] private int maxMoveDistance = 4;

    private float stoppingDistance = 0.1f;

    [SerializeField] private Vector3 targetPosition;

    protected override void Awake()
    {
        base.Awake();
        targetPosition = transform.position;
    }
    private void Update()
    {
        if (!isActive) { return; }

        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            Vector3 moveDiection = (targetPosition - transform.position).normalized;
            transform.position += moveDiection * Time.deltaTime * moveSpeed;
            transform.forward = Vector3.Lerp(transform.forward, moveDiection, Time.deltaTime * rotateSpeed);
        }
        else
        {
            OnStopMoving?.Invoke(this, EventArgs.Empty);
            OnActionComplete();
        }

        
    }
    public override void TakeAction(GridPosition gridPosition, Action OnActionComplete)
    {
        targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
        OnStartMoving?.Invoke(this, EventArgs.Empty);
        ActionStart(OnActionComplete);
    }
    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositions = new();

        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                GridPosition offsetGridPosition = new(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if(!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    // If tile position is outside of declared boundaries
                    continue;
                }
                if(unitGridPosition == testGridPosition)
                {
                    // If Unit position is same as possible to move tile position
                    continue;
                }
                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    // Grid position is ocupied by other unit
                    continue;
                }
                validGridPositions.Add(testGridPosition);
            }
        }

        return validGridPositions;
    }
    public override string GetActionName()
    {
        return "Move";
    }
}
