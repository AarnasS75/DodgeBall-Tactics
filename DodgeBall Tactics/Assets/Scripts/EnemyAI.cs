using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private float timer;

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
        else
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                TurnSystem.Instance.NextTurn();
            }
        }
    }
    private void TurnSystem_OnTurnChangedEvent(object sender, EventArgs e)
    {
        timer = 2f;
    }

}