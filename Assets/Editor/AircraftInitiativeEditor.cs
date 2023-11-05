using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(AircraftInitiativeManager))]
public class AircraftInitiativeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        AircraftInitiativeManager aim = (AircraftInitiativeManager)target;

        if (GUILayout.Button("Pull Chit"))
        {
            aim.ActivateFlights();
        }

    }


}
