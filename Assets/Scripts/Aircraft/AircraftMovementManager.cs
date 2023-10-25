using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AircraftMovementData;

public class AircraftMovementManager : MonoBehaviour
{

    public void SetSpeed() { 
    
    }

    public void MoveAircraft(Aircraft aircraft, HexCord hexCord, AircraftAltitude altitude) {

       
        aircraft.movementData.location = hexCord;
        aircraft.movementData.altitude = altitude;
    }

    public bool AircraftCanMove(Aircraft aircraft) {

        if (aircraft.movementData.speed == AircraftSpeedData.AircraftSpeed.Dash
            && aircraft.movementData.currentFuel <= 0) 
            return false;

        return true; 
    
    }



}
