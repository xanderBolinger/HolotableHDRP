using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static FlightManagerGui;

[CustomEditor(typeof(AircraftMovementManager))]
public class AircraftMovementManagerGui : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        AircraftMovementManager amm = (AircraftMovementManager)target;

        if (GUILayout.Button("Get Distance") && Flights())
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

        if (GUILayout.Button("Move Flight") && Flights())
        {
            amm.MoveAircraftTest(GetFlight());
        }

    }

    
}
