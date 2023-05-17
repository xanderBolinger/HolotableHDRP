using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Operation;
using static Operation.TimeSegment;
using static HexCord;
using static Operation.OperationUnit;

public class OperationTest
{

    OperationManager opm;
    List<List<HexCord>> hexes;

    [SetUp]
    public void CreateOperationTest()
    {
        GameObject obj = new GameObject();
        opm = obj.AddComponent<OperationManager>();
        opm.CreateOperation();

        hexes = new List<List<HexCord>>();

        GameObject t = new GameObject();
        GameObject t1 = new GameObject();
        GameObject t2 = new GameObject();
        GameObject t3 = new GameObject();


        var trees = t.AddComponent<HexCord>();
        trees.hexType = HexType.WOODS;
        trees.x = 0;
        trees.y = 0;
        var trees1 = t1.AddComponent<HexCord>();
        trees1.hexType = HexType.WOODS;
        trees1.x = 0;
        trees1.y = 1;

        var clear = t2.AddComponent<HexCord>();
        clear.hexType = HexType.CLEAR;
        clear.x = 1;
        clear.y = 0;
        var clear1 = t3.AddComponent<HexCord>();
        clear1.hexType = HexType.CLEAR;
        clear1.x = 1;
        clear1.y = 1;

        hexes.Add(new List<HexCord>() { trees, trees1 });
        hexes.Add(new List<HexCord>() { clear, clear1 });

    }

    [Test]
    public void TSTest() {
        Assert.AreEqual(24, opm.timeSegments.Count);
        Assert.AreEqual(5, opm.currentTimeSegment.hour);
    }


    [Test]
    public void AdvanceTimeTest() {

        for (int i = 0; i < 24; i++) {
            opm.AdvanceTS();
        }

        Assert.AreEqual(5, opm.currentTimeSegment.hour);

        for (int i = 0; i < 12; i++)
        {
            opm.AdvanceTS();
        }

        Assert.AreEqual(17, opm.currentTimeSegment.hour);
        Assert.AreEqual(TimeUnit.PM, opm.currentTimeSegment.timeUnit);
    }


    [Test]
    public void MovementCostTest() {

        Assert.AreEqual(0.5, OperationMovement.GetMovementCost(OperationUnit.UnitType.ARMOR, HexType.PATH, true));
        Assert.AreEqual(1, OperationMovement.GetMovementCost(OperationUnit.UnitType.INF, HexType.CLEAR, false));
        Assert.AreEqual(1, OperationMovement.GetMovementCost(OperationUnit.UnitType.ARMOR, HexType.CLEAR, false));
        Assert.AreEqual(0.333, OperationMovement.GetMovementCost(OperationUnit.UnitType.ARMOR, HexType.HIGHWAY, false));
        Assert.AreEqual(-1, OperationMovement.GetMovementCost(OperationUnit.UnitType.ARMOR, HexType.MOUNTAIN, false));
        Assert.AreEqual(2, OperationMovement.GetMovementCost(OperationUnit.UnitType.LIGHT_WALKER, HexType.MOUNTAIN, false));
    }

    [Test]
    public void PlannedMovementTest() {

        var currentTimeSegment = opm.currentTimeSegment;
        
        OperationUnit ou = new OperationUnit("ou1", new GameObject(), new Vector2Int(0, 0), Side.BLUFOR);
        var trooper = new Trooper("t1", 10);
        var infUnit = new Unit("u1");
        infUnit.AddTrooper(trooper);
        ou.AddUnit(infUnit);

        ou.hexPosition = new Vector2Int(0, 0);

        OperationMovement.AddPlannedMovement(opm, ou, hexes[1][1], hexes, ou.hexPosition);
        Assert.AreEqual(0, currentTimeSegment.plannedMovement.Count);

        ou.moveType = MoveType.REGULAR;

        OperationMovement.AddPlannedMovement(opm, ou, hexes[0][1], hexes, ou.hexPosition);
        Assert.AreEqual(1, currentTimeSegment.plannedMovement.Count);

        ou.moveType = MoveType.EXTENDED;

        OperationMovement.AddPlannedMovement(opm, ou, hexes[1][0], hexes, new Vector2Int(0,1));
        Assert.AreEqual(2, currentTimeSegment.plannedMovement[ou].Count);
    }

}
