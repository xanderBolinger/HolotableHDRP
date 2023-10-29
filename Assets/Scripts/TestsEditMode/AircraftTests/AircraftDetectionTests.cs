using NUnit.Framework;
using static AircraftDetectionSuit;
using static AircraftDetectionSuitMethods;

public class AircraftDetectionTests
{

    [Test]
    public void DetectionTableTests() { 
        var dt = new AircraftDetectionTable();

        Assert.IsFalse(dt.Detected("A", 1));
        Assert.IsTrue(dt.Detected("A", 15));
        Assert.IsTrue(dt.Detected("C", 11));
        Assert.IsFalse(dt.Detected("D", 11));
        Assert.IsTrue(dt.Detected("G", 20));
        Assert.IsFalse(dt.Detected("G", 14));

    }


    [Test]
    public void GetDetectionSuitTests() {

        Assert.IsTrue(GetSuit() == Heart);
        Assert.IsTrue(GetSuit() == Spade);
        Assert.IsTrue(GetSuit() == Diamond);

    }


    [Test]
    public void DetectionSuitTests()
    {
        var suit = Heart;
        Assert.IsTrue(Heart == suit);
        suit = GetNextSuit(suit);
        Assert.IsTrue(Spade == suit);
        suit = GetNextSuit(suit);
        Assert.IsTrue(Diamond == suit);
        suit = GetNextSuit(suit);
        Assert.IsTrue(Heart == suit);

    }


}
