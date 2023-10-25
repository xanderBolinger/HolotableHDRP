using static AircraftSpeedData;
using static AircraftMovementData;
using NUnit.Framework;

public class AircraftMovementTests
{

    Aircraft v19;
    AircraftMovementData v19md;
    [SetUp]
    public void Setup() { 
        v19 = AircraftLoader.LoadAirCraft("V19");
        v19.SetupAircraft("hitman", AircraftSpeed.Combat, AircraftAltitude.VERY_HIGH);
    }


    [Test]
    public void CanMoveTest()
    {


        
    }


}
