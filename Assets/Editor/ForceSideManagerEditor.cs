using UnityEditor;
using UnityEngine;
using static AircraftManager;
using static AircraftHexCordManager;
[CustomEditor(typeof(ForceSideManager))]
public class ForceSideManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ForceSideManager forceSideManager = (ForceSideManager)target;

        GUIContent selectedFlightList = new GUIContent("Select Side");
        forceSideManager.inspectorSelectedSideIndex = EditorGUILayout.Popup(selectedFlightList, forceSideManager.inspectorSelectedSideIndex,
            forceSideManager.inspectorSideDisplayList.ToArray());

        if (GUILayout.Button("Print Sides"))
        {
            forceSideManager.PrintSides();
        }
    }
}
