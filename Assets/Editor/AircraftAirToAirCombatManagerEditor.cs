using UnityEditor;
using UnityEngine;
using static AircraftManager;
using static AircraftHexCordManager;
using static AircraftFlightManagerEditor;

[CustomEditor(typeof(AircraftAirToAirCombatManager))]
public class AircraftAirToAirCombatManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        AircraftAirToAirCombatManager cm = (AircraftAirToAirCombatManager)target;

        if (GUILayout.Button("Standard Air to Air"))
        {
            cm.StandardAirToAir(GetFlight(), GetTargetFlight());    
        }

        if (GUILayout.Button("Print Weapons"))
        {
            Debug.Log(cm.weaponLoader.ToString());
        }
    }


}
