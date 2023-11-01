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
            var am2 = aircraftManager;
            var callsign = am2.testCreateAircraftCallsign;
            var type = am2.testCreateAircraftType;
            var alt = am2.testStartingAircraftAltitude;
            am.AddAircraftToFlight(GetFlight(), callsign,
                type, alt,
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
            GetFlight().CrippleAircraft(aircraftManager.testCreateAircraftCallsign);
        }

        if (GUILayout.Button("Damage Aircraft"))
        {
            GetFlight().DamageAircraft(aircraftManager.testCreateAircraftCallsign);
        }

    }


}
