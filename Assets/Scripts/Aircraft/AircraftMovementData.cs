
using Newtonsoft.Json;
using static AircraftSpeedData;

public class AircraftMovementData
{
    public enum AircraftAltitude { 
        VERY_HIGH,HIGH,MEDIUM,LOW,DECK
    }

    [JsonProperty("max_fuel")]
    int _fuel;
    int _currentFuel;
    
    public AircraftAltitude altitude;
    public HexCord location;

    [JsonProperty("laden")]
    AircraftSpeedData _ladenSpeed;
    [JsonProperty("not_laden")]
    AircraftSpeedData _regularSpeed;

    public int fuel { get { return _fuel; } }
    public int currentFuel { get { return _currentFuel; } }

    public int GetSpeed(AircraftSpeed speed, AircraftAltitude altitude, bool laden) {
        return laden ? _ladenSpeed.GetSpeed(speed, altitude) : _regularSpeed.GetSpeed(speed, altitude);
    }

}
