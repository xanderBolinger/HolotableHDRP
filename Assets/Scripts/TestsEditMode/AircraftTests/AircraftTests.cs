using static AircraftSpeedData;
using static AircraftMovementData;
using NUnit.Framework;
using UnityEngine;
using static AircraftLoader;

public class AircraftTests
{
    AircraftMovementManager aircraftMovementManager;
    FlightManager flightManager;
    AircraftFlight flight;
    HexCord cord1;
    HexCord cord2;

    [SetUp]
    public void Setup()
    {
        cord1 = new GameObject().AddComponent<HexCord>();
        cord1.x = 0;
        cord1.y = 0;
        cord2 = new GameObject().AddComponent<HexCord>();
        cord2.x = 1;
        cord2.y = 1;
        var fmObj = new GameObject();
        flightManager = fmObj.AddComponent<FlightManager>();
        var aircraftManager = fmObj.AddComponent<AircraftManager>();
        aircraftManager.Setup();
        aircraftMovementManager = new GameObject().AddComponent<AircraftMovementManager>();
        flightManager.Setup();
        flightManager.AddFlight("testflight", null);
        flight = flightManager.aircraftFlights[0];
        aircraftManager.AddAircraftToFlight(flight, "testaircraft", AircraftType.V19, AircraftAltitude.VERY_HIGH, cord1);
        aircraftMovementManager.MoveAircraft(flight, cord2, AircraftAltitude.HIGH, HexMapper.Direction.B);
    }

    [Test]
    public void CopyTest()
    {
        var actual = flight.flightAircraft[0];
        Aircraft copy = new Aircraft(actual);

        Assert.IsTrue(copy != actual);
        Assert.AreEqual(copy.callsign, actual.callsign);
        Assert.IsTrue(copy.aircraftDisplayName == actual.aircraftDisplayName);
        Assert.IsTrue(copy.movementData.facing == actual.movementData.facing);
        Assert.IsTrue(copy.movementData.speed == actual.movementData.speed);
        Assert.IsTrue(copy.movementData.altitude == actual.movementData.altitude);
        Assert.IsTrue(copy.movementData.location == actual.movementData.location);
        Assert.IsTrue(copy.movementData.isLaden == actual.movementData.isLaden);
        Assert.IsTrue(copy.movementData.fuel == actual.movementData.fuel);
        Assert.IsTrue(copy.movementData.currentFuel == actual.movementData.currentFuel);
        Assert.IsTrue(copy.movementData.GetSpeed() == actual.movementData.GetSpeed());
    }


}
