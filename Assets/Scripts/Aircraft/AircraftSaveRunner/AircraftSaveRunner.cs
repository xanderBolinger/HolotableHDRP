using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class AircraftSaveRunner : MonoBehaviour
{
    private const string AircraftSavesFolder = "AircraftSaves";

    public static void SaveAircraft(AircraftSaveData data, string fileName)
    {
        // Get the path to the "OperationSaves" folder inside the "Assets" directory
        string folderPath = Path.Combine(Application.dataPath, AircraftSavesFolder);

        // Create the directory if it doesn't exist
        Directory.CreateDirectory(folderPath);

        // Serialize the data to a binary file
        string filePath = Path.Combine(folderPath, fileName);
        using (FileStream fileStream = File.Open(filePath, FileMode.OpenOrCreate))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(fileStream, data);
        }

        Debug.Log("Aircraft data saved: " + filePath);
    }

    public static AircraftSaveData LoadAircraft(string fileName)
    {
        // Get the path to the "OperationSaves" folder inside the "Assets" directory
        string folderPath = Path.Combine(Application.dataPath, AircraftSavesFolder);

        // Check if the file exists
        string filePath = Path.Combine(folderPath, fileName);
        if (File.Exists(filePath))
        {
            // Deserialize the data from the binary file
            using (FileStream fileStream = File.Open(filePath, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                AircraftSaveData data = (AircraftSaveData)formatter.Deserialize(fileStream);
                Debug.Log("Aircraft data loaded: " + filePath);
                return data;
            }
        }
        else
        {
            Debug.LogWarning("Aircraft data file not found: " + filePath);
            return null;
        }
    }


}
