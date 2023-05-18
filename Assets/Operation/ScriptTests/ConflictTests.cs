using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Operation;
using UnityEngine;
using UnityEngine.TestTools;
using static HexCord;
using static Operation.OperationUnit;

public class ConflictTests
{

    OperationManager opm;

    [SetUp]
    public void Init()
    {
        GameObject obj = new GameObject();
        opm = obj.AddComponent<OperationManager>();
        opm.CreateOperation();
    }

    [Test]
    public void TwoByGridConflict()
    {
        OperationUnit ou = new OperationUnit("ou1", new GameObject(), new Vector2Int(0, 0), Side.BLUFOR);
        OperationUnit ou1 = new OperationUnit("ou2", new GameObject(), new Vector2Int(1, 1), Side.BLUFOR);
        OperationUnit ou2 = new OperationUnit("ou3", new GameObject(), new Vector2Int(1, 0), Side.OPFOR);
        OperationUnit ou3 = new OperationUnit("ou4", new GameObject(), new Vector2Int(0, 1), Side.OPFOR);

        opm.AddOU(ou);
        opm.AddOU(ou1);
        opm.AddOU(ou2);
        opm.AddOU(ou3);

        var conflicts = OperationMovement.GetConflicts(opm);

        Assert.AreEqual(4, conflicts.Count);
        Assert.AreEqual(2, conflicts[0].targets.Count);
        Assert.AreEqual(2, conflicts[1].targets.Count);
        Assert.AreEqual(2, conflicts[2].targets.Count);
        Assert.AreEqual(2, conflicts[3].targets.Count);
    }

    [Test]
    public void MultiConflictTest() {
        OperationUnit ou = new OperationUnit("ou1", new GameObject(), new Vector2Int(2, 72), Side.BLUFOR);
        OperationUnit ou1 = new OperationUnit("ou2", new GameObject(), new Vector2Int(2, 76), Side.BLUFOR);
        OperationUnit ou2 = new OperationUnit("ou3", new GameObject(), new Vector2Int(3, 74), Side.OPFOR);
        OperationUnit ou3 = new OperationUnit("ou4", new GameObject(), new Vector2Int(3, 70), Side.OPFOR);

        opm.AddOU(ou);
        opm.AddOU(ou1);
        opm.AddOU(ou2);
        opm.AddOU(ou3);

        var conflicts = OperationMovement.GetConflicts(opm);

        Assert.AreEqual(4, conflicts.Count);
        Assert.AreEqual(2, conflicts[0].targets.Count);
        Assert.AreEqual(1, conflicts[1].targets.Count);
        Assert.AreEqual(2, conflicts[2].targets.Count);
        Assert.AreEqual(1, conflicts[3].targets.Count);
    }

    [Test]
    public void ApplyCasulaties() {


        OperationUnit ou = new OperationUnit("ou1", new GameObject(), new Vector2Int(2, 72), Side.BLUFOR);

        Unit unit1 = new Unit("u1");
        Unit unit2 = new Unit("u2");
        Unit unit3 = new Unit("u3");
        Unit unit4 = new Unit("u4");
        ou.AddUnit(unit1);
        ou.AddUnit(unit2);
        ou.AddUnit(unit3);
        ou.AddUnit(unit4);

        CombatResults.SetUnitStatus(ou, 0, UnitStatus.DISORDERED);
        CombatResults.SetUnitStatus(ou, 1, UnitStatus.DISORDERED);

        Assert.AreEqual(UnitStatus.FRESH, ou.unitStatus);

        CombatResults.SetUnitStatus(ou, 2, UnitStatus.DISORDERED);

        Assert.AreEqual(UnitStatus.DISORDERED, ou.unitStatus);


        CombatResults.SetUnitStatus(ou, 1, UnitStatus.ROUTED);
        CombatResults.SetUnitStatus(ou, 2, UnitStatus.ROUTED);

        Assert.AreEqual(UnitStatus.ROUTED, ou.unitStatus);
    }

}
