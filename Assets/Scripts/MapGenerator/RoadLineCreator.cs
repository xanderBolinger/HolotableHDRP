using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadLineCreator : MonoBehaviour
{
    public LineRenderer pathLine;
    public LineRenderer highwayLine;


    public void SetPoints(List<Vector2Int> points, bool highway) {
        var renderer = highway ? highwayLine : pathLine;
        renderer.positionCount = points.Count;


        for (int i = 0; i < points.Count; i++) {
            var point = points[i];
            var x = point.x;
            var y = point.y;
            var hex = MapGenerator.instance.hexes[x][y];
            renderer.SetPosition(i, hex.transform.position);
        }

    }

}
