using System;
using HexMapper;
using Newtonsoft.Json;
using static AircraftSpeedData;

[Serializable]
public class AircraftMovementData
{
    [Serializable]
    public enum AircraftAltitude { 
        VERY_HIGH,HIGH,MEDIUM,LOW,DECK
    }

    [JsonProperty("max_fuel")]
    int _fuel;
    int _currentFuel;

    [JsonProperty("laden")]
    AircraftSpeedData _ladenSpeed;
    [JsonProperty("not_laden")]
    AircraftSpeedData _regularSpeed;


    public Direction facing;
    public AircraftSpeed speed;
    public AircraftAltitude altitude;
    [NonSerialized]
    public HexCord location;
    public bool isLaden;

    public int fuel { get { return _fuel; } }
    public int currentFuel { get { return _currentFuel; } }

    [JsonConstructor]
    public AircraftMovementData() { }

    public AircraftMovementData(AircraftMovementData amd) {
        _fuel = amd.fuel;
        _currentFuel = amd.currentFuel;
        _ladenSpeed = amd._ladenSpeed;
        _regularSpeed = amd._regularSpeed;
        facing = amd.facing;
        speed = amd.speed;
        altitude = amd.altitude;
        location = amd.location;
        isLaden = amd.isLaden;
    }

    public void SetupMovementData(AircraftSpeed speed, AircraftAltitude altitude, bool refuel, HexCord location)
    {
        this.speed = speed;
        this.altitude = altitude;
        this.location = location;
        if (refuel)
            _currentFuel = fuel;
    }

    public int GetSpeed() {
        return GetSpeed(speed, altitude, isLaden);
    }

    public int GetManueverRating()
    {
        return GetManueverRating(altitude, isLaden);
    }

    public int GetManueverRating(AircraftAltitude altitude, bool laden)
    {
        return laden ? _ladenSpeed.GetManuverRating(altitude) : _regularSpeed.GetManuverRating(altitude);
    }

    public int GetSpeed(AircraftSpeed speed, AircraftAltitude altitude, bool laden) {
        return laden ? _ladenSpeed.GetSpeed(speed, altitude) : _regularSpeed.GetSpeed(speed, altitude);
    }

    public void MoveAircraft(HexCord location, AircraftAltitude altitude, Direction facing) {
        this.altitude = altitude;
        this.location = location;
        this.facing = facing;

    }

    public void SpendFuel() {
        _currentFuel--;
    }
    

}
