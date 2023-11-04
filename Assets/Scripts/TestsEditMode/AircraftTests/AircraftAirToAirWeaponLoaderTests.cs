using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using static AircraftSpeedData;
using static AircraftMovementData;
using System.Drawing.Drawing2D;
using static AircraftLoader;
using static AirToAirWeaponType;

public class AircraftAirToAirWeaponLoaderTests
{

	[Test]
	public void LoadWeapons() {

		var loader = new AirToAirWeaponLoader();

		var jatm = loader.GetWeapon(Aim260Jatm);
		Assert.IsTrue(jatm.weaponType == Aim260Jatm);
        Assert.IsTrue(jatm.radarGuided);
		Assert.AreEqual(3, jatm.standardRating);
        Assert.AreEqual(2, jatm.bvrRating);
        Assert.AreEqual(60, jatm.bvrRangeForward);
        Assert.AreEqual(45, jatm.bvrRangeBeam);
        Assert.AreEqual(30, jatm.bvrRangeRear);
        Assert.AreEqual("AIM-260 JATM", 
            jatm.weaponDisplayName);
    }
	

}
