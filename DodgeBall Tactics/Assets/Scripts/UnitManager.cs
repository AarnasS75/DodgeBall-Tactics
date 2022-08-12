using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance { get; private set; }

    public List<Unit> unitList;
    public List<Unit> friendlyUnitList;
    public List<Unit> enemyUnitList;

    public Unit activeUnit;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        unitList = new();
        friendlyUnitList = new();
        enemyUnitList = new();
    }
    private void Start()
    {
        Unit.OnAnyUnitSpawned += Unit_OnAnyUnitSpawned;
        Unit.OnAnyUnitDead += Unit_OnAnyUnitDead;
    }
    private void Unit_OnAnyUnitSpawned(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;

        unitList.Add(unit);

        if (unit.IsEnemy())
        {
            enemyUnitList.Add(unit);
        }
        else
        {
            friendlyUnitList.Add(unit);
        }
    }

    private void Unit_OnAnyUnitDead(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;

        unitList.Remove(unit);

        if (unit.IsEnemy())
        {
            enemyUnitList.Remove(unit); 
            UnitActionSystem.Instance.SetRandomSelectedUnit(true);
        }
        else
        {
            friendlyUnitList.Remove(unit);
            UnitActionSystem.Instance.SetRandomSelectedUnit(false);
        }
    }
    public List<Unit> GetUnitList() => unitList;
    public List<Unit> GetFriendlyUnitList() => friendlyUnitList;
    public List<Unit> GetEnemyUnitList() => enemyUnitList;
    public void SetNewActiveUnit(Unit unit)
    {
        activeUnit = unit;
    }
}
