using static AircraftSpeedData;
using static AircraftMovementData;
using NUnit.Framework;
using UnityEngine;
using static AircraftLoader;

public class AircraftMovementTests
{
    AircraftMovementManager aircraftMovementManager;
    FlightManager flightManager;
    AircraftFlight flight;
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
        var fmObj = new GameObject();
        flightManager = fmObj.AddComponent<FlightManager>();
        var aircraftManager = fmObj.AddComponent<AircraftManager>();
        aircraftManager.Setup();
        aircraftMovementManager = new GameObject().AddComponent<AircraftMovementManager>();
        flightManager.Setup();
        flightManager.AddFlight("testflight", null);
        flight = flightManager.aircraftFlights[0];
        aircraftManager.AddAircraftToFlight(flight, "testaircraft", AircraftType.V19, AircraftAltitude.VERY_HIGH, cord1);
    }

    [Test]
    public void SpeedTest() {
        Assert.IsTrue(AircraftSpeed.Combat == flight.GetSpeed());
    }

    [Test]
    public void AltitudeTest() {
        Assert.IsTrue(AircraftAltitude.VERY_HIGH == flight.GetAltitude());
    }

    [Test]
    public void MoveTest()
    {

        Assert.IsTrue(cord1 == flight.GetLocation());

        aircraftMovementManager.MoveAircraft(flight, cord2, AircraftAltitude.HIGH, HexMapper.Direction.B);

        Assert.IsTrue(cord2 == flight.GetLocation());
        Assert.IsTrue(AircraftAltitude.HIGH == flight.GetAltitude());
        Assert.IsTrue(HexMapper.Direction.B == flight.GetFacing());
    }


}
