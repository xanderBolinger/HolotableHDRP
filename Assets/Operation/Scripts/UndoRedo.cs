using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Operation {
    
    public class UndoRedo
    {
        public List<Turn> turns;
        public int currentTurn = 0;

        private OperationManager opm;

        public UndoRedo(OperationManager opm) {
            this.opm = opm;
            turns = new List<Turn>();
        }

        public void AddTurn() {
            turns.Add(new Turn(opm.operationUnits, opm.currentTimeSegment));
            currentTurn = turns.Count - 1;
        }

        public void Undo() {
            currentTurn--;
            if (currentTurn < 0) {
                Debug.Log("Already on first turn, no more turns to undo.");
                currentTurn = 0;
            }
            UpdateOpm();
        }

        public void Redo() {
            currentTurn++;
            if (currentTurn > turns.Count - 1) {
                currentTurn = turns.Count - 1;
                Debug.Log("Already on last turn, no more turns to redo.");
            }
            UpdateOpm();
        }

        private void UpdateOpm() {

            var turn = turns[currentTurn];

            foreach (var unit in turn.operationUnits) {

                // that unit has been destroyed
                if (unit == null) {

                    continue;
                }

                opm.gridMover.MoveUnit(unit, unit.hexPosition, unit.unitGameobject.transform.position, 
                    opm.hexes[unit.hexPosition.x][unit.hexPosition.y].transform.position, 
                    opm.hexCords[unit.hexPosition.x][unit.hexPosition.y].hexType == HexCord.HexType.CLEAR );
            }

            opm.currentTimeSegment = turn.currentTimeSegment;
            opm.operationUnits = turn.operationUnits;

        }


    }

    public class Turn {
        public List<OperationUnit> operationUnits;
        public TimeSegment currentTimeSegment;

        public Turn(List<OperationUnit> oldOperationUnits, TimeSegment currentTimeSegment)
        {
            this.operationUnits = new List<OperationUnit>();

            foreach (var oldUnit in oldOperationUnits) {
                operationUnits.Add(new OperationUnit(oldUnit));
            }


            //timeSegments = new List<TimeSegment>();
            //operationUnits = new List<OperationUnit>();
            //this.operationUnits = operationUnits;
            this.currentTimeSegment = currentTimeSegment;
        }
    }

}


