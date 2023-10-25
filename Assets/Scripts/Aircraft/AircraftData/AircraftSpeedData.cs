using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AircraftMovementData;
using static AircraftSpeedData;

public class AircraftSpeedData
{

    public enum AircraftSpeed { 
        Combat,Dash,Manuever
    }

    [JsonProperty("very_high")]
    ElevationSpeedData _veryHigh;
    [JsonProperty("high")]
    ElevationSpeedData _high;
    [JsonProperty("medium")]
    ElevationSpeedData _medium;
    [JsonProperty("low")]
    ElevationSpeedData _low;
    [JsonProperty("deck")]
    ElevationSpeedData _deck;

    public int GetSpeed(AircraftSpeed speed, AircraftAltitude altitude) {

        switch (altitude) {
            case AircraftAltitude.VERY_HIGH:
                return _veryHigh.GetSpeed(speed);
            case AircraftAltitude.HIGH:
                return _high.GetSpeed(speed);
            case AircraftAltitude.MEDIUM:
                return _medium.GetSpeed(speed);
            case AircraftAltitude.LOW:
                return _low.GetSpeed(speed);
            case AircraftAltitude.DECK:
                return _deck.GetSpeed(speed);
            default:
                throw new System.Exception("Altitude not found for altitude: "+altitude);
        }

    }

}

class ElevationSpeedData {
    [JsonProperty("combat")]
    int _combat;
    [JsonProperty("dash")]
    int _dash;
    [JsonProperty("manuever")]
    int _manuenver;

    public int combatSpeed { get { return _combat; } }
    public int dashSpeed { get { return _dash; } }
    public int manuenverSpeed { get { return _manuenver; } }

    public int GetSpeed(AircraftSpeed speed)
    {
        switch(speed)
        {
            case AircraftSpeed.Combat:
                return _combat;
            case AircraftSpeed.Dash:
                return _dash;
            case AircraftSpeed.Manuever:
                return _manuenver;
            default:
                throw new System.Exception("Speed not found for speed: " + speed);
        }
    }
}
