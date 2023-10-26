using NUnit.Framework;

public class AircraftFlightTests
{



    [Test]
    public void CreateFlight()
    {

        var v191 = AircraftLoader.LoadAircraftJson("V19");
        var v192 = AircraftLoader.LoadAircraftJson("V19");
        var v193 = AircraftLoader.LoadAircraftJson("V19");

        var flight = new AircraftFlight("testflight");
        flight.AddAircraft(v191);
        flight.AddAircraft(v192);
        flight.AddAircraft(v193);
        Assert.AreEqual("testflight", flight.flightCallsign);
        Assert.AreEqual(3, flight.flightAircraft.Count);

    }


}
