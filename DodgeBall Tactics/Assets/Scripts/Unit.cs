using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private bool isEnemy;

    [SerializeField] private int maxActionPoints = 4;   // Can change to const

    public static event EventHandler OnAnyActionPointsChanged;

    private GridPosition currentGridPosition;
    private MoveAction moveAction;
    private ThrowAction throwAction;
    private BaseAction[] baseActionArray;

    private int actionPoints;

    private void Awake()
    {
        actionPoints = maxActionPoints;

        moveAction = GetComponent<MoveAction>();
        throwAction = GetComponent<ThrowAction>();
        baseActionArray = GetComponents<BaseAction>();
    }
    private void Start()
    {
        currentGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(currentGridPosition, this);

        TurnSystem.Instance.OnTurnChangedEvent += TurnSystem_OnTurnChangedEvent;
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
    // Get units all possible actions array
    public BaseAction[] GetBaseActionArray()
    {
        return baseActionArray;
    }

    public bool TrySpendActionPointsForAction(BaseAction baseAction)
    {
        if (CanSpendActionPoints(baseAction))
        {
            SpendActionPoint(baseAction.GetActionPointsCost());
            return true;
        }
        else { return false; }
    }

    public bool CanSpendActionPoints(BaseAction baseAction)
    {
        return actionPoints >= baseAction.GetActionPointsCost();
    }
    private void SpendActionPoint(int ammount)
    {
        actionPoints -= ammount;

        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }

    private void TurnSystem_OnTurnChangedEvent(object sender, EventArgs e)
    {
        if ((IsEnemy() && TurnSystem.Instance.IsPlayerTurn()) ||
            (!IsEnemy() && TurnSystem.Instance.IsPlayerTurn()))
        {
            actionPoints = maxActionPoints;
            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
        }
        

      
    }
    public int GetActionPoints() => actionPoints;

    public bool IsEnemy() => isEnemy;

    public void Damage()
    {
        print(transform + " Deal damage: -20");
    }
    public Vector3 GetWorldPosition() => transform.position;

}
