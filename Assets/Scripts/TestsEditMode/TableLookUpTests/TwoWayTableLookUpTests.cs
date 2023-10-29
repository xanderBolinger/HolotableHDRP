using NUnit.Framework;
using UnityEngine;

public class TwoWayTableLookUpTests
{


    [Test]
    public void LookUpTestStringXStringY()
    {
        TextAsset csvFile = Resources.Load<TextAsset>("TableLookUpTests/TableStringXStringY");
        var table = new TwoWayTable(csvFile);

        Assert.AreEqual("c11", table.GetValue("x1", "y1"));
        Assert.AreEqual("c23", table.GetValue("x2", "y3"));
        Assert.AreEqual("c44", table.GetValue("x4", "y4"));
    }

    [Test]
    public void LookUpTestIntXStringY()
    {
        TextAsset csvFile = Resources.Load<TextAsset>("TableLookUpTests/TableIntXStringY");
        var table = new TwoWayTable(csvFile);

        Assert.AreEqual("c11", table.GetValue(1, "y1"));
        Assert.AreEqual("c23", table.GetValue(4, "y3"));
        Assert.AreEqual("c44", table.GetValue(7, "y4"));
    }

    [Test]
    public void LookUpTestIntXIntY()
    {
        TextAsset csvFile = Resources.Load<TextAsset>("TableLookUpTests/TableIntXIntY");
        var table = new TwoWayTable(csvFile);

        Assert.AreEqual("c11", table.GetValue(1, 2));
        Assert.AreEqual("c23", table.GetValue(4, 5));
        Assert.AreEqual("c44", table.GetValue(7, 8));
    }

}
