using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Operation.OperationUnit;

namespace Operation {
    public class CombatResults
    {

        public static void AdvanceOrRetreat(OperationManager opm, OperationUnit unit, Vector2Int cord, Vector3 startPos, Vector3 endPos)
        {
            unit.spentFreeConflictResultMovement = true;
            opm.gridMover.MoveUnit(unit, cord, startPos, endPos);
        }

        public static void SetUnitStatus(OperationUnit unit, int subUnitIndex, UnitStatus status) {
            unit.GetUnit(subUnitIndex).unitStatus = status;
            unit.UnitStatusUpdate();
        }

    }
}


