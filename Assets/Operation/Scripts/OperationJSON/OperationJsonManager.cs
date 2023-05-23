using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Operation {
    public class OperationJsonManager : MonoBehaviour
    {

        OperationControlsManager ocm;

        void Start()
        {
            ocm = GetComponent<OperationControlsManager>();
        }

        public void LoadAllUnits() {


            foreach (var unit in ocm.opm.operationUnits)
                LoadSpecificUnit(unit);

        }

        public void LoadSelectedUnit() {
            if (ocm.selectedUnitObject == null) {
                Debug.Log("Select unit to load.");
                return; 
            }

            LoadSpecificUnit(ocm.selectedUnitObject.GetComponent<OperationUnitData>().ou);

        }

        private void LoadSpecificUnit(OperationUnit targetOu) {
            string folderPath = Path.Combine("Assets", "Resources", "OperationUnits");

            string[] filePaths = Directory.GetFiles(folderPath, "*.json");

            foreach (string filePath in filePaths)
            {
                string fileName = Path.GetFileNameWithoutExtension(filePath);

                OperationUnit ou = OperationUnitLoader.LoadJSON(fileName);

                if (ou.unitName != targetOu.unitName) 
                    continue;

                targetOu.SetUnits(ou.GetUnits());
                Debug.Log("Loaded unit: "+targetOu.unitName);
                return;
            }

            Debug.Log("Could not load unit, target ou name not found: "+targetOu.unitName);
        }

    }
}


