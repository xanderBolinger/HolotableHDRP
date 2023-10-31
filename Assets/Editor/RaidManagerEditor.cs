using UnityEditor;
using UnityEngine;
using static AircraftManager;
using static AircraftHexCordManager;
using static AircraftFlightManagerEditor;

[CustomEditor(typeof(RaidManager))]
public class RaidManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        RaidManager rm = (RaidManager)target;

        if (GUILayout.Button("Next Turn"))
        {
            rm.NextTurn();
        }

    }


}
