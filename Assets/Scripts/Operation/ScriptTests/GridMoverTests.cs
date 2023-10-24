using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Operation;
using UnityEngine;
using UnityEngine.TestTools;

public class GridMoverTests
{

    OperationUnit unit;
    OperationUnit unit2;
    OperationUnit unit3;
    Vector3Int hex1pos;
    Vector3Int hex2pos;
    GridMover gridMover;

    [SetUp]
    public void SetUp() {
        unit = new OperationUnit();
        unit.unitName = "unit 1";
        unit.unitGameobject = new GameObject();
        unit.unitGameobject.AddComponent<OperationUnitData>();
        unit.unitGameobject.transform.position = new Vector3(0, 0, 0);

        unit2 = new OperationUnit();
        unit2.unitName = "unit 2";
        unit2.unitGameobject = new GameObject();
        unit2.unitGameobject.AddComponent<OperationUnitData>();
        unit2.unitGameobject.transform.position = new Vector3(0, 0, 0);

        unit3 = new OperationUnit();
        unit3.unitName = "unit 3";
        unit3.unitGameobject = new GameObject();
        unit3.unitGameobject.AddComponent<OperationUnitData>();
        unit3.unitGameobject.transform.position = new Vector3(0, 0, 0);

        hex1pos = new Vector3Int(1, 0, 1);
        hex2pos = new Vector3Int(2, 0, 2);

        GameObject gridMoverObj = new GameObject();
        gridMover = gridMoverObj.AddComponent<GridMover>();
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


    

}
