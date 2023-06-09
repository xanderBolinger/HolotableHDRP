using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Operation;
using UnityEngine;
using UnityEngine.TestTools;

public class MoveUnitTests
{
    OperationUnit unit;
    OperationUnit unit2;
    OperationUnit unit3;
    Vector3Int hex1pos;
    Vector3Int hex2pos;
    GridMover gridMover;

    [SetUp]
    public void SetUp()
    {
        unit = new OperationUnit();
        unit.unitName = "unit 1";
        unit.unitGameobject = new GameObject();
        unit.unitGameobject.AddComponent<OperationUnitData>();
        unit.unitGameobject.transform.position = new Vector3(0, 0, 0);

        /*unit2 = new OperationUnit();
        unit2.unitName = "unit 2";
        unit2.unitGameobject = new GameObject();
        unit2.unitGameobject.AddComponent<OperationUnitData>();
        unit2.unitGameobject.transform.position = new Vector3(0, 0, 0);

        unit3 = new OperationUnit();
        unit3.unitName = "unit 3";
        unit3.unitGameobject = new GameObject();
        unit3.unitGameobject.AddComponent<OperationUnitData>();
        unit3.unitGameobject.transform.position = new Vector3(0, 0, 0);*/

        hex1pos = new Vector3Int(1, 0, 1);
        hex2pos = new Vector3Int(2, 0, 2);

        GameObject gridMoverObj = new GameObject();
        gridMover = gridMoverObj.AddComponent<GridMover>();
    }

    [UnityTest]
    public IEnumerator MoveUnit()
    {

        gridMover.MoveUnit(unit, new Vector2Int(1, 1), new Vector3(0, 0, 0), hex1pos);

        yield return new WaitForSeconds(4.0f);

        Assert.AreEqual(0.15f,
            GetUnitPosition(unit));

        /*gridMover.MoveUnit(unit2, new Vector2Int(1, 1), new Vector3(0, 0, 0), hex1pos);

        Assert.AreEqual(0.175f,
            GetUnitPosition(unit2));

        gridMover.MoveUnit(unit3, new Vector2Int(1, 1), new Vector3(0, 0, 0), hex1pos);

        yield return new WaitForSeconds(4.0f);

        Assert.AreEqual(0.2f,
             GetUnitPosition(unit3));

        gridMover.MoveUnit(unit, new Vector2Int(2, 2), hex1pos, hex2pos);

        yield return new WaitForSeconds(4.0f);

        Assert.AreEqual(2, unit.unitGameobject.transform.position.x);
        Assert.AreEqual(0.15f, GetUnitPosition(unit));
        Assert.AreEqual(2, unit.unitGameobject.transform.position.z);

        Assert.AreEqual(1, unit2.unitGameobject.transform.position.x);
        Assert.AreEqual(0.15f, GetUnitPosition(unit2));
        Assert.AreEqual(1, unit2.unitGameobject.transform.position.z);

        Assert.AreEqual(1, unit3.unitGameobject.transform.position.x);
        Assert.AreEqual(0.175f, GetUnitPosition(unit3));
        Assert.AreEqual(1, unit3.unitGameobject.transform.position.z);*/

        //Assert.AreEqual(0.15f, unit.unitGameobject.transform.position.y);
    }

    public Decimal GetUnitPosition(OperationUnit unit)
    {
        return Math.Round((Decimal)unit.unitGameobject.transform.position.y, 5, MidpointRounding.AwayFromZero);
    }
}
