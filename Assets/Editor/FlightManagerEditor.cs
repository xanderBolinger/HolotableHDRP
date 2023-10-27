using UnityEditor;
using UnityEngine;
using static AircraftManager;
using static AircraftHexCordManager;
[CustomEditor(typeof(FlightManager))]
public class FlightManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        FlightManager flightManager = (FlightManager)target;

        GUIContent selectedFlightList = new GUIContent("Select Flight");
        flightManager.selectedAircraftFlightIndex = EditorGUILayout.Popup(selectedFlightList, flightManager.selectedAircraftFlightIndex, flightManager.testAircraftFlightDisplayList.ToArray());

        if (GUILayout.Button("Add flight"))
        {
            flightManager.AddFlight(flightManager.testAddFlightCallsign, );
        }

        if (GUILayout.Button("Remove flight") && flightManager.testAircraftFlightDisplayList.Count > 0)
        {
            flightManager.RemoveFlight(flightManager.testAircraftFlightDisplayList[flightManager.selectedAircraftFlightIndex]);
        }

        if (GUILayout.Button("Print Flight") && flightManager.testAircraftFlightDisplayList.Count > 0)
        {
            flightManager.PrintFlight(flightManager.testAircraftFlightDisplayList[flightManager.selectedAircraftFlightIndex]);
        }

        if (GUILayout.Button("Print Flights"))
        {
            flightManager.PrintFlights();
        }
    }

    public static bool Flights()
    {
        return FlightManager.flightManager.aircraftFlights.Count > 0;
    }

    public static AircraftFlight GetFlight()
    {
        var am = FlightManager.flightManager;
        var index = am.selectedAircraftFlightIndex;

        return am.aircraftFlights[index];
    }
}
