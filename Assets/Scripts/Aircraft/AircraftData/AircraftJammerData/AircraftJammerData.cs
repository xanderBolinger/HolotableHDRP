using Newtonsoft.Json;
using System;
using UnityEngine;

[Serializable]
public class AircraftJammerData
{

    [JsonProperty("aircraft_jammer")]
    AircraftJammer _aircraftJammer;
    [JsonProperty("aircraft_standoff_jammer")]
    AircraftStandoffJammer _aircraftStandoffJammer;

    public AircraftJammer aircraftJammer { get { return _aircraftJammer; } }
    public AircraftStandoffJammer aircraftStandoffJammer { get { return _aircraftStandoffJammer; } }

    [JsonConstructor]
    public AircraftJammerData() { }

    public AircraftJammerData(AircraftJammerData aircraftJammerData) {
        _aircraftJammer = new AircraftJammer(aircraftJammerData.aircraftJammer);
        _aircraftStandoffJammer = new AircraftStandoffJammer(aircraftJammerData.aircraftStandoffJammer);
    }

}
