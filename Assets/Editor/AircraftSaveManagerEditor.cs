using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AircraftSaveManager))]
public class AircraftSaveManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        AircraftSaveManager sm = (AircraftSaveManager)target;

        if (GUILayout.Button("Save"))
        {
            sm.SaveAircraft();
        }

        if (GUILayout.Button("Load"))
        {
            sm.LoadAircraft();
        }

    }


}
