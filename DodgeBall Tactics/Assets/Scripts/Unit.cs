using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private bool isEnemy;

    [SerializeField] private int maxActionPoints = 4;   // Can change to const

    public static event EventHandler OnAnyActionPointsChanged, OnAnyUnitSpawned, OnAnyUnitDead;

    private GridPosition currentGridPosition;
    private HealthSystem healthSystem;
    private BaseAction[] baseActionArray;

    private int actionPoints;

    private void Awake()
    {
        actionPoints = maxActionPoints;

        healthSystem = GetComponent<HealthSystem>();
        baseActionArray = GetComponents<BaseAction>();
    }
    private void Start()
    {
        currentGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(currentGridPosition, this);

        TurnSystem.Instance.OnTurnChangedEvent += TurnSystem_OnTurnChangedEvent;

        healthSystem.OnDead += HealthSystem_OnDead; 
        OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);

    }

    private void Update()
    {
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);

        if(newGridPosition != currentGridPosition)
        {
            GridPosition oldGridPosition = currentGridPosition;
            currentGridPosition = newGridPosition;
            // Unit changed grid position
            LevelGrid.Instance.UnitMovedGridPosition(this, oldGridPosition, newGridPosition);

        }
    }
    public T GetAction<T>() where T : BaseAction
    {
        foreach (BaseAction baseAction in baseActionArray)
        {
            if (baseAction is T)
            {
                return (T)baseAction;
            }
        }
        return null;
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
        else 
        { 
            return false; 
        }
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
    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        LevelGrid.Instance.RemoveUnitAtGridPosition(currentGridPosition, this);
        Destroy(gameObject);

        OnAnyUnitDead?.Invoke(this, EventArgs.Empty);
    }

    public int GetActionPoints() => actionPoints;

    public bool IsEnemy() => isEnemy;

    public void Damage(int damageAmmount)
    {
        healthSystem.TakeDamage(damageAmmount);
    }
    public Vector3 GetWorldPosition() => transform.position;

    public float GetHealthNormalized() => healthSystem.GetHealthNormalized(); 
}
