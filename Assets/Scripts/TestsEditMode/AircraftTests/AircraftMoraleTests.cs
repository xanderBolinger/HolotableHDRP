using NUnit.Framework;
using UnityEngine;
using static AircraftLoader;
using static AirToAirMoraleCalculator;

public class AircraftMoraleTests
{

    AircraftFlight flight;

    [SetUp]
    public void SetUp() {
        AircraftLoader al = new AircraftLoader();
        var v1 = al.LoadAircraft(AircraftType.V19);
        v1.SetupAircraft("v1", AircraftSpeedData.AircraftSpeed.Combat, AircraftMovementData.AircraftAltitude.VERY_HIGH, null);

        flight = new AircraftFlight("testflight");
        flight.AddAircraft(v1);
    }

    [Test]
    public void MoraleCheck_WithHighModifiedRoll_ShouldJettison()
    {
        DiceRoller.AddNextTestValue(20); // Set the dice roll value to ensure the desired result

        MoraleCheckStandard(flight, hasSurprise: true, disadvantage: false, enemyCasualties: 0, friendlyCasualties: 0);

        Assert.AreEqual(0, flight.agressionValue);
    }

    [Test]
    public void MoraleCheck_WithLowModifiedRoll_ShouldAbort()
    {
        DiceRoller.AddNextTestValue(5); // Set the dice roll value to ensure the desired result

        MoraleCheckStandard(flight, hasSurprise: false, disadvantage: false, enemyCasualties: 2, friendlyCasualties: 3);

        Assert.AreEqual(AircraftFlight.FlightStatus.Aborted, flight.flightStatus);
        Assert.AreEqual(-3, flight.agressionValue);
    }

    [Test]
    public void MoraleCheck_WithDisorderAndAgressionDecrement_ShouldUpdateAgression()
    {
        DiceRoller.AddNextTestValue(10); // Set the dice roll value to ensure the desired result

        MoraleCheckStandard(flight, hasSurprise: false, disadvantage: true, enemyCasualties: 1, friendlyCasualties: 0);

        Assert.AreEqual(AircraftFlight.FlightStatus.Disordered, flight.flightStatus);
        Assert.AreEqual(-1, flight.agressionValue);
    }

} 
