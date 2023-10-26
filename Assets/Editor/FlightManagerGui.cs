using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FlightManager))]
public class FlightManagerGui : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        FlightManager am = (FlightManager)target;

        GUIContent selectedFlightList = new GUIContent("Select Flight");
        am.selectedAircraftFlightIndex = EditorGUILayout.Popup(selectedFlightList, am.selectedAircraftFlightIndex, am.testAircraftFlightDisplayList.ToArray());

        if (GUILayout.Button("Add flight"))
        {
            am.AddFlight(am.testAddFlightCallsign);
        }

        if (GUILayout.Button("Remove flight") && am.testAircraftFlightDisplayList.Count > 0)
        {
            am.RemoveFlight(am.testAircraftFlightDisplayList[am.selectedAircraftFlightIndex]);
        }

        if (GUILayout.Button("Print Flight") && am.testAircraftFlightDisplayList.Count > 0)
        {
            am.PrintFlight(am.testAircraftFlightDisplayList[am.selectedAircraftFlightIndex]);
        }

        if (GUILayout.Button("Print Flights"))
        {
            am.PrintFlights();
        }
    }
}
