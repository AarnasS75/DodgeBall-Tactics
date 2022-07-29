using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] private Transform actionButtonPrefab;
    [SerializeField] private Transform actionButtonContainerTransform;

    //[SerializeField] private TextMeshProUGUI actionPointsText;

    List<ActionButtonUI> actionButtonUIList;
    private void Awake()
    {
        actionButtonUIList = new();
    }
    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
        UnitActionSystem.Instance.OnActionStarted += UnitActionSystem_OnActionStarted;

        TurnSystem.Instance.OnTurnChangedEvent += TurnSystem_OnTurnChangedEvent;
        Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;


        CreateUnitActionButtons();
        UpdateSelectedVisual();
        UpdateActionPoints();
    }

    private void Unit_OnAnyActionPointsChanged(object sender, EventArgs e)
    {
        UpdateActionPoints();
    }

    private void TurnSystem_OnTurnChangedEvent(object sender, EventArgs e)
    {
        UpdateActionPoints();
    }

    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
    {
        UpdateSelectedVisual();
    }
    private void UnitActionSystem_OnActionStarted(object sender, EventArgs e)
    {
        UpdateActionPoints();
    }
    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs e)
    {
        CreateUnitActionButtons();
        UpdateSelectedVisual();
        UpdateActionPoints();
    }

    private void CreateUnitActionButtons()
    {
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();

        foreach (Transform actionButton in actionButtonContainerTransform)
        {
            Destroy(actionButton.gameObject);
        }
        actionButtonUIList.Clear();
        
        foreach (BaseAction baseAction in selectedUnit.GetBaseActionArray())
        {
            Transform actionButtonTransform = Instantiate(actionButtonPrefab, actionButtonContainerTransform);
            ActionButtonUI actionButtonUI = actionButtonTransform.GetComponent<ActionButtonUI>();
            actionButtonUI.SetBaseAction(baseAction);
            actionButtonUIList.Add(actionButtonUI);
        }
    }
    private void UpdateSelectedVisual()
    {
        foreach (var button in actionButtonUIList)
        {
            button.UpdateSelectedVisual();
        }
    }
    private void UpdateActionPoints()
    {
        Unit unit = UnitActionSystem.Instance.GetSelectedUnit();
        //actionPointsText.text = "Action Points: " + unit.GetActionPoints();
    }
}
