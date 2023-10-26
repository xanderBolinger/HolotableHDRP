using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AircraftLoader
{
    public enum AircraftType { 
        V19
    }

    List<Aircraft> _loadedAircraft;

    public AircraftLoader() {
        LoadAllAircraft();
    }



    private void LoadAllAircraft() {
        _loadedAircraft = new List<Aircraft>();

        foreach (AircraftType aType in Enum.GetValues(typeof(AircraftType))) {
            var name = GetAircraftName(aType);
            var aircraft = LoadAircraftJson(name);
            _loadedAircraft.Add(aircraft);
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
        return _loadedAircraft.Count;
    }

}
