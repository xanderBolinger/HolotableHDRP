using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AircraftMovementData;
using static AircraftSpeedData;

public class AircraftMovementManager : MonoBehaviour
{
    [SerializeField]
    AircraftSpeed testSetAircraftSpeed;
    [SerializeField]
    AircraftAltitude testSetAircraftAltitude;
    [SerializeField]
    int testCordX;
    [SerializeField]
    int testCordY;
    [SerializeField]
    bool testCordRough;

    

    public HexCord CreateTestHexCord(bool roughTerrain, int x, int y) {
        var testSetHexCord = new GameObject().AddComponent<HexCord>();
        testSetHexCord.x = x;
        testSetHexCord.y = y;
        testSetHexCord.hexType = roughTerrain ? HexCord.HexType.MOUNTAIN : HexCord.HexType.Clear;
        return testSetHexCord;
    }

    public void SetSpeed(Aircraft aircraft, AircraftSpeed aircraftSpeed) {
        aircraft.movementData.speed = aircraftSpeed;
    }

    public void MoveAircraft(Aircraft aircraft, HexCord hexCord, AircraftAltitude altitude) {
        if (!AircraftCanMove(aircraft))
            return;

        aircraft.movementData.MoveAircraft(hexCord, altitude);
    }

    public bool AircraftCanMove(Aircraft aircraft) {

        if (aircraft.movementData.speed == AircraftSpeed.Dash
            && aircraft.movementData.currentFuel <= 0) 
            return false;

        return true; 
    
    }

}
