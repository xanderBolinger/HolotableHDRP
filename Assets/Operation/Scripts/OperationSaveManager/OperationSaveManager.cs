using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


namespace Operation {
    public class OperationSaveManager
    {
        private const string OperationSavesFolder = "OperationSaves";

        public static void SaveOperation(OperationSaveData data, string fileName)
        {
            // Get the path to the "OperationSaves" folder inside the "Assets" directory
            string folderPath = Path.Combine(Application.dataPath, OperationSavesFolder);

            // Create the directory if it doesn't exist
            Directory.CreateDirectory(folderPath);

            // Serialize the data to a binary file
            string filePath = Path.Combine(folderPath, fileName);
            using (FileStream fileStream = File.Open(filePath, FileMode.OpenOrCreate))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fileStream, data);
            }

            Debug.Log("Operation data saved: " + filePath);
        }

        public static OperationSaveData LoadOperation(string fileName)
        {
            // Get the path to the "OperationSaves" folder inside the "Assets" directory
            string folderPath = Path.Combine(Application.dataPath, OperationSavesFolder);

            // Check if the file exists
            string filePath = Path.Combine(folderPath, fileName);
            if (File.Exists(filePath))
            {
                // Deserialize the data from the binary file
                using (FileStream fileStream = File.Open(filePath, FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    OperationSaveData data = (OperationSaveData)formatter.Deserialize(fileStream);
                    Debug.Log("Operation data loaded: " + filePath);
                    return data;
                }
            }
            else
            {
                Debug.LogWarning("Operation data file not found: " + filePath);
                return null;
            }
        }
    }
}


