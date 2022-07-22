using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{
    [SerializeField] private Animator animator;

    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float rotateSpeed = 10f;

    [SerializeField] private int maxMoveDistance = 4;

    private float stoppingDistance = 0.1f;

    private Vector3 targetPosition;

    protected override void Awake()
    {
        base.Awake();   // To also run parent Awake and not only override completely
        targetPosition = transform.position;
    }
    private void Update()
    {
        if (!isActive) { return; }

        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            animator.SetBool("Walking", true);
            Vector3 moveDiection = (targetPosition - transform.position).normalized;
            transform.position += moveDiection * Time.deltaTime * moveSpeed;
            transform.forward = Vector3.Lerp(transform.forward, moveDiection, Time.deltaTime * rotateSpeed);
        }
        else
        {
            animator.SetBool("Walking", false);
            isActive = false;
            OnActionComplete(); // Null
        }
    }
    public override void TakeAction(GridPosition gridPosition, Action OnActionComplete)
    {
        this.OnActionComplete = OnActionComplete;
        targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
        isActive = true;
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
