using NUnit.Framework;
using static AircraftLoader;

public class AircraftFlightTests
{



    [Test]
    public void CreateFlight()
    {

        AircraftLoader al = new AircraftLoader();
        var v1 = al.LoadAircraft(AircraftType.V19);
        v1.SetupAircraft("v1", AircraftSpeedData.AircraftSpeed.Combat, AircraftMovementData.AircraftAltitude.VERY_HIGH, null);

        var flight = new AircraftFlight("testflight");
        flight.AddAircraft(v1);
        Assert.AreEqual("testflight", flight.flightCallsign);
        Assert.AreEqual(1, flight.flightAircraft.Count);

    }


}
