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

        GUIContent selectedFlightList = new GUIContent("Select Flight");
        amm.selectedAircraftFlightIndex = EditorGUILayout.Popup(selectedFlightList, amm.selectedAircraftFlightIndex, amm.testAircraftFlightDisplayList.ToArray());

        /*if (GUILayout.Button("Enter Combat"))
        {
            ocm.EnterCombat();
        }*/

    }
}
