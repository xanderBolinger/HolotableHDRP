using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceSideTests
{

    [Test]
    public void CreateSideTest() {

        var side = new ForceSide("side");
        Assert.AreEqual("side", side.displayName);

    }

    [Test]
    public void SideRelationTest() {

        var blufor = new ForceSide("blufor");
        var opfor = new ForceSide("opfor");
        var indfor = new ForceSide("indfor");

        blufor.AddFriendly(indfor);
        indfor.AddFriendly(blufor);

        Assert.IsTrue(blufor.FriendlyTowards(indfor));
        Assert.IsTrue(indfor.FriendlyTowards(blufor));
        Assert.IsTrue(!indfor.FriendlyTowards(opfor));
        Assert.IsTrue(!opfor.FriendlyTowards(indfor));
        Assert.IsTrue(!blufor.FriendlyTowards(opfor));
        Assert.IsTrue(!opfor.FriendlyTowards(blufor));
    }

}
