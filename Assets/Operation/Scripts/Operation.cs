using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Operation.TimeSegment;

namespace Operation {

    public class Operation : MonoBehaviour
    {
        public int startTime = 5; 

        [HideInInspector]
        public List<TimeSegment> timeSegments;
        [HideInInspector]
        public List<OperationUnit> operationUnits;

        private TimeSegment currentTimeSegment;

        public void CreateOperation() {

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

            if (nextTS.timeUnit != currentTimeSegment.timeUnit)
                NewTU();

            currentTimeSegment = nextTS;
        }

        private void NewTU() {
            foreach (var unit in operationUnits) {
                unit.spentFreeConflictResultMovement = false;
                unit.moveType = OperationUnit.MoveType.NONE;
                unit.spentMP = 0;
            }
        }

        private TimeSegment GetNextTS() {
            TimeSegment nextTS = null;

            if (currentTimeSegment.hour == timeSegments.Count)
            {
                nextTS = timeSegments[0];
                return nextTS;
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



