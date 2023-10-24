using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class AircraftJsonTests
{
    // A Test behaves as an ordinary method
    [Test]
    public void ReadV19Test()
    {
        var aircraft = AircraftLoader.LoadAirCraft("V19");
        //Assert.AreEqual("OperationUnit1", ou.unitName);

        /*Unit unit1 = ou.GetUnits()[0];
        Assert.AreEqual("a1", unit1.name);

        Trooper trooper1 = unit1.GetTroopers()[0];
        Assert.AreEqual("Trooper1", trooper1.name);

        Unit unit2 = ou.GetUnits()[1];
        Vehicle vehicle2 = unit2.GetVehicles()[0];
        Assert.AreEqual("Vehicle2", vehicle2.callsign);*/


    }

}
