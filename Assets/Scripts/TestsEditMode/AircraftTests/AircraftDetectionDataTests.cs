using NUnit.Framework;
using static AircraftDetectionSuit;
using static AircraftDetectionSuitMethods;

public class AircraftDetectionDataTests
{

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
