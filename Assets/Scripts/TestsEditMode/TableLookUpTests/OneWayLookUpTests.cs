using NUnit.Framework;
using UnityEngine;

public class OneWayLookUpTests
{


	[Test]
	public void LookUpTestString()
	{
		TextAsset csvFile = Resources.Load<TextAsset>("TableLookUpTests/OneWayTable");
		var table = new OneWayTable(csvFile);

		Assert.AreEqual("y1", table.GetValue("x1"));
		Assert.AreEqual("y2", table.GetValue("x2"));
		Assert.AreEqual("y3", table.GetValue("x3"));
	}

	[Test]
	public void LookUpTestInt()
	{
		TextAsset csvFile = Resources.Load<TextAsset>("TableLookUpTests/OneWayTableInt");
		var table = new OneWayTable(csvFile);

        Assert.AreEqual("y1", table.GetValue(1));
        Assert.AreEqual("y2", table.GetValue(4));
        Assert.AreEqual("y3", table.GetValue(5));
    }

}
