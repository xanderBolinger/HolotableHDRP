using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AircraftPayload
{

    [JsonProperty("pylons")]
    List<AirToAirPylon> _pylons;

    public List<AirToAirPylon> pylons { get { return _pylons; } }

    [JsonConstructor]
    public AircraftPayload() { }

    public AircraftPayload(AircraftPayload aircraftPayload) {
        _pylons = new List<AirToAirPylon>();

        foreach (var pylon in aircraftPayload.pylons) {
            _pylons.Add(new AirToAirPylon(pylon));
        }

    }

}
