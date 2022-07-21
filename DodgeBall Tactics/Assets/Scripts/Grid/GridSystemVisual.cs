using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    public static GridSystemVisual Instance { get; private set; }


    [SerializeField] private Transform gridVisualSinglePrefab;

    private GridSystemVisualSingle[,] gridSystemVisualSingleArray;

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
                GridPosition gridPos = new GridPosition(x,z);
                Transform gridSystemVisualSinglePrefab = Instantiate(gridVisualSinglePrefab, LevelGrid.Instance.GetWorldPosition(gridPos), Quaternion.identity);

                gridSystemVisualSingleArray[x,z] = gridSystemVisualSinglePrefab.GetComponent<GridSystemVisualSingle>();
            }
        }
    }
    private void Update()
    {
        UpdateGridVisual();
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
    public void ShowGridPositionList(List<GridPosition> gridPositionList)
    {
        foreach (var gridPosition in gridPositionList)
        {
            gridSystemVisualSingleArray[gridPosition.x, gridPosition.z].Show();
        }
    }
    private void UpdateGridVisual()
    {
        HideAllGridPositions();
        ShowGridPositionList(UnitActionSystem.Instance.GetSelectedUnit().GetMoveAction().GetValidActionGridPositionList());
    }
}
