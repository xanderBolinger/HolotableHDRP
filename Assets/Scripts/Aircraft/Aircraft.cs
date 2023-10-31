
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using static AircraftLoader;
using static AircraftMovementData;
using static AircraftSpeedData;

public class Aircraft
{
    [JsonProperty("aircraft_type"), JsonConverter(typeof(StringEnumConverter))]
    AircraftType _aircraftType;

    string _callsign;
    [JsonProperty("aircraft_display_name")]
    string _aircraftDisplayName;
    [JsonProperty("aircraft_movement_data")]
    AircraftMovementData _movementData;
    [JsonProperty("aircraft_detection_data")]
    AircraftDetectionData _aircraftDetectionData;
    [JsonProperty("aircraft_jammer_data")]
    AircraftJammerData _aircraftJammerData;

    public string callsign { get { return _callsign; } }
    public string aircraftDisplayName { get { return _aircraftDisplayName; } }
    public AircraftType aircraftType { get { return _aircraftType; } }

    public AircraftMovementData movementData { get { return _movementData; } }
    public AircraftDetectionData aircraftDetectionData { get { return _aircraftDetectionData; } }
    public AircraftJammerData aircraftJammerData { get { return _aircraftJammerData; } }

    public bool damaged;
    public bool crippled;
    public bool destroyed;

    [JsonConstructor]
    public Aircraft() { }

    public Aircraft(Aircraft aircraft) {
        _callsign = aircraft.callsign;
        _aircraftDisplayName = aircraft.aircraftDisplayName;
        _movementData = new AircraftMovementData(aircraft.movementData);
        _aircraftDetectionData = new AircraftDetectionData(aircraft.aircraftDetectionData);
        _aircraftJammerData = new AircraftJammerData(aircraft.aircraftJammerData);
        damaged = aircraft.damaged;
        crippled = aircraft.crippled;
        destroyed = aircraft.destroyed;
    }

    public void SetupAircraft(string callsign, AircraftSpeed speed, AircraftAltitude altitude, HexCord cord) {
        _callsign = callsign;
        _movementData.SetupMovementData(speed, altitude, true, cord);
        
    }

    public override string ToString()
    {
        return _aircraftDisplayName + ": " + _callsign 
            + " fuel: " + _movementData.currentFuel + "/" + _movementData.fuel 
            + ", Destroyed: "+destroyed+", Crippled: "+crippled+", Damaged: "+damaged; 
    }

}
