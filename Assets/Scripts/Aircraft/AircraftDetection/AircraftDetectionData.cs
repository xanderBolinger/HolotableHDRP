using Newtonsoft.Json;
using static AircraftDetectionSuitMethods;
public class AircraftDetectionData
{

    [JsonProperty("aircraft_radar")]
    AircraftRadar _radar;

    AircraftDetectionSuit _detectionSuit;

    public bool detected;

    public AircraftRadar aircraftRadar { get { return _radar; } }

    public AircraftDetectionSuit detectionSuit { get { return _detectionSuit; } }

    [JsonConstructor]
    public AircraftDetectionData() {
        _detectionSuit = GetSuit();
        detected = false;
    }

}
