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
        flightManager.selectedAircraftFlightIndex = EditorGUILayout.Popup(selectedFlightList, 
            flightManager.selectedAircraftFlightIndex, flightManager.testAircraftFlightDisplayList.ToArray());

        GUIContent targetFlightList = new GUIContent("Target Flight");
        flightManager.selectedTargetAircraftFlightIndex = EditorGUILayout.Popup(targetFlightList,
            flightManager.selectedTargetAircraftFlightIndex, flightManager.testTargetAircraftFlightDisplayList.ToArray());

        if (GUILayout.Button("Add flight"))
        {
            flightManager.AddFlight(flightManager.testAddFlightCallsign, 
                ForceSideManager.forceSideManager.GetInspectorSelectedSide(),
                flightManager.inspectorFlightQuality);
        }

        if (GUILayout.Button("Remove flight") && flightManager.testAircraftFlightDisplayList.Count > 0)
        {
            flightManager.RemoveFlight(flightManager.testAircraftFlightDisplayList[flightManager.selectedAircraftFlightIndex]);
        }

        if (GUILayout.Button("Print Selected Flight") && flightManager.testAircraftFlightDisplayList.Count > 0)
        {
            flightManager.PrintFlight(flightManager.testAircraftFlightDisplayList[flightManager.selectedAircraftFlightIndex]);
        }

        if (GUILayout.Button("Print All Flights"))
        {
            flightManager.PrintFlights();
        }

        if (GUILayout.Button("Toggle Disengaging") && Flights())
        {
            GetFlight().disengaing = !GetFlight().disengaing;
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

    public static AircraftFlight GetTargetFlight()
    {
        var am = AircraftFlightManager.aircraftFlightManager;
        var index = am.selectedTargetAircraftFlightIndex;

        return am.aircraftFlights[index];
    }
}
