using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AircraftTrackTable
{

    TwoWayTable table;

    public AircraftTrackTable()
    {
        TextAsset csvFile = Resources.Load<TextAsset>("Aircraft/Tables/DetectionTrackTable");
        table = new TwoWayTable(csvFile);
    }

    public string GetTrackResult(string detectionClass, int roll)
    {
        return table.GetValue(detectionClass, roll);
    }

    public static void ApplyTrackResult(string result, List<AircraftFlight> flights) {

        bool braces = result.Contains('b');
        bool heart = result.Contains('h');
        bool spade = result.Contains('s');
        bool diamond = result.Contains('d');

        foreach (var flight in flights) {
            if (braces && flight.GetAltitude() != AircraftMovementData.AircraftAltitude.DECK)
                continue;

            if (flight.GetDetectionSuit() == AircraftDetectionSuit.Heart && heart) {
                Undetect(flight);
            } else if (flight.GetDetectionSuit() == AircraftDetectionSuit.Spade && spade) {
                Undetect(flight);
            } else if (flight.GetDetectionSuit() == AircraftDetectionSuit.Diamond && diamond) {
                Undetect(flight);
            }

        }

    }

    private static void Undetect(AircraftFlight flight) {
        foreach (var aircraft in flight.flightAircraft)
            aircraft.aircraftDetectionData.detected = false;
    }

}
