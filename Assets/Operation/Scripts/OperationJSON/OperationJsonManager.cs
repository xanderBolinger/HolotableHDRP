using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Operation {
    public class OperationJsonManager : MonoBehaviour
    {

        OperationControlsManager opm;

        void Start()
        {
            opm = GetComponent<OperationControlsManager>();
        }

        public void LoadAllUnits() { 
        
        }

        public void LoadSpecificUnit() {
            string folderPath = Path.Combine("Assets", "Resources", "OperationUnits");

            // Get all the text files in the folder
            string[] filePaths = Directory.GetFiles(folderPath, "*.txt");

            // Iterate over each text file
            foreach (string filePath in filePaths)
            {
                // Read the contents of the text file
                string text = File.ReadAllText(filePath);

                // Process the text file contents (replace with your own logic)
                Debug.Log("Text file found: " + filePath);
                Debug.Log("Text contents: " + text);
            }
        }

    }
}


