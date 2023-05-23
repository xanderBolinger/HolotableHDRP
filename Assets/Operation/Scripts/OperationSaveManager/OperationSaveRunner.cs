using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HexCord;

namespace Operation {
    public class OperationSaveRunner
    {

        public static OperationSaveData GetOperationSaveData(List<List<HexCord>> hexCords, List<OperationUnit> operationUnits, int startTime) {
            return new OperationSaveData(hexCords, operationUnits, startTime);
        }

    }

    [Serializable]
    public class OperationSaveData {

        public List<List<HexType>> hexes;
        public List<OperationUnit> operationUnits;
        public int startTime;

        public OperationSaveData(List<List<HexCord>> hexCords, List<OperationUnit> operationUnits, int startTime) {
            hexes = new List<List<HexType>>();
            this.operationUnits = operationUnits;
            this.startTime = startTime;

            foreach (var row in hexCords) {

                var newRow = new List<HexType>();

                foreach (var column in row) {
                    newRow.Add(column.hexType);
                }

                hexes.Add(newRow);
            }

        }
        

    }
}


