using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem
{
    private int width;
    private int length;
    private float cellSize;

    private GridObject[,] gridObjects;

    public GridSystem(int length, int width, float cellSize)
    {
        this.length = length;
        this.width = width;
        this.cellSize = cellSize;

        gridObjects = new GridObject[width, length];

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < length; z++)
            {
                gridObjects[x,z] = new GridObject(new GridPosition(x, z), this);
            }
        }
    }
    public Vector3 GetWorldPosition(GridPosition gridPosition)
    {
        return new Vector3(gridPosition.x, 0, gridPosition.z) * cellSize;
    }
    public GridPosition GetGridPosition(Vector3 worldPos)
    {
        return new GridPosition(
            Mathf.RoundToInt(worldPos.x / cellSize),
            Mathf.RoundToInt(worldPos.z / cellSize));
    }
    public void CreateDebugObjects(Transform debugPrefab)
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < length; z++)
            {
                GridPosition gridPosition = new GridPosition(x,z);
                Transform debugObject = GameObject.Instantiate(debugPrefab, GetWorldPosition(gridPosition), Quaternion.identity);
                GridDebugObject gridDebugObject = debugObject.GetComponent<GridDebugObject>();
                gridDebugObject.SetGridobject(GetGridObject(gridPosition));
            }
        }
    }
    public GridObject GetGridObject(GridPosition gridPosition)
    {
        return gridObjects[gridPosition.x, gridPosition.z];
    }
    public bool IsValidGridPosition(GridPosition gridPosition, Unit unit, bool attacking = false)
    {
        if (!attacking)
        {
            if (unit.transform.position.z <= 6 && unit.transform.position.z >= 0)
            {
                // Unit is on right side
                return gridPosition.x >= 0 &&
                   gridPosition.z >= 0 &&
                   gridPosition.x < width &&
                   gridPosition.z < length / 2;
            }
            else
            {
                // Unit is on left side
                return gridPosition.x >= 0 &&
                   gridPosition.z >= length / 2 &&
                   gridPosition.x < width &&
                   gridPosition.z < length;
            }
        }
        else
        {
            // Check the whole board
            return gridPosition.x >= 0 &&
               gridPosition.z >= 0 &&
               gridPosition.x < width &&
               gridPosition.z < length;
        }
    }
    public int GetWidth() => width;
    public int GetHeight() => length;
}
