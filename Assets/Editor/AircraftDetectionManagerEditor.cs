using UnityEditor;
using UnityEngine;
using static AircraftManager;
using static AircraftHexCordManager;
using static AircraftFlightManagerEditor;

[CustomEditor(typeof(AircraftDetectionManager))]
public class AircraftDetectionManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        AircraftDetectionManager adm = (AircraftDetectionManager)target;

        if (GUILayout.Button("Detect Flights"))
        {
            adm.DetectFlights(AircraftFlightManager.flightManager.aircraftFlights);
        }

        if (GUILayout.Button("Track Flights"))
        {
            adm.UndetectFlights(AircraftFlightManager.flightManager.aircraftFlights);
        }

        if (GUILayout.Button("Toggle Radar") && Flights())
        {
            var flight = GetFlight();
            flight.ToggleRadar();
            Debug.Log("Toggle Radar: ");
            Debug.Log(flight.ToString());
        }

    }


}
