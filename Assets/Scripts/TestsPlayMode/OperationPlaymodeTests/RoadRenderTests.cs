using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Operation;
using UnityEngine;
using UnityEngine.TestTools;

public class RoadRenderTests
{
    GameObject testLine;
    GameObject pathLine;
    GameObject highwayLine;
    RoadLineCreator roadLineCreator;
    GameObject mapGeneratorObject;
    MapGenerator mapGenerator;

    [SetUp]
    public void SetUp()
    {
        testLine = new GameObject();
        testLine.AddComponent<LineRenderer>();
        testLine.transform.position = new Vector3(0, 0, 0);

        var roadLineCreatorObj = new GameObject();
        roadLineCreator = roadLineCreatorObj.AddComponent<RoadLineCreator>();

        pathLine = new GameObject();
        pathLine.AddComponent<LineRenderer>();
        pathLine.transform.position = new Vector3(0, 0, 0);

        highwayLine = new GameObject();
        highwayLine.AddComponent<LineRenderer>();
        highwayLine.transform.position = new Vector3(0, 0, 0);

        roadLineCreator.pathLine = pathLine.GetComponent<LineRenderer>();
        roadLineCreator.highwayLine = highwayLine.GetComponent<LineRenderer>();

        mapGeneratorObject = new GameObject();
        mapGenerator = mapGeneratorObject.AddComponent<MapGenerator>();
        mapGenerator.createTileMapOnStart = false;
        mapGenerator.hexes = new List<List<GameObject>>();
        GameObject hex1 = new GameObject();
        hex1.transform.position = new Vector3(0, 0, 0);
        GameObject hex2 = new GameObject();
        hex2.transform.position = new Vector3(0, 1, 0);
        GameObject hex3 = new GameObject();
        hex3.transform.position = new Vector3(0, 2, 0);
        mapGenerator.hexes.Add(new List<GameObject> { hex1, hex2, hex3 });
    }

    [UnityTest]
    public IEnumerator LineRendererTest()
    {
        yield return new WaitForSeconds(0.01f);
        var line = testLine.GetComponent<LineRenderer>();
        Assert.AreEqual(Vector3.zero, line.GetPosition(0));
        Assert.AreEqual(new Vector3(0,0,1), line.GetPosition(1));
    }

    [UnityTest]
    public IEnumerator SetPointsTest() {
        yield return new WaitForSeconds(0.01f);

        List<Vector2Int> points = new List<Vector2Int>();
        points.Add(new Vector2Int(0,0));
        points.Add(new Vector2Int(0, 1));
        points.Add(new Vector2Int(0, 2));

        roadLineCreator.SetPoints(points, false);

        var line = roadLineCreator.pathLine;
        Assert.AreEqual(Vector3.zero, line.GetPosition(0));
        Assert.AreEqual(new Vector3(0, 1, 0), line.GetPosition(1));
        Assert.AreEqual(new Vector3(0, 2, 0), line.GetPosition(2));

        roadLineCreator.SetPoints(points, true);

        line = roadLineCreator.highwayLine;
        Assert.AreEqual(Vector3.zero, line.GetPosition(0));
        Assert.AreEqual(new Vector3(0, 1, 0), line.GetPosition(1));
        Assert.AreEqual(new Vector3(0, 2, 0), line.GetPosition(2));
    }

}
