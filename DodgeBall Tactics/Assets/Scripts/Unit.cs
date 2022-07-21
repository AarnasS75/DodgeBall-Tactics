using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private GridPosition currentGridPosition;
    private MoveAction moveAction;
    private ThrowAction throwAction;

    private void Awake()
    {
        moveAction = GetComponent<MoveAction>();
        throwAction = GetComponent<ThrowAction>();
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
    public GridPosition GetGridPosition()
    {
        return currentGridPosition;
    }
}
