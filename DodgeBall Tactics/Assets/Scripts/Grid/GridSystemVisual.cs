using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    public static GridSystemVisual Instance { get; private set; }

    [Serializable]
    public struct GridVisualTypeMaterial
    {
        public VisualType gridVisualType;
        public Material material;
    }
    public enum VisualType
    {
        White,
        Blue,
        RedSoft,
        Red
    }

    [SerializeField] private Transform gridVisualSinglePrefab;
    [SerializeField] private List<GridVisualTypeMaterial> gridVisualTypeList;

    private GridSystemVisualSingle[,] gridSystemVisualSingleArray;

    Unit selectedUnit;

    private void Awake()
    {
        if (Instance == null)
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
        gridSystemVisualSingleArray = new GridSystemVisualSingle[
            LevelGrid.Instance.GetGridWidth(),
            LevelGrid.Instance.GetGridHeight()
            ];

        for (int x = 0; x < LevelGrid.Instance.GetGridWidth(); x++)
        {
            for (int z = 0; z < LevelGrid.Instance.GetGridHeight(); z++)
            {
                GridPosition gridPos = new(x,z);
                Transform gridSystemVisualSinglePrefab = Instantiate(gridVisualSinglePrefab, LevelGrid.Instance.GetWorldPosition(gridPos), Quaternion.identity);

                gridSystemVisualSingleArray[x,z] = gridSystemVisualSinglePrefab.GetComponent<GridSystemVisualSingle>();
            }
        }

        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
        LevelGrid.Instance.OnAnyUnitMovedGridPosition += LevelGrid_OnAnyUnitMovedGridPosition;

        TurnSystem.Instance.OnTurnChangedEvent += TurnSystem_OnTurnChangedEvent;

        UpdateGridVisual();
    }

    private void TurnSystem_OnTurnChangedEvent(object sender, EventArgs e)
    {
        HideAllGridPositions();
    }

    public void HideAllGridPositions()
    {
        for (int x = 0; x < LevelGrid.Instance.GetGridWidth(); x++)
        {
            for (int z = 0; z < LevelGrid.Instance.GetGridHeight(); z++)
            {
                gridSystemVisualSingleArray[x, z].Hide();
            }
        }
    }
    public void ShowGridPositionList(List<GridPosition> gridPositionList, VisualType visualType)
    {
        foreach (var gridPosition in gridPositionList)
        {
            gridSystemVisualSingleArray[gridPosition.x, gridPosition.z].Show(GetGridVisualMaterial(visualType));
        }
    }
    private void UpdateGridVisual()
    {
        HideAllGridPositions();

        selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();

        VisualType visualType = VisualType.White;

        switch (selectedAction)
        {
            case MoveAction moveAction:
                visualType = VisualType.White;
                break;
            case ThrowAction throwAction:
                visualType = VisualType.Red;
                ShowGridPositonRange(selectedUnit.GetGridPosition(), throwAction.GetMaxThrowDistance(), VisualType.RedSoft);
                break;
        }

        ShowGridPositionList(selectedAction.GetValidActionGridPositionList(), visualType);
    }
    private void ShowGridPositonRange(GridPosition gridPosition, int range, VisualType visualType)
    {
        List<GridPosition> gridPositionList = new();

        for (int x = -range; x <= range; x++)
        {
            for (int z = -range; z <= range; z++)
            {
                GridPosition testGridPosition = gridPosition + new GridPosition(x, z);

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition, selectedUnit))
                {
                    // If tile position is outside of declared boundaries
                       continue;
                }

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > range)
                {
                    continue;
                }
                gridPositionList.Add(testGridPosition);
            }
        }
        ShowGridPositionList(gridPositionList, visualType);
    }

    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
    {
        UpdateGridVisual();
    }  
    private void LevelGrid_OnAnyUnitMovedGridPosition(object sender, EventArgs e)
    {
        UpdateGridVisual();
    }
    private Material GetGridVisualMaterial(VisualType visualType)
    {
        foreach (var gridVisualMaterial in gridVisualTypeList)
        {
            if(gridVisualMaterial.gridVisualType == visualType)
            {
                return gridVisualMaterial.material;
            }
        }
        Debug.LogError("Could not find GridVisualTypeMaterila for GridVisualType" + visualType);
        return null;
    }
}
