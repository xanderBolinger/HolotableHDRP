using static AircraftSpeedData;
using static AircraftMovementData;
using NUnit.Framework;
using UnityEngine;

public class AircraftMovementTests
{
    AircraftMovementManager aircraftMovementManager;
    AircraftManager aircraftManager;
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
        aircraftManager = new GameObject().AddComponent<AircraftManager>();
        aircraftMovementManager = new GameObject().AddComponent<AircraftMovementManager>();
        aircraftManager.Setup();
        aircraftManager.AddFlight("testflight");
        flight = aircraftManager.aircraftFlights[0];
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

        aircraftMovementManager.MoveAircraft(flight, cord2, AircraftAltitude.HIGH);

        Assert.IsTrue(cord2 == flight.GetLocation());
        Assert.IsTrue(AircraftAltitude.HIGH == flight.GetAltitude());
    }


}
