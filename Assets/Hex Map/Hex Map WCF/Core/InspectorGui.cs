using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PerlinGenerator))]
public class InspectorGui : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PerlinGenerator myScript = (PerlinGenerator)target;

        if (GUILayout.Button("Run WFC")) {
            myScript.RunWFC();
        }
        if (GUILayout.Button("Refresh Input Grid"))
        {
            myScript.SetInputReader();
        }
        if (GUILayout.Button("Create Tilemap"))
        {
            myScript.CreateTileMap();
        }

    }
}
