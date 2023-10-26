using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AircraftLoader
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum AircraftType { 
        V19
    }

    List<Aircraft> _loadedAircrafts;

    public AircraftLoader() {
        LoadAllAircraft();
    }

    public Aircraft LoadAircraft(AircraftType aircraftType) {

        foreach (var loadedAircraft in _loadedAircrafts) {
            if (loadedAircraft.aircraftType == aircraftType) {
                return new Aircraft(loadedAircraft);
            }
        }

        throw new Exception("Aircraft not found in loaded aircraft for type: "+aircraftType);
        
    }

    private void LoadAllAircraft() {
        _loadedAircrafts = new List<Aircraft>();

        foreach (AircraftType aType in Enum.GetValues(typeof(AircraftType))) {
            var name = GetAircraftName(aType);
            var aircraft = LoadAircraftJson(name);
            _loadedAircrafts.Add(aircraft);
        }
    }

    private static string GetAircraftName(AircraftType aircraftType)
    {
        return aircraftType switch
        {
            AircraftType.V19 => "V19",
            _ => throw new Exception("Aircraft not found for type: " + aircraftType),
        };
    }

    public static Aircraft LoadAircraftJson(string aircraftName) {
        TextAsset asset = Resources.Load("Aircraft/" + aircraftName, typeof(TextAsset)) as TextAsset;
        string jsonString = asset.text;
        var aircraft = JsonConvert.DeserializeObject<Aircraft>(jsonString);
        return aircraft;
    }


    // The bellow methods are used exclusively for testing 
    public int LoadedAircraftCount() {
        return _loadedAircrafts.Count;
    }

}
