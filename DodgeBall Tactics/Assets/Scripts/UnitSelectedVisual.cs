using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectedVisual : MonoBehaviour
{
    [SerializeField] private Unit selectedUnit;

    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }
    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
        TurnSystem.Instance.OnTurnChangedEvent += TurnSystem_OnTurnChangedEvent;

        UpdateVisual();
    }

    private void TurnSystem_OnTurnChangedEvent(object sender, EventArgs e)
    {
        meshRenderer.enabled = false;
    }

    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs empty)
    {
        UpdateVisual();
    }
    private void UpdateVisual()
    {
        if (UnitActionSystem.Instance.GetSelectedUnit() == selectedUnit)
        {
            meshRenderer.enabled = true;
        }
        else
        {
            meshRenderer.enabled = false;
        }
    }
    private void OnDestroy()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged -= UnitActionSystem_OnSelectedUnitChanged;
    }
}
