using NUnit.Framework;
using static AircraftLoader;

public class AircraftManueverTests
{



    [Test]
    public void ManueverTests()
    {

        var man = new AircraftCombatManueverTable();

        Assert.AreEqual(0, man.GetValueStandard(1, 9));
        Assert.AreEqual(2, man.GetValueStandard(3, 15));
        Assert.AreEqual(8, man.GetValueStandard(4, 23));

        Assert.AreEqual(0, man.GetValueBvr(1, 9));
        Assert.AreEqual(2, man.GetValueBvr(3, 15));
        Assert.AreEqual(3, man.GetValueBvr(4, 23));

    }


}
