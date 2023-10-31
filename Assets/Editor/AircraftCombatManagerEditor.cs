using UnityEditor;
using UnityEngine;
using static AircraftManager;
using static AircraftHexCordManager;
using static AircraftFlightManagerEditor;

[CustomEditor(typeof(AircraftCombatManager))]
public class AircraftCombatManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        AircraftCombatManager cm = (AircraftCombatManager)target;

        if (GUILayout.Button("Standard Air to Air"))
        {
            cm.StandardAirToAir(GetFlight(), GetTargetFlight());    
        }


    }


}
