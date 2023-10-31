using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using static AircraftDetectionSuit;
using static AircraftDetectionSuitMethods;
using static AircraftLoader;
using static AircraftDetectionCalculator;
using static AircraftSpeedData;
using static AircraftMovementData;


public class AircraftDetectionTests
{
    AircraftMovementManager aircraftMovementManager;
    AircraftFlightManager aircraftFlightManager;
    AircraftFlight flight;
    AircraftFlight opforFlight;
    List<AircraftFlight> flights;
    ForceSideManager fsm;
    HexCord cord1;
    HexCord cord2;

    [SetUp]
    public void Setup() {
        cord1 = new GameObject().AddComponent<HexCord>();
        cord1.x = 0;
        cord1.y = 0;
        cord2 = new GameObject().AddComponent<HexCord>();
        cord2.x = 1;
        cord2.y = 1;
        ResetDetectionSuits();
        fsm = new ForceSideManager();
        fsm.Setup();
        AircraftLoader al = new AircraftLoader();
        var v1 = al.LoadAircraft(AircraftType.V19);
        v1.SetupAircraft("v1", AircraftSpeed.Combat, AircraftAltitude.VERY_HIGH, cord1);

        flight = new AircraftFlight("testflight");
        flight.AddAircraft(v1);
        flights = new List<AircraftFlight>();
        flights.Add(flight);

        opforFlight = new AircraftFlight("testflight2");
        var v2 = al.LoadAircraft(AircraftType.V19);
        v2.SetupAircraft("v1", AircraftSpeed.Combat, AircraftAltitude.VERY_HIGH, cord2);
        opforFlight.AddAircraft(v2);
        flights.Add(opforFlight);
        aircraftFlightManager = new GameObject().AddComponent<AircraftFlightManager>();
        aircraftMovementManager = new GameObject().AddComponent<AircraftMovementManager>();
        aircraftFlightManager.Setup();
        aircraftFlightManager.AddFlight("t", null);
        aircraftFlightManager.AddFlight("t2", null);
        aircraftFlightManager.aircraftFlights[0] = flight;
        aircraftFlightManager.aircraftFlights[1] = opforFlight;
    }

    [Test]
    public void RadarTest() {
        DiceRoller.AddNextTestValue(10);
        Assert.AreEqual(8, RadarDetectionRoll(flight, opforFlight, 10, false));
    }

    [Test]
    public void VisualDetectionTests() {

        DiceRoller.AddNextTestValue(10);

        Assert.AreEqual(8, VisualDetectionRoll(true, 2, flight, opforFlight));

        aircraftMovementManager.MoveAircraft(flight, cord2, AircraftAltitude.HIGH, HexMapper.Direction.B);
        
        DiceRoller.AddNextTestValue(10);

        Assert.AreEqual(7, VisualDetectionRoll(true, 2, flight, opforFlight));

        DiceRoller.AddNextTestValue(10);

        opforFlight.flightAircraft[0].movementData.speed = AircraftSpeed.Dash;

        Assert.AreEqual(9, VisualDetectionRoll(true, 2, flight, opforFlight));

        DiceRoller.AddNextTestValue(10);

        Assert.AreEqual(9, VisualDetectionRoll(false, 2, flight, opforFlight));

        DiceRoller.AddNextTestValue(10);

        aircraftMovementManager.MoveAircraft(flight, cord2, AircraftAltitude.VERY_HIGH, HexMapper.Direction.B);

        Assert.AreEqual(10, VisualDetectionRoll(false, 2, flight, opforFlight));

        DiceRoller.AddNextTestValue(10);

        Assert.AreEqual(8, VisualDetectionRoll(false, 4, flight, opforFlight));

    }

    [Test]
    public void TrackTableRoll() {
        var trackTable = new AircraftTrackTable();
        Assert.AreEqual("hsd", trackTable.GetTrackResult("A",2));
        Assert.AreEqual("s", trackTable.GetTrackResult("C", 14));
        Assert.AreEqual("", trackTable.GetTrackResult("F", 18));
    }

    [Test]
    public void FlightDetectionSuit() {
        Assert.IsTrue(flight.GetDetectionSuit() == Heart);
    }

    [Test]
    public void UndetectFlight() {
        AircraftDetectionManager.DetectFlight(flight);

        Assert.AreEqual(Heart, flight.GetDetectionSuit());
        AircraftTrackTable.ApplyTrackResult("h", flights);
        Assert.IsFalse(flight.flightAircraft[0].aircraftDetectionData.detected);

        AircraftDetectionManager.DetectFlight(flight);
        AircraftTrackTable.ApplyTrackResult("hb", flights);
        Assert.IsTrue(flight.flightAircraft[0].aircraftDetectionData.detected);
    }

    [Test]
    public void DetectFlight() {
        AircraftDetectionManager.DetectFlight(flight);
        Assert.IsTrue(flight.flightAircraft[0].aircraftDetectionData.detected);
    }

    [Test]
    public void DetectionTableTests() { 
        var dt = new AircraftDetectionTable();

        Assert.IsFalse(dt.Detected("A", 1));
        Assert.IsTrue(dt.Detected("A", 15));
        Assert.IsTrue(dt.Detected("C", 11));
        Assert.IsFalse(dt.Detected("D", 11));
        Assert.IsTrue(dt.Detected("G", 20));
        Assert.IsFalse(dt.Detected("G", 14));

    }


    [Test]
    public void GetDetectionSuitTests() {
        ResetDetectionSuits();
        Assert.IsTrue(GetSuit() == Heart);
        Assert.IsTrue(GetSuit() == Spade);
        Assert.IsTrue(GetSuit() == Diamond);

    }


    [Test]
    public void DetectionSuitTests()
    {
        var suit = Heart;
        Assert.IsTrue(Heart == suit);
        suit = GetNextSuit(suit);
        Assert.IsTrue(Spade == suit);
        suit = GetNextSuit(suit);
        Assert.IsTrue(Diamond == suit);
        suit = GetNextSuit(suit);
        Assert.IsTrue(Heart == suit);

    }


}
