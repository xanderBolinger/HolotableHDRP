using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using static HexCord;
using Operation;
using static Operation.OperationUnit;

public class OperationSaveTests
{
    List<List<HexCord>> hexCords;

    [SetUp]
    public void CreateOperationTest()
    {

        hexCords = new List<List<HexCord>>();

        GameObject t = new GameObject();
        GameObject t1 = new GameObject();
        GameObject t2 = new GameObject();
        GameObject t3 = new GameObject();


        var trees = t.AddComponent<HexCord>();
        trees.hexType = HexType.HeavyWoods;
        trees.x = 0;
        trees.y = 0;
        var trees1 = t1.AddComponent<HexCord>();
        trees1.hexType = HexType.HeavyWoods;
        trees1.x = 0;
        trees1.y = 1;

        var clear = t2.AddComponent<HexCord>();
        clear.hexType = HexType.Clear;
        clear.x = 1;
        clear.y = 0;
        var clear1 = t3.AddComponent<HexCord>();
        clear1.hexType = HexType.Clear;
        clear1.x = 1;
        clear1.y = 1;

        hexCords.Add(new List<HexCord>() { trees, trees1 });
        hexCords.Add(new List<HexCord>() { clear, clear1 });

    }

    [Test]
    public void TestSaveManager()
    {

        OperationUnit ou = new OperationUnit("ou1", new GameObject(), new Vector2Int(1, 1), Side.BLUFOR);
        var trooper = new Trooper("t1", 10);
        var infUnit = new Unit("u1");
        infUnit.AddTrooper(trooper);
        ou.AddUnit(infUnit);

        var saveData = OperationSaveRunner.GetOperationSaveData(hexCords, new List<OperationUnit>() { ou }, 5);

        OperationSaveManager.SaveOperation(saveData, "testSave");

        var loadData = OperationSaveManager.LoadOperation("testSave");

        Assert.AreEqual(HexType.HeavyWoods, loadData.hexes[0][0]);
        Assert.AreEqual(HexType.Clear, loadData.hexes[1][0]);
        Assert.AreEqual(2, loadData.hexes.Count);
        Assert.AreEqual(2, loadData.hexes[0].Count);

        Assert.AreEqual("ou1", loadData.operationUnits[0].unitName);
        Assert.AreEqual(Side.BLUFOR, loadData.operationUnits[0].side);
        Assert.AreEqual(infUnit.identifier, loadData.operationUnits[0].GetUnitByName("u1").identifier);
        Assert.AreEqual(trooper.identifier, loadData.operationUnits[0].GetUnitByName("u1").GetTrooper(0).identifier);
        Assert.AreEqual(false, infUnit == loadData.operationUnits[0].GetUnitByName("u1"));
        Assert.AreEqual(1, loadData.operationUnits[0].x);
        Assert.AreEqual(1, loadData.operationUnits[0].y);

    }

    
}
