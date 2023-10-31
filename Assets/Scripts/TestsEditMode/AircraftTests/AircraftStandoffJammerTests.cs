using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using static AircraftDetectionSuitMethods;
using static AircraftLoader;
using static AircraftMovementData;
using static AircraftSpeedData;
using static AircraftStandoffJammerCalculator;


public class AircraftStandoffJammerTests
{
    AircraftFlight flight;
    AircraftFlight opforFlight;
    List<AircraftFlight> flights;
    HexCord cord1;
    HexCord cord2;
    HexCord cord3;

    [SetUp]
    public void Setup()
    {
        cord1 = new GameObject().AddComponent<HexCord>();
        cord1.x = 0;
        cord1.y = 0;
        cord2 = new GameObject().AddComponent<HexCord>();
        cord2.x = 10;
        cord2.y = 10;
        cord3 = new GameObject().AddComponent<HexCord>();
        cord3.x = 1;
        cord3.y = 1;
        ResetDetectionSuits();
        
        AircraftLoader al = new AircraftLoader();
        var v1 = al.LoadAircraft(AircraftType.V19);
        v1.SetupAircraft("v1", AircraftSpeed.Combat, AircraftAltitude.VERY_HIGH, cord3);

        flight = new AircraftFlight("testflight");
        flight.AddAircraft(v1);
        flights = new List<AircraftFlight>();
        flights.Add(flight);

        opforFlight = new AircraftFlight("testflight2");
        var v2 = al.LoadAircraft(AircraftType.V19);
        v2.SetupAircraft("v1", AircraftSpeed.Combat, AircraftAltitude.VERY_HIGH, cord2);
        opforFlight.AddAircraft(v2);
        flights.Add(opforFlight);
    }

    [Test]
    public void StandoffJammerTest() {
        var jc = opforFlight.jammerControls;
        jc.ToggleSojs();

        Assert.IsTrue(jc.SojActive());

        jc.SetSojFacing(HexMapper.Direction.FA);

       // Assert.AreEqual(-2, GetJammerStrength(flight, cord1, flights));
        Assert.AreEqual(-4, GetJammerStrength(flight, cord2, flights));
    }

}
