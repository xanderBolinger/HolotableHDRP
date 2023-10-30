using UnityEditor;
using UnityEngine;
using static AircraftManager;
using static AircraftHexCordManager;
[CustomEditor(typeof(HexElevationManager))]
public class HexElevationManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        HexElevationManager manager = (HexElevationManager)target;

        if (GUILayout.Button("Show"))
        {
            manager.ShowElevation();
        }

        if (GUILayout.Button("Hide"))
        {
            manager.HideElevation();
        }

    }
}
