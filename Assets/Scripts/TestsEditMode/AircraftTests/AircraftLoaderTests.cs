using NUnit.Framework;
using UnityEngine;
using static AircraftSpeedData;
using static AircraftMovementData;

public class AircraftLoaderTests
{

    [Test]
    public void LoadTest()
    {
        var aircraftLoader = new AircraftLoader();

        Assert.AreEqual(1, aircraftLoader.LoadedAircraftCount());
    }

}
