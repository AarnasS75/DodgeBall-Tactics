using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }

    [SerializeField] private Unit selectedUnit;
    [SerializeField] private LayerMask unitMask;

    public event EventHandler OnSelectedUnitChanged;

    private BaseAction selectedAction;

    private bool isBusy;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    private void Start()
    {
        SetSelectedUnit(selectedUnit);
    }
    void Update()
    {
        if(isBusy) return;  // If action is not finished, return

        if (EventSystem.current.IsPointerOverGameObject())  // If mouse is on UI, don't call actions at the same time
        {
            return;
        }

        if (TryHandleUnitSelection()) return;   // Unit selection

        HandleSelectedAction();
    }
    private void HandleSelectedAction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());

            if (selectedAction.IsValidActionGridPosition(mouseGridPosition))
            {
                SetBusy();
                selectedAction.TakeAction(mouseGridPosition, ClearBusy);
            }
        }
    }

    private void SetBusy()
    {
        isBusy = true;
    }
    private void ClearBusy()
    {
        isBusy = false;
    }
    private bool TryHandleUnitSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, unitMask))
            {
                if (hit.transform.TryGetComponent(out Unit unit))
                {
                    SetSelectedUnit(unit);
                    return true;
                }
            }
        }
        return false;
    }
    private void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;
        SetSelectedAction(unit.GetMoveAction());           // Set default action to perform, when player is selected.
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }
    public BaseAction GetSelectedAction()
    {
        return selectedAction;
    }
    public void SetSelectedAction(BaseAction baseAction)
    {
        selectedAction = baseAction;
    }
    public Unit GetSelectedUnit()
    {
        return selectedUnit;
    }
}
