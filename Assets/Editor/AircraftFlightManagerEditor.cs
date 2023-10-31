using UnityEditor;
using UnityEngine;
using static AircraftManager;
using static AircraftHexCordManager;
[CustomEditor(typeof(AircraftFlightManager))]
public class AircraftFlightManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        AircraftFlightManager flightManager = (AircraftFlightManager)target;

        GUIContent selectedFlightList = new GUIContent("Select Flight");
        flightManager.selectedAircraftFlightIndex = EditorGUILayout.Popup(selectedFlightList, flightManager.selectedAircraftFlightIndex, flightManager.testAircraftFlightDisplayList.ToArray());

        if (GUILayout.Button("Add flight"))
        {
            flightManager.AddFlight(flightManager.testAddFlightCallsign, ForceSideManager.forceSideManager.GetInspectorSelectedSide());
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
        return AircraftFlightManager.aircraftFlightManager.aircraftFlights.Count > 0;
    }

    public static AircraftFlight GetFlight()
    {
        var am = AircraftFlightManager.aircraftFlightManager;
        var index = am.selectedAircraftFlightIndex;

        return am.aircraftFlights[index];
    }
}
