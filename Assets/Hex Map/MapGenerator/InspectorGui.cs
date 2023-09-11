using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapGenerator))]
public class InspectorGui : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MapGenerator myScript = (MapGenerator)target;

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

        if (GUILayout.Button("Save Tilemap"))
        {
            myScript.SaveTilemap();
        }

        if (GUILayout.Button("Export Json Tilemap"))
        {
            MapExporter.ExportMap();
        }


    }
}


[CustomEditor(typeof(UrbanGenerator))]
public class UrbanGeneratorGui : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        UrbanGenerator myScript = (UrbanGenerator)target;

        if (GUILayout.Button("Generate"))
        {
            myScript.Generate();
        }



    }
}
