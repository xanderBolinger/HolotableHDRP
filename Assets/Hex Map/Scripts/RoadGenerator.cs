using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
//using AStarSharp;

public class RoadGenerator
{

    public static List<List<int>> GetPath(List<int> from, List<int> to) {
        Debug.Log("Get Path");
        List<List<int>> cords = new List<List<int>>();

        Dictionary<Vector2Int, int> grid = new Dictionary<Vector2Int, int>();

        for (int i = 0; i < PerlinGenerator.instance.mapWidth; i++)
        {



            for (int j = 0; j < PerlinGenerator.instance.mapHeight; j++)
            {

                grid.Add(new Vector2Int(i, j), 0);

            }

        }

        List<int> passableValues = new List<int>();
        passableValues.Add(0);
       /* for (int i = 0; i < 100; i++) {
            passableValues.Add(i);
        }*/

        List<Vector2Int> list = PathFinding2D.find4(new Vector2Int(from[0], from[1]), new Vector2Int(to[0], to[1]), grid, passableValues);
        foreach (Vector2Int vector in list) {
            cords.Add(new List<int> { vector.x, vector.y });
        }
        /*List<List<int>> cords = new List<List<int>>();

        List<List<Node>> grid = new List<List<Node>>();

        for (int i = 0; i < PerlinGenerator.instance.mapWidth; i++) {

            var row = new List<Node>();

            for (int j = 0; j < PerlinGenerator.instance.mapHeight; j++) {

                row.Add(new Node(new System.Numerics.Vector2(i, j), true));

            }
            grid.Add(row);
        }

        Astar aStar = new Astar(grid);

        System.Numerics.Vector2 v1 = new System.Numerics.Vector2(from[0], from[1]);
        System.Numerics.Vector2 v2 = new System.Numerics.Vector2(to[0], to[1]);
        Debug.Log("V1: " + v1.X+", "+v1.Y+", V2: "+v2.X+", "+v2.Y);

        var stack = aStar.FindPath(v1, v2);

        for (int i = 0; i < stack.Count; i++) {
            Debug.Log("Stack: "+stack.Pop().Position.X);
        }*/

        /*while (stack.Count > 0) {
            System.Numerics.Vector2 pos = stack.Pop().Position;
            cords.Add(new List<int> {(int)pos.X,(int)pos.Y});
            Debug.Log("Add Cord to Path: X: "+pos.X+", Y: "+pos.Y);
        }*/

        return cords;
    }

    public static int Distance(int aX1, int aY1, int aX2, int aY2)
    {
        int dx = aX2 - aX1;     // signed deltas
        int dy = aY2 - aY1;
        int x = Mathf.Abs(dx);  // absolute deltas
        int y = Mathf.Abs(dy);
        // special case if we start on an odd row or if we move into negative x direction
        if ((dx < 0) ^ ((aY1 & 1) == 1))
            x = Mathf.Max(0, x - (y + 1) / 2);
        else
            x = Mathf.Max(0, x - (y) / 2);
        return x + y;
    }

    public static void CreateRoads(List<List<int>> coordinates, List<List<GameObject>> hexes, GameObject prefab)
    {


        for (int i = 0; i < coordinates.Count; i++)
        {
            Debug.Log("Cord in Create Roads");

            for (int j = 0; j < coordinates.Count; j++)
            {
                if (i == j || Distance(coordinates[i][0], coordinates[i][1], coordinates[j][0], coordinates[j][1]) == 1)
                    continue;

                var path = GetPath(new List<int> { coordinates[i][0], coordinates[i][1] },
                    new List<int> { coordinates[j][0], coordinates[j][1] });


                for (int cord = 0; cord < path.Count; cord++) {

                    var hex = hexes[path[cord][0]][path[cord][1]];
                    
                    if (hex.GetComponent<HexCord>() != null
                        && (hex.GetComponent<HexCord>().urbanHex || hex.GetComponent<HexCord>().roadHex))
                        continue;
                    else if (hex.GetComponentInChildren<HexCord>().urbanHex ||
                        hex.GetComponentInChildren<HexCord>().roadHex)
                        continue;
                    Debug.Log("Swap Hex");
                    HexMap.SwapHex(prefab, hexes[path[cord][0]][path[cord][1]]);
                }


                return;
                //hexes[i][j]


            }

        }


    }

}
