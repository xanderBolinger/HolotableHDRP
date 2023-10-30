using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AircraftDetectionTable
{

    TwoWayTable table;

    public AircraftDetectionTable() {
        TextAsset csvFile = Resources.Load<TextAsset>("Aircraft/Tables/DetectionTable");
        table = new TwoWayTable(csvFile);
    }

    public bool Detected(string detectionClass, int roll) {
        if (roll > 15)
            roll = 15;

        return table.GetValue(detectionClass, roll) == "D";
    }

    

}
