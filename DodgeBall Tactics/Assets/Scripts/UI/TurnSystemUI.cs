using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField] private Button nextTurnButton;
    [SerializeField] private TextMeshProUGUI turnNumberText;

    //[SerializeField] private GameObject enemyTurnVisual;      If you want you can add banner when enemy attacks

    private void Start()
    {
        nextTurnButton.onClick.AddListener(() =>
        {
            TurnSystem.Instance.NextTurn();
        });
        TurnSystem.Instance.OnTurnChangedEvent += TurnSystem_OnTurnChangedEvent;
        //UpdateEnemyTurnVisual();
        UpdateEndTurnButtonVisibility();
    }

    private void TurnSystem_OnTurnChangedEvent(object sender, EventArgs e)
    {
        UpdateText();
        //UpdateEnemyTurnVisual();
        UpdateEndTurnButtonVisibility();
    }

    private void UpdateText()
    {
        turnNumberText.text = "Turn: " + TurnSystem.Instance.GetTurnNumber();
    }
    //private void UpdateEnemyTurnVisual()
    //{
    //    enemyTurnVisual.SetActive(!TurnSystem.Instance.IsPlayerTurn());
    //}
    private void UpdateEndTurnButtonVisibility()
    {
        nextTurnButton.gameObject.SetActive(TurnSystem.Instance.IsPlayerTurn());
    }
}
