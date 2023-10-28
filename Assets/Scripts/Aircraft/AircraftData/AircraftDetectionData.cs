using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AircraftDetectionSuit;
using static AircraftDetectionSuitMethods;
public class AircraftDetectionData
{


    AircraftDetectionSuit _detectionSuit;

    public bool detected;

    public AircraftDetectionSuit detectionSuit { get { return _detectionSuit; } }


    public AircraftDetectionData() {
        _detectionSuit = GetSuit();
        detected = false;
    }



}
