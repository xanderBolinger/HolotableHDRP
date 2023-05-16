using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class GridMoverTests
{

    Unit unit;
    Unit unit2;
    Unit unit3;
    Vector3Int hex1pos;
    Vector3Int hex2pos;
    GridMover gridMover;

    [SetUp]
    public void SetUp() {
        unit = new Unit();
        unit.unitName = "unit 1";
        unit.unitGameobject = new GameObject();
        unit.unitGameobject.transform.position = new Vector3(0, 0, 0);

        unit2 = new Unit();
        unit2.unitName = "unit 2";
        unit2.unitGameobject = new GameObject();
        unit2.unitGameobject.transform.position = new Vector3(0, 0, 0);

        unit3 = new Unit();
        unit3.unitName = "unit 3";
        unit3.unitGameobject = new GameObject();
        unit3.unitGameobject.transform.position = new Vector3(0, 0, 0);

        hex1pos = new Vector3Int(1, 0, 1);
        hex2pos = new Vector3Int(2, 0, 2);

        GameObject gridMoverObj = new GameObject();
        gridMover = gridMoverObj.AddComponent<GridMover>();
    }
    
    [Test]
    public void MoveUnit()
    {

        gridMover.MoveUnit(unit, new Vector2Int(1,1), new Vector3(0, 0, 0), hex1pos);
        
        Assert.AreEqual(0.15f,
            GetUnitPosition(unit));

        gridMover.MoveUnit(unit2, new Vector2Int(1, 1), new Vector3(0, 0, 0), hex1pos);

        Assert.AreEqual(0.175f,
            GetUnitPosition(unit2));

        gridMover.MoveUnit(unit3, new Vector2Int(1, 1), new Vector3(0, 0, 0), hex1pos);

        Assert.AreEqual(0.2f,
             GetUnitPosition(unit3));

        gridMover.MoveUnit(unit, new Vector2Int(2, 2), hex1pos, hex2pos);

        Assert.AreEqual(2, unit.unitGameobject.transform.position.x);
        Assert.AreEqual(0.15f, GetUnitPosition(unit));
        Assert.AreEqual(2, unit.unitGameobject.transform.position.z);

        Assert.AreEqual(1, unit2.unitGameobject.transform.position.x);
        Assert.AreEqual(0.15f, GetUnitPosition(unit2));
        Assert.AreEqual(1, unit2.unitGameobject.transform.position.z);
        
        Assert.AreEqual(1, unit3.unitGameobject.transform.position.x);
        Assert.AreEqual(0.175f, GetUnitPosition(unit3));
        Assert.AreEqual(1, unit3.unitGameobject.transform.position.z);

        //Assert.AreEqual(0.15f, unit.unitGameobject.transform.position.y);
    }

    [Test]
    public void GetClickedUnit() {
        gridMover.MoveUnit(unit, new Vector2Int(1, 1), new Vector3(0, 0, 0), hex1pos);
        gridMover.MoveUnit(unit2, new Vector2Int(1, 1), new Vector3(0, 0, 0), hex1pos);
        gridMover.MoveUnit(unit3, new Vector2Int(1, 1), new Vector3(0, 0, 0), hex1pos);
        gridMover.MoveUnit(unit, new Vector2Int(2, 2), hex1pos, hex2pos);

        Assert.AreEqual("unit 1", gridMover.GetClickedUnit(unit.unitGameobject).unitName);
        Assert.AreEqual("unit 2", gridMover.GetClickedUnit(unit2.unitGameobject).unitName);
        Assert.AreEqual("unit 3", gridMover.GetClickedUnit(unit3.unitGameobject).unitName);
    }


    public Decimal GetUnitPosition(Unit unit) {
        return Math.Round((Decimal)unit.unitGameobject.transform.position.y, 5, MidpointRounding.AwayFromZero);
    }

}
