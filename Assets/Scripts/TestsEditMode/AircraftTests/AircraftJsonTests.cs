using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using static AircraftSpeedData;
using static AircraftMovementData;
using System.Drawing.Drawing2D;
using static AircraftLoader;
using static AirToAirWeaponType;

public class AircraftJsonTests
{

    [Test]
    public void LoadTest()
    {
        var cord = new GameObject().AddComponent<HexCord>();
        cord.x = 1;
        cord.y = 1;
        var v19 = LoadAircraftJson("V19");
        v19.SetupAircraft("hitman", AircraftSpeed.Combat, AircraftAltitude.VERY_HIGH, cord);
        var md = v19.movementData;
        Assert.AreEqual("hitman", v19.callsign);
        Assert.AreEqual(AircraftSpeed.Combat, md.speed);
        Assert.AreEqual(AircraftAltitude.VERY_HIGH, md.altitude);
        Assert.AreEqual(cord, md.location);
    }

    [Test]
    public void V19DetectionDataTest() {
        var aircraft = LoadAircraftJson("V19");
        var dd = aircraft.aircraftDetectionData;
        var radar = dd.aircraftRadar;

        Assert.AreEqual("C", radar.detectionClass);

        Assert.AreEqual(25,radar.irstRear);
        Assert.AreEqual(18, radar.irstFront);

        Assert.AreEqual(125, radar.radarMaxRangeActive);
        Assert.AreEqual(100, radar.radarMediumRangeActive);
        Assert.AreEqual(90, radar.radarShortRangeActive);

        Assert.AreEqual(75, radar.radarMaxRange);
        Assert.AreEqual(60, radar.radarMediumRange);
        Assert.AreEqual(50, radar.radarShortRange);

    }

    [Test]
    public void ReadV19Test()
    {
        var aircraft = LoadAircraftJson("V19");
        var jd = aircraft.aircraftJammerData;
        var md = aircraft.movementData;
        Assert.IsTrue(AircraftType.V19 == aircraft.aircraftType);
        Assert.AreEqual("V19 Torrent", aircraft.aircraftDisplayName);
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

        Assert.AreEqual(0, jd.aircraftJammer.jammerStrengthNoise);
        Assert.AreEqual(4, jd.aircraftJammer.jammerStrengthDeception);

        Assert.AreEqual(22, jd.aircraftStandoffJammer.shortRange);
        Assert.AreEqual(45, jd.aircraftStandoffJammer.mediumRange);
        Assert.AreEqual(88, jd.aircraftStandoffJammer.longRange);

        Assert.AreEqual(4, jd.aircraftStandoffJammer.shortRangeStrength);
        Assert.AreEqual(3, jd.aircraftStandoffJammer.mediumRangeStrength);
        Assert.AreEqual(2, jd.aircraftStandoffJammer.longRangeStrength);

        Assert.AreEqual(1, md.manueverRating);

        Assert.IsTrue(Aim260Jatm == aircraft.aircraftPayload.pylons[0].weaponType);
        Assert.AreEqual(6, aircraft.aircraftPayload.pylons[0].depletionPoints);

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
