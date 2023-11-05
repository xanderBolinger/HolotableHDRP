using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AircraftMovementData;
using static AircraftSpeedData;

[Serializable]
public class AircraftSpeedData
{
    [Serializable]
    public enum AircraftSpeed { 
        Combat,Dash
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

    public int GetManuverRating(AircraftAltitude altitude) {
        switch (altitude)
        {
            case AircraftAltitude.VERY_HIGH:
                return _veryHigh.manuenverRating;
            case AircraftAltitude.HIGH:
                return _high.manuenverRating;
            case AircraftAltitude.MEDIUM:
                return _medium.manuenverRating;
            case AircraftAltitude.LOW:
                return _low.manuenverRating;
            case AircraftAltitude.DECK:
                return _deck.manuenverRating;
            default:
                throw new System.Exception("Altitude not found for altitude: " + altitude);
        }
    }

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
[Serializable]
class ElevationSpeedData {
    [JsonProperty("combat")]
    int _combat;
    [JsonProperty("dash")]
    int _dash;
    [JsonProperty("manuever")]
    int _manuenver;

    public int combatSpeed { get { return _combat; } }
    public int dashSpeed { get { return _dash; } }
    public int manuenverRating { get { return _manuenver; } }

    public int GetSpeed(AircraftSpeed speed)
    {
        switch(speed)
        {
            case AircraftSpeed.Combat:
                return _combat;
            case AircraftSpeed.Dash:
                return _dash;
            default:
                throw new System.Exception("Speed not found for speed: " + speed);
        }
    }
}
