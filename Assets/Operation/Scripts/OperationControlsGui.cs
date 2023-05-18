using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Operation {
    [CustomEditor(typeof(OperationControlsManager))]
    public class OperationControlsGui : Editor
    {

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            OperationControlsManager ocm = (OperationControlsManager)target;

            /*if (GUILayout.Button("Run WFC"))
            {
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
            }*/

        }

    }
}


