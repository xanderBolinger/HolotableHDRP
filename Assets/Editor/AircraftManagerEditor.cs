using UnityEditor;
using UnityEngine;
using static AircraftManager;
using static AircraftHexCordManager;
using static AircraftFlightManagerEditor;

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

        if (GUILayout.Button("Remove Fighter from Flight by Callsign"))
        {
            am.RemoveAircraftFromFlight(GetFlight(), aircraftManager.testCreateAircraftCallsign);
        }

        if (GUILayout.Button("Destroy Aircraft"))
        {
            GetFlight().DestroyAircraft(aircraftManager.testCreateAircraftCallsign);
        }

        if (GUILayout.Button("Cripple Aircraft"))
        {
            GetFlight().DestroyAircraft(aircraftManager.testCreateAircraftCallsign);
        }

        if (GUILayout.Button("Damage Aircraft"))
        {
            GetFlight().DestroyAircraft(aircraftManager.testCreateAircraftCallsign);
        }

    }


}
