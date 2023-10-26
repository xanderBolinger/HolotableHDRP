using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AircraftManager))]
public class AircraftManagerGui : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        AircraftManager am = (AircraftManager)target;

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
