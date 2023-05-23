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
                return;
            }
            UpdateOpm();
        }

        public void Redo() {
            currentTurn++;
            if (currentTurn > turns.Count - 1) {
                currentTurn = turns.Count - 1;
                Debug.Log("Already on last turn, no more turns to redo.");
                return;
            }
            UpdateOpm();
        }

        private void UpdateOpm() {

            var turn = turns[currentTurn];

            opm.operationUnits.Clear();

            foreach (var unit in turn.operationUnits)
                opm.operationUnits.Add(new OperationUnit(unit));

            foreach (var unit in opm.operationUnits) {

                // that unit has been destroyed
                if (unit == null) {

                    continue;
                }

                /*var pos = unit.unitGameobject.transform.position;
                
                pos.y -= 0.1f;

                opm.gridMover.MoveUnit(unit, unit.hexPosition, pos, 
                    opm.hexes[unit.hexPosition.x][unit.hexPosition.y].transform.position, 
                    opm.hexCords[unit.hexPosition.x][unit.hexPosition.y].hexType == HexCord.HexType.CLEAR );*/
                //if(turn.positions.ContainsKey(unit))
                var hex = opm.hexes[unit.hexPosition.x][unit.hexPosition.y];
                var pos = hex.transform.position;
                var hexCord = HexCord.GetHexCord(hex);
                int units = opm.gridMover.unitLocations[hexCord.GetCord()].Count;
                pos.y = opm.gridMover.GetUnitElevation(units, pos) - (hexCord.hexType == HexCord.HexType.CLEAR ? 0.1f : 0f);
                unit.unitGameobject.GetComponent<OperationUnitData>().ou = unit;
                unit.unitGameobject.GetComponent<OperationUnitData>().destination = pos;

                unit.spentMPTS = 0;
            }

            opm.currentTimeSegment = turn.currentTimeSegment;

            //opm.operationUnits = turn.operationUnits;
            

        }


    }

    public class Turn {
        public List<OperationUnit> operationUnits;
        public TimeSegment currentTimeSegment;
        public Dictionary<string, Vector3> positions;

        public Turn(List<OperationUnit> oldOperationUnits, TimeSegment currentTimeSegment)
        {
            this.operationUnits = new List<OperationUnit>();
            positions = new Dictionary<string, Vector3>();

            foreach (var oldUnit in oldOperationUnits) {
                var newUnit = new OperationUnit(oldUnit);
                operationUnits.Add(newUnit);
                positions.Add(newUnit.unitName, newUnit.unitGameobject.transform.position);
            }


            //timeSegments = new List<TimeSegment>();
            //operationUnits = new List<OperationUnit>();
            //this.operationUnits = operationUnits;
            this.currentTimeSegment = currentTimeSegment;
        }
    }

}


