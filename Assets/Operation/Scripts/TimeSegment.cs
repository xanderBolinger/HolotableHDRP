using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Operation {
    public class TimeSegment
    {
        public enum TimeUnit { 
            AM,PM,NIGHT
        }

        public int hour;
        public TimeUnit timeUnit;
        public Dictionary<OperationUnit, List<Vector2Int>> plannedMovement;

        List<string> records;


        public TimeSegment(int hour, TimeUnit timeUnit) {
            this.hour = hour;
            this.timeUnit = timeUnit;

            records = new List<string>();
            plannedMovement = new Dictionary<OperationUnit, List<Vector2Int>>();

        }



    }
}

