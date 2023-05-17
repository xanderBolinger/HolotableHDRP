using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Operation;
using static Operation.OperationUnit;

public class OperationUnitTests
{

    Trooper trooper;
    Trooper trooper2;
    Unit infUnit;
    Unit motorizedUnit;
    Unit mechanizedUnit;
    Unit armorUnit;
    Unit heavyWalkerUnit;
    Unit lightWalkerUnit;
    Unit speederUnit;

    [SetUp]
    public void init() {
        trooper = new Trooper("t1", 10);
        trooper2 = new Trooper("t2", 10);
        infUnit = new Unit("u1");
        infUnit.AddTrooper(trooper);

        Vehicle motorized = new Vehicle("motorized", Vehicle.VehicleType.MOTORIZED, "Truck", false, 20);
        Vehicle mechanized = new Vehicle("mechanized", Vehicle.VehicleType.MECHANIZED, "Mechanized", false, 20);
        Vehicle armor = new Vehicle(Vehicle.VehicleClass.AAT, "armor");
        Vehicle heavyWalker = new Vehicle(Vehicle.VehicleClass.ATTE, "heavy walker");
        Vehicle lightWalker = new Vehicle("light walker", Vehicle.VehicleType.LIGHT_WALKER, "Light Walker", false, 1);
        Vehicle speeder = new Vehicle("speeder", Vehicle.VehicleType.SPEEDER, "speeder", true, 1);

        motorizedUnit = new Unit("motorizedUnit");
        motorizedUnit.AddTrooper(trooper);
        motorizedUnit.AddVehicle(motorized);
        motorizedUnit.AddVehicle(lightWalker);
        motorizedUnit.AddVehicle(speeder);

        mechanizedUnit = new Unit("mechanizedUnit");
        mechanizedUnit.AddTrooper(trooper);
        mechanizedUnit.AddVehicle(motorized);
        mechanizedUnit.AddVehicle(mechanized);
        mechanizedUnit.AddVehicle(lightWalker);
        mechanizedUnit.AddVehicle(speeder);

        armorUnit = new Unit("armorUnit");
        armorUnit.AddTrooper(trooper);
        armorUnit.AddVehicle(motorized);
        armorUnit.AddVehicle(mechanized);
        armorUnit.AddVehicle(armor);
        armorUnit.AddVehicle(heavyWalker);
        armorUnit.AddVehicle(lightWalker);
        armorUnit.AddVehicle(speeder);

        heavyWalkerUnit = new Unit("heavyWalkerUnit");
        heavyWalkerUnit.AddTrooper(trooper);
        heavyWalkerUnit.AddVehicle(motorized);
        heavyWalkerUnit.AddVehicle(mechanized);
        heavyWalkerUnit.AddVehicle(heavyWalker);
        heavyWalkerUnit.AddVehicle(lightWalker);
        heavyWalkerUnit.AddVehicle(speeder);

        lightWalkerUnit = new Unit("lightWalkerUnit");
        lightWalkerUnit.AddTrooper(trooper);
        lightWalkerUnit.AddVehicle(lightWalker);
        lightWalkerUnit.AddVehicle(speeder);

        speederUnit = new Unit("speederUnit");
        speederUnit.AddTrooper(trooper);
        speederUnit.AddVehicle(speeder);

    }

    [Test]
    public void TrooperTest()
    {
        Assert.AreEqual("t1", trooper.name);
        Assert.AreEqual(10, trooper.sl);
        Assert.AreEqual(10, trooper.identifier.Length);
        Assert.AreEqual(false, Identifier.CompareTo(trooper, trooper2));
    }

    [Test]
    public void UnitTest() {
        Assert.AreEqual("u1", infUnit.name);
        Assert.AreEqual(false, infUnit.spotted);
        Assert.AreEqual(10, infUnit.identifier.Length);
        Assert.AreEqual(trooper.identifier, infUnit.GetTrooper(0).identifier);
        Assert.AreEqual(UnitStatus.FRESH, infUnit.unitStatus);
        Assert.AreEqual(UnitType.INF, infUnit.unitType);


        Assert.AreEqual(UnitType.SPEEDER, speederUnit.unitType);
        Assert.AreEqual(UnitType.LIGHT_WALKER, lightWalkerUnit.unitType);
        Assert.AreEqual(UnitType.HEAVY_WALKER, heavyWalkerUnit.unitType);
        Assert.AreEqual(UnitType.ARMOR, armorUnit.unitType);
        Assert.AreEqual(UnitType.MOTORIZED, motorizedUnit.unitType);
        Assert.AreEqual(UnitType.MECHANIZED, mechanizedUnit.unitType);
    }

    [Test]
    public void OperationUnitTest() {

        OperationUnit ou = new OperationUnit("ou1", new GameObject(), new Vector2Int(0,0), Side.BLUFOR);

        ou.AddUnit(infUnit);

        var u = ou.GetUnit(infUnit);

        Assert.AreEqual(true, Identifier.CompareTo(infUnit, u));
        Assert.AreEqual(UnitType.INF, ou.unitType);

        ou.AddUnit(speederUnit);
        Assert.AreEqual(UnitType.SPEEDER, ou.unitType);

        ou.AddUnit(lightWalkerUnit);
        Assert.AreEqual(UnitType.LIGHT_WALKER, ou.unitType);

        ou.AddUnit(motorizedUnit);
        Assert.AreEqual(UnitType.MOTORIZED, ou.unitType);

        ou.AddUnit(mechanizedUnit);
        Assert.AreEqual(UnitType.MECHANIZED, ou.unitType);

        ou.AddUnit(heavyWalkerUnit);
        Assert.AreEqual(UnitType.HEAVY_WALKER, ou.unitType);

        ou.AddUnit(armorUnit);
        Assert.AreEqual(UnitType.ARMOR, ou.unitType);
        
    }


}
