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
    List<List<HexCord>> hexCords;

    [SetUp]
    public void CreateOperationTest()
    {
        GameObject obj = new GameObject();
        var gridMover = obj.AddComponent<GridMover>();
        opm = obj.AddComponent<OperationManager>();
        opm.CreateOperation();
        opm.undoRedo = new UndoRedo(opm);
        opm.gridMover = gridMover;
        
        hexCords = new List<List<HexCord>>();

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

        hexCords.Add(new List<HexCord>() { trees, trees1 });
        hexCords.Add(new List<HexCord>() { clear, clear1 });

        opm.SetHexes(
            new List<List<GameObject>> { 
                new List<GameObject>() { t, t1 },
                new List<GameObject>() { t2, t3 }
            }
        );

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
        var ouData = ou.unitGameobject.AddComponent<OperationUnitData>();
        ouData.ou = ou;
        ou.hexPosition = new Vector2Int(0, 0);
        opm.AddOU(ou);
        opm.undoRedo.AddTurn();

        OperationMovement.AddPlannedMovement(opm, ou, hexCords[1][1], hexCords, ou.hexPosition);
        Assert.AreEqual(0, currentTimeSegment.plannedMovement.Count);

        ou.moveType = MoveType.REGULAR;

        OperationMovement.AddPlannedMovement(opm, ou, hexCords[0][1], hexCords, ou.hexPosition);
        Assert.AreEqual(1, currentTimeSegment.plannedMovement.Count);

        opm.AdvanceTS();
        currentTimeSegment = opm.currentTimeSegment;

        ou.moveType = MoveType.EXTENDED;

        OperationMovement.AddPlannedMovement(opm, ou, hexCords[1][0], hexCords, new Vector2Int(0, 1));
        Assert.AreEqual(1, currentTimeSegment.plannedMovement[ou].Count);

        opm.AdvanceTS();

        opm.undoRedo.Undo();

        Assert.AreEqual(new Vector2Int(0, 1), opm.operationUnits[0].hexPosition);

        opm.undoRedo.Undo();

        Assert.AreEqual(new Vector2Int(0, 0), opm.operationUnits[0].hexPosition);

        opm.undoRedo.Redo();

        Assert.AreEqual(new Vector2Int(0, 1), opm.operationUnits[0].hexPosition);

        opm.undoRedo.Redo();

        Assert.AreEqual(new Vector2Int(1, 0), opm.operationUnits[0].hexPosition);
    }

}
