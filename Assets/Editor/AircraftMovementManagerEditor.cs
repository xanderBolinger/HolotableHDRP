using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static AircraftFlightManagerEditor;

[CustomEditor(typeof(AircraftMovementManager))]
public class AircraftMovementManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        AircraftMovementManager amm = (AircraftMovementManager)target;


        if (GUILayout.Button("Get Distance Between Flights") && Flights())
        {
            amm.GetDistanceBetweenTwoFlightsTest(GetFlight(), GetTargetFlight());
        }

        if (GUILayout.Button("Get Distance to Hex") && Flights())
        {
            amm.GetHexDistanceTest(GetFlight());
        }

        if (GUILayout.Button("Set Altitude") && Flights())
        {
            amm.SetAltitudeTest(GetFlight());
        }

        if (GUILayout.Button("Set Speed") && Flights())
        {
            amm.SetSpeedTest(GetFlight());
        }

        if (GUILayout.Button("Set Facing") && Flights())
        {
            amm.SetFacingTest(GetFlight());
        }

        if (GUILayout.Button("Set Location") && Flights())
        {
            var flight = GetFlight();
            var cord = AircraftHexCordManager.aircraftHexCordManager.TestCreateHexCord();
            foreach (var aircraft in flight.flightAircraft)
                aircraft.movementData.location = cord;
            Debug.Log("Set flight location " + flight.flightCallsign + " to " + cord.GetCord());
        }

        if (GUILayout.Button("Move Flight") && Flights())
        {
            amm.MoveAircraftTest(GetFlight());
        }

        

    }

    
}
