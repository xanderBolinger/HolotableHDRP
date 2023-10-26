using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AircraftMovementManager))]
public class AircraftMovementManagerGui : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        AircraftMovementManager amm = (AircraftMovementManager)target;

        if (GUILayout.Button("Set Altitude") && Flights())
        {
            amm.SetAltitudeTest(GetFlight());
        }

        if (GUILayout.Button("Set Speed") && Flights())
        {
            amm.SetSpeedTest(GetFlight());
        }

        if (GUILayout.Button("Move Flight") && Flights())
        {
            amm.MoveAircraftTest(GetFlight());
        }

    }

    private bool Flights() {
        return AircraftManager.aircraftManager.aircraftFlights.Count > 0;
    }

    private AircraftFlight GetFlight() {
        var am = AircraftManager.aircraftManager;
        var index = am.selectedAircraftFlightIndex;

        return am.aircraftFlights[index];
    }
}
