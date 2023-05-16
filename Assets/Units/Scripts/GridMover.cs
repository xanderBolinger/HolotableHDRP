using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMover : MonoBehaviour
{

    public Dictionary<Vector2Int, List<Unit>> unitLocations;

    public void MoveUnit(Unit unit, Vector2Int cord, Vector3 oldPosition, Vector3 moveToPosition) {
        if(unitLocations == null)
            unitLocations = new Dictionary<Vector2Int, List<Unit>>();

        RemoveUnit(unit, cord, oldPosition);
        AddUnit(unit, cord, moveToPosition);
    }

    

    public float GetUnitElevation(int unitIndex, Vector3 worldPosition) {
        var y = worldPosition.y;

        if (unitIndex == 0)
        {
            y += 0.15f;
        }
        else
        {
            y += 0.15f + 0.025f * unitIndex;
        }

        return y;
    }


    public Unit GetClickedUnit(GameObject clickedUnit) {

        foreach (var units in unitLocations) {
            foreach (var unit in units.Value)
            {
                if (unit.unitGameobject == clickedUnit)
                    return unit;
            }
        }
        throw new Exception("Unit not found clicked unit: "+clickedUnit.name);
    }

    private void AddUnit(Unit unit, Vector2Int cord, Vector3 worldPosition)
    {
        int units = unitLocations[cord].Count;

        var y = GetUnitElevation(units, worldPosition);

        unit.unitGameobject.transform.position = new Vector3(worldPosition.x, y, worldPosition.z);
        unitLocations[cord].Add(unit);
        unit.cord = cord;

    }

    private void RemoveUnit(Unit unit, Vector2Int moveToCord, Vector3 oldPosition)
    {
        if (!unitLocations.ContainsKey(moveToCord))
        {
            unitLocations.Add(moveToCord, new List<Unit>());
        }

        if (!unitLocations.ContainsKey(unit.cord))
            return;

        Vector2Int cord = unit.cord;
        var units = unitLocations[cord];

        if (units.Contains(unit)) {
            units.Remove(unit);

            for (int i = 0; i < units.Count; i++) {
                units[i].unitGameobject.transform.position = new Vector3(oldPosition.x, GetUnitElevation(i, oldPosition), oldPosition.z);
            }
            
        }

    }

   

}
