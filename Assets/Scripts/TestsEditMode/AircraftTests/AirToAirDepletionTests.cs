using NUnit.Framework;
using static AirToAirWeaponType;
using static AircraftLoader;
using static AirToAirDepletionCalculator;
using static AirToAirCombatCalculator;
using UnityEngine;

public class AirToAirDepletionTests
{

    AircraftAirToAirCombatManager aircraftCombatManager;

    AircraftFlight flight;

    [SetUp]
    public void SetUp() {

        aircraftCombatManager = new GameObject().AddComponent<AircraftAirToAirCombatManager>();
        aircraftCombatManager.SetUp();

        AircraftLoader al = new AircraftLoader();
        var v1 = al.LoadAircraft(AircraftType.V19);
        v1.SetupAircraft("v1", AircraftSpeedData.AircraftSpeed.Combat, AircraftMovementData.AircraftAltitude.VERY_HIGH, null);

        flight = new AircraftFlight("testflight");
        flight.AddAircraft(v1);

    }

    [Test]
    public void SetDepletedFlightTest() {
        DepleteFlightTest(flight, GetPylon(flight, false, null));
        DepleteFlightTest(flight, GetPylon(flight, false, null));
        Assert.IsNull(GetPylon(flight, false, null));
    }

    [Test]
    public void DepletionCheckTest() {

        DiceRoller.AddNextTestValue(9);

        DepletionCheck(flight, 3, GetPylon(flight, false, null), false);

        Assert.IsNotNull(GetPylon(flight, false, null));

        DiceRoller.AddNextTestValue(8);

        DepletionCheck(flight, 3, GetPylon(flight, false, null), false);

        Assert.IsNull(GetPylon(flight, false, null));

    }

}

