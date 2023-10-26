using HexMapper;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AircraftMovementData;
using static AircraftSpeedData;

public class AircraftMovementManager : MonoBehaviour
{
    [SerializeField]
    Direction testAircraftFacing;
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

    public void GetHexDistanceTest(AircraftFlight flight) {
        var cord = flight.GetLocation().GetCord();
        Debug.Log("Hex distance for "+flight.flightCallsign+" from "+cord+" to ("+testCordX+", "+testCordY+"), dist: "
            +HexMap.GetDistance(cord.x, cord.y, testCordX, testCordY));
        
    }

    public void SetAltitudeTest(AircraftFlight flight) {
        SetAltitude(flight, testSetAircraftAltitude);
    }

    public void SetSpeedTest(AircraftFlight flight)
    {
        SetSpeed(flight, testSetAircraftSpeed);
    }

    public void SetFacingTest(AircraftFlight flight)
    {
        SetFacing(flight, testAircraftFacing);
    }

    public void MoveAircraftTest(AircraftFlight flight)
    {
        SetSpeedTest(flight);
        MoveAircraft(flight, CreateTestHexCord(testCordRough, testCordX, testCordY), testSetAircraftAltitude, testAircraftFacing);
    }

    public void SetFacing(AircraftFlight flight, Direction facing)
    {
        foreach (var aircraft in flight.flightAircraft)
            aircraft.movementData.facing = facing;
    }

    public void SetAltitude(AircraftFlight flight, AircraftAltitude altitude) {
        foreach (var aircraft in flight.flightAircraft)
            aircraft.movementData.altitude = altitude;
    }

    public void SetSpeed(AircraftFlight flight, AircraftSpeed aircraftSpeed) {
        foreach(var aircraft in flight.flightAircraft)
            aircraft.movementData.speed = aircraftSpeed;
    }

    public void MoveAircraft(AircraftFlight flight, HexCord hexCord, AircraftAltitude altitude, Direction facing) {
        foreach (var aircraft in flight.flightAircraft) {
            if (!AircraftCanMove(aircraft, hexCord)) {
                Debug.Log("Flight "+flight.flightCallsign+" can't move check distance and fuel.");
                return;
            }
        }

        foreach (var aircraft in flight.flightAircraft)
            aircraft.movementData.MoveAircraft(
                hexCord != null ? hexCord : CreateTestHexCord(testCordRough, testCordX, testCordY),
                altitude, facing);
        Debug.Log("Moved flight "+flight.flightCallsign+" to "+hexCord.GetCord()
            +", Alt: "+altitude.ToString()+", Facing: "+facing);
    }

    private bool AircraftCanMove(Aircraft aircraft, HexCord hexCord) {

        if (aircraft.movementData.speed == AircraftSpeed.Dash
            && aircraft.movementData.currentFuel <= 0)
            return false;

        var speed = aircraft.movementData.GetSpeed();

        if (HexMap.GetDistance(aircraft.movementData.location, hexCord) > speed)
            return false;

        return true; 
    
    }

    public static HexCord CreateTestHexCord(bool roughTerrain, int x, int y)
    {
        var testSetHexCord = new GameObject().AddComponent<HexCord>();
        testSetHexCord.x = x;
        testSetHexCord.y = y;
        testSetHexCord.hexType = roughTerrain ? HexCord.HexType.MOUNTAIN : HexCord.HexType.Clear;
        return testSetHexCord;
    }

}
