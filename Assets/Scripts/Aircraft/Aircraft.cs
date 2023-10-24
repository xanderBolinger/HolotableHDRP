
using Newtonsoft.Json;

public class Aircraft
{
    string _callsign;
    [JsonProperty("aircraft_name")]
    string _aircraftName;
    [JsonProperty("aircraft_movement_data")]
    AircraftMovementData _movementData;

    public string callsign { get { return _callsign; } }
    public string aircraftName { get { return _aircraftName; } }
    public AircraftMovementData movementData { get { return _movementData; } }

}
