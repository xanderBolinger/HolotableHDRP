

using Newtonsoft.Json;
using UnityEngine;

public class AircraftLoader
{

    public static Aircraft LoadAirCraft(string aircraftName) {
        TextAsset asset = Resources.Load("Aircraft/" + aircraftName, typeof(TextAsset)) as TextAsset;
        string jsonString = asset.text;
        var aircraft = JsonConvert.DeserializeObject<Aircraft>(jsonString);
        return aircraft;
    }


}
