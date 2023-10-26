using UnityEditor;
using UnityEngine;
using static AircraftManager;
using static AircraftHexCordManager;
using static FlightManagerEditor;

[CustomEditor(typeof(AircraftManager))]
public class AircraftManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        AircraftManager am = (AircraftManager)target;

        if (GUILayout.Button("Add Fighter to Flight"))
        {
            am.AddAircraftToFlight(GetFlight(), aircraftManager.testCreateAircraftCallsign,
                aircraftManager.testCreateAircraftType, aircraftManager.testStartingAircraftAltitude,
                aircraftHexCordManager.TestCreateHexCord());
        }

    }


}
