using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private GridPosition currentGridPosition;
    private MoveAction moveAction;
    private ThrowAction throwAction;
    private BaseAction[] baseActionArray;


    private void Awake()
    {
        moveAction = GetComponent<MoveAction>();
        throwAction = GetComponent<ThrowAction>();
        baseActionArray = GetComponents<BaseAction>();
    }
    private void Start()
    {
        currentGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(currentGridPosition, this);
    }

    private void Update()
    {
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);

        if(newGridPosition != currentGridPosition)
        {
            // Unit changed grid position
            LevelGrid.Instance.UnitMovedGridPosition(this, currentGridPosition, newGridPosition);
            currentGridPosition = newGridPosition;
        }
    }
    public MoveAction GetMoveAction()
    {
        return moveAction;
    }
    public ThrowAction GetThrowAction()
    {
        return throwAction;
    }
    // Get grid tile position, on which the unit is standing
    public GridPosition GetGridPosition()
    {
        return currentGridPosition;
    }
    // Get unit all possible actions array
    public BaseAction[] GetBaseActionArray()
    {
        return baseActionArray;
    }
}
