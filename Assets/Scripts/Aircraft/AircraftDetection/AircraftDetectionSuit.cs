using static HexCord;
using System;

public enum AircraftDetectionSuit
{
    
    Heart,Spade,Diamond


    
}

public static class AircraftDetectionSuitMethods
{
    public static AircraftDetectionSuit lastDetectionSuit = AircraftDetectionSuit.Diamond;


    public static void ResetDetectionSuits() {
        lastDetectionSuit = AircraftDetectionSuit.Diamond;
    }

    public static AircraftDetectionSuit GetSuit() {
        lastDetectionSuit = GetNextSuit(lastDetectionSuit);
        return lastDetectionSuit;
    }

    public static AircraftDetectionSuit GetNextSuit(AircraftDetectionSuit suit) {
        return suit switch
        {
            AircraftDetectionSuit.Diamond => AircraftDetectionSuit.Heart,
            AircraftDetectionSuit.Heart => AircraftDetectionSuit.Spade,
            AircraftDetectionSuit.Spade => AircraftDetectionSuit.Diamond,
            _ => throw new Exception("Aircraft detection suit not found for suit: " + suit),
        };
    }
    
}
