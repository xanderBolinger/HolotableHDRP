using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using static AircraftSpeedData;
using static AircraftMovementData;
using System.Drawing.Drawing2D;

public class AircraftJsonTests
{

    [Test]
    public void LoadTest()
    {
        var cord = new GameObject().AddComponent<HexCord>();
        cord.x = 1;
        cord.y = 1;
        var v19 = AircraftLoader.LoadAircraftJson("V19");
        v19.SetupAircraft("hitman", AircraftSpeed.Combat, AircraftAltitude.VERY_HIGH, cord);
        var md = v19.movementData;
        Assert.AreEqual("hitman", v19.callsign);
        Assert.AreEqual(AircraftSpeed.Combat, md.speed);
        Assert.AreEqual(AircraftAltitude.VERY_HIGH, md.altitude);
        Assert.AreEqual(cord, md.location);
    }

    [Test]
    public void ReadV19Test()
    {
        var aircraft = AircraftLoader.LoadAircraftJson("V19");
        var md = aircraft.movementData;
        Assert.AreEqual("V19 Torrent", aircraft.aircraftName);
        Assert.AreEqual(10, md.fuel);
        Assert.AreEqual(0, md.currentFuel);

        TestSpeed(md, 4, 7, 7, false, 
            AircraftAltitude.VERY_HIGH);
        TestSpeed(md, 4, 7, 7, false,
           AircraftAltitude.HIGH);
        TestSpeed(md, 4, 6, 7, false,
           AircraftAltitude.MEDIUM);
        TestSpeed(md, 4, 5, 6, false,
           AircraftAltitude.LOW);
        TestSpeed(md, 4, 5, 6, false,
           AircraftAltitude.DECK);

        TestSpeed(md, 4, 5, 5, true,
            AircraftAltitude.VERY_HIGH);
        TestSpeed(md, 4, 5, 5, true,
           AircraftAltitude.HIGH);
        TestSpeed(md, 3, 4, 5, true,
           AircraftAltitude.MEDIUM);
        TestSpeed(md, 3, 4, 4, true,
           AircraftAltitude.LOW);
        TestSpeed(md, 3, 4, 4, true,
           AircraftAltitude.DECK);
    }

    private void TestSpeed(AircraftMovementData md,
        int cmbt, int dash, int mnvr, bool laden, 
        AircraftAltitude altitude) {
        Assert.AreEqual(cmbt,
            md.GetSpeed(AircraftSpeed.Combat,
            altitude, laden));
        Assert.AreEqual(dash,
            md.GetSpeed(AircraftSpeed.Dash,
            altitude, laden));
        Assert.AreEqual(mnvr,
            md.GetSpeed(AircraftSpeed.Manuever,
            altitude, laden));
    }

}
