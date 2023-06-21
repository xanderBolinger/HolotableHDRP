using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Operation;

public class OperationUnitJSONTests
{
    // A Test behaves as an ordinary method
    [Test]
    public void ReadTestUnitFile()
    {
        var ou = OperationUnitLoader.LoadJSON("TestUnit");
        Assert.AreEqual("OperationUnit1", ou.unitName);

        // Example verification: Check if the first Unit's name is correct
        Unit unit1 = ou.GetUnits()[0];
        Assert.AreEqual("a1", unit1.name);

        // Example verification: Check if the first Unit's first Trooper's name is correct
        Trooper trooper1 = unit1.GetTroopers()[0];
        Assert.AreEqual("Trooper1", trooper1.name);

        // Example verification: Check if the second Unit's second Vehicle's callsign is correct
        Unit unit2 = ou.GetUnits()[1];
        Vehicle vehicle2 = unit2.GetVehicles()[0];
        Assert.AreEqual("Vehicle2", vehicle2.callsign);


    }

}
