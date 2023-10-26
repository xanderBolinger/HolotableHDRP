
using Newtonsoft.Json;
using static AircraftMovementData;
using static AircraftSpeedData;

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

    public void SetupAircraft(string callsign, AircraftSpeed speed, AircraftAltitude altitude) {
        _callsign = callsign;
        _movementData.SetupMovementData(speed, altitude, true);
    }

    public override string ToString()
    {
        return _aircraftName + ": " + _callsign 
            + " fuel: " + _movementData.currentFuel + "/" + _movementData.fuel; ;
    }

}
