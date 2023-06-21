using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Operation.OperationUnit;
using static Operation.TimeSegment;

namespace Operation {

    public class OperationManager : MonoBehaviour
    {
        public int startTime = 5;

        public string operationName;

        [HideInInspector]
        public List<TimeSegment> timeSegments;
        [HideInInspector]
        public List<OperationUnit> operationUnits;
        [HideInInspector]
        public TimeSegment currentTimeSegment;
        [HideInInspector]
        public GridMover gridMover;
        [HideInInspector]
        public List<List<GameObject>> hexes;
        [HideInInspector]
        public List<List<HexCord>> hexCords;
        [HideInInspector]
        public UndoRedo undoRedo;

        private int day = 1;

        public void Start()
        {
            var mover = GetComponent<GridMover>();
            if (mover != null)
                gridMover = mover;
            undoRedo = new UndoRedo(this);
        }

        public void SetHexes(List<List<GameObject>> hexes) {
            this.hexes = hexes;
            hexCords = new List<List<HexCord>>();

            for (int row = 0; row < hexes.Count; row++) {

                List<HexCord> hexCordRow = new List<HexCord>();

                for (int col = 0; col < hexes[0].Count; col++) {
                    var hex = hexes[row][col];
                    if (hex.GetComponent<HexCord>() != null)
                    {
                        hexCordRow.Add(hex.GetComponent<HexCord>());
                    }
                    else {
                        hexCordRow.Add(hex.GetComponentInChildren<HexCord>());
                    }
                }

                hexCords.Add(hexCordRow);
            }

        }

        public void SaveOperation()
        {
            var saveData = OperationSaveRunner.GetOperationSaveData(hexCords, operationUnits, currentTimeSegment.hour);
            OperationSaveManager.SaveOperation(saveData, operationName);
        }

        public void LoadOperation()
        {
            CreateOperation();
            var saveData = OperationSaveManager.LoadOperation(operationName);
            operationUnits = saveData.operationUnits;
            currentTimeSegment = timeSegments[saveData.startTime];
            
            var mapGenerator = GameObject.Find("MapGenerator").GetComponent<MapGenerator>();
            mapGenerator.ClearMap();
            List<List<GameObject>> hexPrefabs = new List<List<GameObject>>();
            foreach (var rowTypes in saveData.hexes)
            {
                List<GameObject> row = new List<GameObject>();

                foreach (var colTypes in rowTypes)
                {
                    row.Add(HexMap.GetPrefab(colTypes));
                }
                hexPrefabs.Add(row);
            }
            mapGenerator.InstantiateHexes(hexPrefabs, hexPrefabs[0].Count, hexPrefabs.Count);
            SetHexes(mapGenerator.hexes);

            foreach (var unit in operationUnits) {
                RecreateUnitGameObject(unit);
                gridMover.MoveUnit(unit, unit.hexPosition, new Vector3(0, 0, 0), hexes[unit.x][unit.y].transform.position,
                    hexCords[unit.x][unit.y].hexType == HexCord.HexType.CLEAR);
            }

        }

        private void DestroyOperationUnitObjects() {
            if (operationUnits == null)
                return;

            foreach (var unit in operationUnits)
                if (unit.unitGameobject != null)
                    Destroy(unit.unitGameobject);
        }

        public void CreateOperation() {

            DestroyOperationUnitObjects();

            timeSegments = new List<TimeSegment>();
            operationUnits = new List<OperationUnit>();

            for (int i = 1; i <= 24; i++) {

                TimeUnit timeUnit = TimeUnit.AM;

                if (i >= 19 && i <= 4)
                {
                    timeUnit = TimeUnit.NIGHT;
                }
                else if (i >= 12 && i < 19) {
                    timeUnit = TimeUnit.PM;
                }

                var ts = new TimeSegment(i, timeUnit);
                timeSegments.Add(ts);

                if (i == startTime)
                    currentTimeSegment = ts;
            }

            operationUnits = new List<OperationUnit>();
        }

        public void AddOU(OperationUnit ou) {
            operationUnits.Add(ou);
        }

        internal void RecreateUnitGameObject(OperationUnit unit)
        {
            var ocm = GetComponent<OperationControlsManager>();
            GameObject newUnitObject = unit.side == Side.BLUFOR ? Instantiate(ocm.bluforPrefab) : Instantiate(ocm.opforPrefab);
            newUnitObject.transform.position = new Vector3(0, 0, 0);
            var ouData = newUnitObject.GetComponent<OperationUnitData>();
            ouData.ou = unit;
            unit.unitGameobject = newUnitObject;
            unit.unitGameobject.transform.position = hexes[0][0].transform.position;
            unit.hexPosition = new Vector2Int(unit.x, unit.y);
        }

        public void DeleteUnit(GameObject obj) {
            Destroy(obj);
        }

        public void RemoveOU(OperationUnit ou) {
            operationUnits.Remove(ou);
        }

        public void RemoveOU(int i)
        {
            operationUnits.RemoveAt(i);
        }

        public void RemoveOU(string name)
        {
            var ou = FindOU(name);
            operationUnits.Remove(ou);
        }

        public OperationUnit FindOU(string name) {

            foreach (var unit in operationUnits) {
                if (unit.unitName == name)
                    return unit;
            }

            throw new System.Exception("Could not find OU for name: "+name);
        } 

        public void AdvanceTS() {

            var nextTS = GetNextTS();

            OperationMovement.MoveUnits(this, currentTimeSegment, gridMover);
            var conflicts = OperationMovement.GetConflicts(this);

            foreach (var c in conflicts) {

                string output = "Conflict, Aggressor: " + c.aggressor.unitName;
                output += ", Targets: ";

                foreach (var target in c.targets) {
                    output += target.unitName + ", ";
                }

                Debug.Log(output);

            }

            if (nextTS.hour == startTime)
                day++;

            if (nextTS.timeUnit != currentTimeSegment.timeUnit)
                NewTU();
            NewTS();

            currentTimeSegment.plannedMovement.Clear();
            currentTimeSegment = nextTS;
            undoRedo.AddTurn();
            Debug.Log("Day: "+day+", Time Segment: "+currentTimeSegment.hour+" "+currentTimeSegment.timeUnit);
        }

        private void NewTS() {
            foreach (var unit in operationUnits) {
                unit.spentMPTS = 0;
            }
        }

        private void NewTU() {
            foreach (var unit in operationUnits) {
                unit.spentFreeConflictResultMovement = false;
                unit.moveType = OperationUnit.MoveType.NONE;
                unit.spentMPTU = 0;
                unit.tacticalMovement = 0;
            }
        }

        private TimeSegment GetNextTS() {

            if (currentTimeSegment.hour == timeSegments.Count)
            {
                return timeSegments[0];
            }

            foreach (var ts in timeSegments)
            {
                if (ts.hour == currentTimeSegment.hour + 1)
                    return ts;
            }


            throw new System.Exception("Next TS not found, current ts hour: "+currentTimeSegment.hour);
        }


    }
}



