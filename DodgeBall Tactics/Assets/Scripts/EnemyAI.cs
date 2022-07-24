using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private enum State
    {
        WaitingForEnemyTurn,
        TakingTurn,
        Busy
    }

    State state;

    private float timer;
    private void Awake()
    {
        state = State.WaitingForEnemyTurn;
    }
    private void Start()
    {
        TurnSystem.Instance.OnTurnChangedEvent += TurnSystem_OnTurnChangedEvent;
    }

    private void Update()
    {
        if (TurnSystem.Instance.IsPlayerTurn())
        {
            return;
        }

        switch (state)
        {
            case State.WaitingForEnemyTurn:

                break;
            case State.TakingTurn:

                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    if (TryTakeEnemyAIAction(SetStateTakingTurn))
                    {
                        print("Taking action");
                        state = State.Busy;
                    }
                    else
                    {
                        print("Enemies are out of actions");
                        // enemies are out of actions
                        TurnSystem.Instance.NextTurn();
                    }
                }
                break;
            case State.Busy:

                break;
        }


    }
    private void TurnSystem_OnTurnChangedEvent(object sender, EventArgs e)
    {
        if (!TurnSystem.Instance.IsPlayerTurn())
        {
            state = State.TakingTurn;
            timer = 2f;
        }
    }
    private void SetStateTakingTurn()
    {
        timer = 0.5f;
        state = State.TakingTurn;
    }
    private bool TryTakeEnemyAIAction(Action onEnemyAIaCtionComplete)
    {
        print("Enemy takes action");
        foreach (var enemyUnit in UnitManager.Instance.GetEnemyUnitList())
        {
            if(TryTakeEnemyAIAction(enemyUnit, onEnemyAIaCtionComplete))
            {
                print("Enemy throw action");
                return true;
            }
            print("Enemy throw action not available");
        }
        return false;
    }
    private bool TryTakeEnemyAIAction(Unit enemyUnit, Action onEnemyAIactionComplete)
    {
        EnemyAIAction bestEnemyAIAction = null;
        BaseAction bestBaseAction = null;

        foreach (var baseAction in enemyUnit.GetBaseActionArray())
        {
            if( !enemyUnit.CanSpendActionPoints(baseAction))
            {
                // Enemy cannot afford this action
                continue;
            }
            else
            {
                if(bestEnemyAIAction == null)
                {
                    bestEnemyAIAction = baseAction.GetBestEnemyAIAction();
                    bestBaseAction = baseAction;
                }
                else
                {
                    EnemyAIAction testEnemyAIAction = baseAction.GetBestEnemyAIAction();

                    if (testEnemyAIAction != null && testEnemyAIAction.actionValue > bestEnemyAIAction.actionValue)
                    {
                        bestEnemyAIAction = testEnemyAIAction;
                        bestBaseAction = baseAction;
                    }
                }
            }
        }

        if(bestEnemyAIAction != null && enemyUnit.TrySpendActionPointsForAction(bestBaseAction))
        {
            bestBaseAction.TakeAction(bestEnemyAIAction.gridPosition, onEnemyAIactionComplete);
            return true;
        }
        else
        {
            return false;
        }
    }
}
