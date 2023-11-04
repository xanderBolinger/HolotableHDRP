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

            if (GUILayout.Button("Create Game"))
            {
                ocm.CreateGame();
            }

            if (GUILayout.Button("Advance Segment"))
            {
                ocm.AdvanceTimeSegment();
            }

            if (GUILayout.Button("Set Unit Move Type"))
            {
                ocm.SetUnitMoveType();
            }

            if (GUILayout.Button("Set Sub-unit Status"))
            {
                ocm.SetUnitStatus();
            }

            if (GUILayout.Button("Undo"))
            {
                ocm.opm.undoRedo.Undo();
            }

            if (GUILayout.Button("Redo"))
            {
                ocm.opm.undoRedo.Redo();
            }

            if (GUILayout.Button("Add Turn"))
            {
                ocm.opm.undoRedo.AddTurn();
            }

            if (GUILayout.Button("Load Units JSON"))
            {
                ocm.opjsm.LoadAllUnits();
            }

            if (GUILayout.Button("Load Selected Unit JSON"))
            {
                ocm.opjsm.LoadSelectedUnit();
            }


            if (GUILayout.Button("Save Operation"))
            {
                ocm.opm.SaveOperation();
            }

            if (GUILayout.Button("Load Operation"))
            {
                ocm.opm.LoadOperation();
            }
        }

    }
}


