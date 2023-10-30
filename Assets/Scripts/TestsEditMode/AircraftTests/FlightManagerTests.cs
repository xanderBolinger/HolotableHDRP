using NUnit.Framework;
using UnityEngine;

public class FlightManagerTests
{

    AircraftFlightManager manager;

    [SetUp]
    public void Setup() {
        manager = new GameObject().AddComponent<AircraftFlightManager>();
        manager.Setup();
        manager.AddFlight("testflight", null);
    }

    [Test]
    public void AddFlightTests()
    {
        Assert.AreEqual("testflight", manager.aircraftFlights[0].flightCallsign);
    }

    [Test]
    public void DuplicateFlightTest() {
        manager.AddFlight("testflight", null);
        Assert.AreEqual(1, manager.aircraftFlights.Count);
    }

    [Test]
    public void RemoveFlightTests() {
        manager.RemoveFlight("testflight");
        Assert.AreEqual(0, manager.aircraftFlights.Count);

        manager.RemoveFlight("testflight");
        Assert.AreEqual(0, manager.aircraftFlights.Count);
    }


}
