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
    public event EventHandler OnSelectedActionChanged;
    public event EventHandler<bool> OnBusyChanged;
    public event EventHandler OnActionStarted;

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
        if (isBusy)
        {
            return; // If action is not finished, return
        }

        if (!TurnSystem.Instance.IsPlayerTurn())
        {
            return;
        }

        if (EventSystem.current.IsPointerOverGameObject())  // If mouse is on UI, don't call actions at the same time
        {
            return;
        }

        if (TryHandleUnitSelection())
        {
            return; 
        }

        HandleSelectedAction();
    }
    private void HandleSelectedAction()
    {
        if (InputManager.Instance.IsMouseButtonDown())
        {
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.Instance.GetPosition());

            if (selectedAction.IsValidActionGridPosition(mouseGridPosition))
            {
                if (selectedUnit.TrySpendActionPointsForAction(selectedAction))
                {
                    SetBusy();

                    selectedAction.TakeAction(mouseGridPosition, ClearBusy);
                    OnActionStarted?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }

    private void SetBusy()
    {
        isBusy = true;
        OnBusyChanged?.Invoke(this, isBusy);
    }
    private void ClearBusy()
    {
        isBusy = false;
        OnBusyChanged?.Invoke(this, isBusy);
    }
    private bool TryHandleUnitSelection()
    {
        if (InputManager.Instance.IsMouseButtonDown())
        {
            Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.GetMouseScreenPosition());
            if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, unitMask))
            {
                if (hit.transform.TryGetComponent(out Unit unit))
                {
                    if(unit == selectedUnit)
                    {
                        // Unit is already selected
                        return false;
                    }

                    if (unit.IsEnemy())
                    {
                        // Clicked on enemy
                        return false;
                    }
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
        SetSelectedAction(unit.GetAction<MoveAction>());           // Set default action to perform, when player is selected.
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }
    public void SetRandomSelectedUnit(bool isEnemy)
    {
        if (isEnemy)
        {
            selectedUnit = UnitManager.Instance.enemyUnitList[0];
        }
        else
        {
            selectedUnit = UnitManager.Instance.friendlyUnitList[0];
        }
    }
    public BaseAction GetSelectedAction()
    {
        return selectedAction;
    }
    public void SetSelectedAction(BaseAction baseAction)
    {
        selectedAction = baseAction;

        OnSelectedActionChanged?.Invoke(this, EventArgs.Empty);
    }
    public Unit GetSelectedUnit()
    {
        return selectedUnit;
    }
}
