using HexMapper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AircraftDetectionManager : MonoBehaviour
{
    private enum DetectionClass { 
        B
    }

    AircraftTrackTable _trackTable;
    AircraftDetectionTable _detectionTable;
    DetectionClass _detectionClass = DetectionClass.B;

    public bool night;

    private void Awake()
    {
        SetUp();
    }

    public void SetUp() {
        _trackTable = new AircraftTrackTable();
        _detectionTable = new AircraftDetectionTable();
    }

    public void DetectFlights(List<AircraftFlight> flights) {

        Debug.Log("Detect flights: ");

        foreach (var flight in flights) {

            if (flight.DisorderdOrAborted()) {
                Debug.Log("Flight " + flight.flightCallsign+" cannot spot, flight disordered or aborted");
                continue;
            }

            foreach (var possibleTarget in flights) {
                if (flight.side.FriendlyTowards(possibleTarget.side)
                    || possibleTarget.Detected())
                    continue;

                Detect(flight, possibleTarget);
            }

        }

    }

    private void Detect(AircraftFlight spotter, AircraftFlight target) {
        var dist = AircraftUtility.Distance(spotter, target);

        Debug.Log("Flight "+spotter.flightCallsign+" attempts to spot "+target.flightCallsign);

        // Visual 
        Visual(spotter, target, dist);

        // IRST 
        Irst(spotter, target, dist);

        // Radar
        Radar(spotter, target, dist);
    }

    private void Radar(AircraftFlight spotter, AircraftFlight target, int dist) {
        var radar = spotter.flightAircraft[0].aircraftDetectionData.aircraftRadar;
        var targetRadar = target.flightAircraft[0].aircraftDetectionData.aircraftRadar;
        if (!radar.active || dist > (targetRadar.active ? radar.radarMaxRangeActive : radar.radarMaxRange))
            return;

        var roll = AircraftDetectionCalculator.RadarDetectionRoll(spotter, target, dist, false);
        var detected = _detectionTable.Detected(radar.detectionClass, roll);

        Debug.Log("Radar detection "+ radar.detectionClass+", "+roll+" Rslt: "+(detected?"DETECTED":"-"));
        if (detected) {
            DetectFlight(target);
        }
    }

    private void Irst(AircraftFlight spotter, AircraftFlight target, int dist) {
        var locationSpotter = spotter.GetLocation();
        var locationTarget = target.GetLocation();
        var rear = HexDirection.Rear(new Vector2Int(locationSpotter.x, locationSpotter.y),
            new Vector2Int(locationTarget.x, locationTarget.y),
            target.GetFacing());

        var roll = AircraftDetectionCalculator.IrstDetectionRoll(spotter, target, rear);
        var detected = _detectionTable.Detected("E", roll);
        Debug.Log("IRST detection E, " + roll + " Rslt: " + (detected ? "DETECTED" : "-"));
        if (detected)
            DetectFlight(target);

    }

    private void Visual(AircraftFlight spotter, AircraftFlight target, int dist) {
        
        if ((night && dist > 2 ) || dist > 4)
            return;

        var roll = AircraftDetectionCalculator.VisualDetectionRoll(night, dist, spotter, target);
        var detected = _detectionTable.Detected("D", roll);
        Debug.Log("Visual detection E, " + roll + " Rslt: " + (detected ? "DETECTED" : "-"));
        if (detected)
            DetectFlight(target);

    }

    public static void DetectFlight(AircraftFlight flight)
    {
        foreach (var aircraft in flight.flightAircraft)
            aircraft.aircraftDetectionData.detected = true;
    }

    public void UndetectFlights(List<AircraftFlight> flights) {

        int roll = DiceRoller.Roll(2, 20);

        var rslt = _trackTable.GetTrackResult(_detectionClass.ToString(), roll);

        Debug.Log("Track Results: "+rslt);

        AircraftTrackTable.ApplyTrackResult(rslt, flights);

    }

}
