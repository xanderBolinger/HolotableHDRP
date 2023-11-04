using Newtonsoft.Json;
using System;
using System.Collections.Generic;

[Serializable]
public class AircraftPayload
{

    [JsonProperty("air_to_air_pylons")]
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
