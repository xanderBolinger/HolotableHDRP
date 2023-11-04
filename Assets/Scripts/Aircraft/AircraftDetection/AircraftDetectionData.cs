using Newtonsoft.Json;
using System;

[Serializable]
public class AircraftDetectionData
{

    [JsonProperty("aircraft_radar")]
    AircraftRadar _radar;

    public bool detected;

    public AircraftRadar aircraftRadar { get { return _radar; } }

    [JsonConstructor]
    public AircraftDetectionData() {
        detected = false;
    }

    public AircraftDetectionData(AircraftDetectionData data) {
        detected = data.detected;
        _radar = new AircraftRadar(data.aircraftRadar);
    }

}
