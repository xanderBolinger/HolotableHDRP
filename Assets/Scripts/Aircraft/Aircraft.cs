
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

    public string callsign { get { return _callsign; } }
    public string aircraftDisplayName { get { return _aircraftDisplayName; } }
    public AircraftType aircraftType { get { return _aircraftType; } }
    public AircraftMovementData movementData { get { return _movementData; } }

    [JsonConstructor]
    public Aircraft() { }

    public Aircraft(Aircraft aircraft) {

        this._callsign = aircraft.callsign;
        this._aircraftDisplayName = aircraft.aircraftDisplayName;
        this._movementData = new AircraftMovementData(aircraft.movementData);
    }

    public void SetupAircraft(string callsign, AircraftSpeed speed, AircraftAltitude altitude, HexCord cord) {
        _callsign = callsign;
        _movementData.SetupMovementData(speed, altitude, true, cord);
        
    }

    public override string ToString()
    {
        return _aircraftDisplayName + ": " + _callsign 
            + " fuel: " + _movementData.currentFuel + "/" + _movementData.fuel; ;
    }

}
