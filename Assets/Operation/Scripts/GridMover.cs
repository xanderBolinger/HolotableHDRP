using Operation;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMover : MonoBehaviour
{

    public Dictionary<Vector2Int, List<OperationUnit>> unitLocations;

    public void MoveUnit(OperationUnit unit, Vector2Int cord, Vector3 oldPosition, Vector3 moveToPosition, bool clear=false) {
        if(unitLocations == null)
            unitLocations = new Dictionary<Vector2Int, List<OperationUnit>>();

        RemoveUnit(unit, cord, oldPosition);
        AddUnit(unit, cord, moveToPosition);

        if(clear)
            unit.unitGameobject.GetComponent<OperationUnitData>().destination = 
                new Vector3(unit.unitGameobject.GetComponent<OperationUnitData>().destination.x,
                unit.unitGameobject.GetComponent<OperationUnitData>().destination.y - 0.1f,
                unit.unitGameobject.GetComponent<OperationUnitData>().destination.z);

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


    public OperationUnit GetClickedUnit(GameObject clickedUnit) {

        foreach (var units in unitLocations) {
            foreach (var unit in units.Value)
            {
                if (unit.unitGameobject == clickedUnit)
                    return unit;
            }
        }
        throw new Exception("Unit not found clicked unit: "+clickedUnit.name);
    }

    private void AddUnit(OperationUnit unit, Vector2Int cord, Vector3 worldPosition)
    {
        int units = unitLocations[cord].Count;

        var y = GetUnitElevation(units, worldPosition);

        unit.unitGameobject.GetComponent<OperationUnitData>().destination = new Vector3(worldPosition.x, y, worldPosition.z);
        
        unitLocations[cord].Add(unit);
        unit.hexPosition = cord;

    }

    private void RemoveUnit(OperationUnit unit, Vector2Int moveToCord, Vector3 oldPosition)
    {
        if (!unitLocations.ContainsKey(moveToCord))
        {
            unitLocations.Add(moveToCord, new List<OperationUnit>());
        }

        if (!unitLocations.ContainsKey(unit.hexPosition))
            return;

        Vector2Int cord = unit.hexPosition;
        var units = unitLocations[cord];

        if (units.Contains(unit)) {
            units.Remove(unit);

            for (int i = 0; i < units.Count; i++) {
                units[i].unitGameobject.GetComponent<OperationUnitData>().destination
                    = new Vector3(oldPosition.x, GetUnitElevation(i, oldPosition), oldPosition.z);
            }
            
        }

    }

   

}
