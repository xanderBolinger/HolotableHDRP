using HexMapper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UrbanGenerator;

public class RoadPoint {
    public Vector2Int cord;
    public HexCord.HexType hexType;

    public RoadPoint(Vector2Int cord, HexCord.HexType hexType) {
        this.cord = cord;
        this.hexType = hexType;
    }

}

public class UrbanRoadGenerator
{

    

    public static List<RoadPoint> CalculateTour(List<RoadPoint> places) {
        if (places.Count == 2)
            return new List<RoadPoint>() { places[0], places[1] };
        else if (places.Count <= 1)
            return new List<RoadPoint>();

        List<RoadPoint> tour = new List<RoadPoint>();
        List<RoadPoint> visitedPoints = new List<RoadPoint>();

        var currentPoint = places[0];

        List<RoadPoint> unvisitedPlaces = new List<RoadPoint>(places);

        while (unvisitedPlaces.Count > 0) {
            RoadPoint closestPoint = unvisitedPlaces[0];
            foreach (var point in unvisitedPlaces) { 
                if(visitedPoints.Contains(point) || point == currentPoint) {
                    continue; 
                }


                if (HexMap.GetDistance(currentPoint.cord, point.cord) < HexMap.GetDistance(currentPoint.cord, closestPoint.cord)) {
                    closestPoint = point;
                }

            }

            tour.Add(closestPoint);
            unvisitedPlaces.Remove(closestPoint);
            currentPoint = closestPoint;

        }

        tour.Add(places[0]);

        return tour;
    }



    // Fork chance 1 in 50 tiles 
    // Curve chance 
    // Enter off map
    // Posibility of cross map highway
    public static Dictionary<Vector2Int, UrbanType> GetRoadTiles(List<RoadPoint> tour, UrbanType urbanType) {
        if (tour.Count < 2)
            return new Dictionary<Vector2Int, UrbanType>();

        Dictionary<Vector2Int, UrbanType> tiles = new Dictionary<Vector2Int, UrbanType>();

        for (int i = 0; i < tour.Count - 1; i++) {
            foreach (var tile in GetPathOfTiles(tour[i].cord, tour[i+1].cord, urbanType, tour, tiles))
            {
                if (!tiles.ContainsKey(tile.Key))
                    tiles.Add(tile.Key, tile.Value);
            }
        }

        return tiles;
    }


    
    private static Dictionary<Vector2Int, UrbanType> GetPathOfTiles(Vector2Int a, Vector2Int b, UrbanType urbanType, List<RoadPoint> tour, Dictionary<Vector2Int, UrbanType> roadTiles) {

        Dictionary<Vector2Int, UrbanType> tiles = new Dictionary<Vector2Int, UrbanType>();

        int iteration = 0;
        var currentPoint = a;
        while (true) {
            

            var neighbours = HexDirection.GetHexNeighbours(currentPoint);
            List<Vector2Int> options = new List<Vector2Int>();

            foreach (var hex in neighbours) {

                if (HexMap.GetDistance(hex, b) < HexMap.GetDistance(currentPoint, b)
                    && (hex.x > -1 && hex.y > -1 && hex.x < MapGenerator.instance.hexes.Count && hex.y < MapGenerator.instance.hexes[0].Count)) {
                    options.Add(hex);
                }

            }

            if (options.Count == 0) {
                iteration++;
                continue;
            }


            currentPoint = options.Count > 1 ? options[DiceRoller.Roll(0, options.Count - 1)] : options[0];

            if ((MapGenerator.instance.hexes[currentPoint.x][currentPoint.y].GetComponent<HexCord>() == null ||
                (MapGenerator.instance.hexes[currentPoint.x][currentPoint.y].GetComponent<HexCord>().hexType != HexCord.HexType.CITY &&
                MapGenerator.instance.hexes[currentPoint.x][currentPoint.y].GetComponent<HexCord>().hexType != HexCord.HexType.TOWN)) &&
                (MapGenerator.instance.hexes[currentPoint.x][currentPoint.y].GetComponentInChildren<HexCord>() == null ||
                (MapGenerator.instance.hexes[currentPoint.x][currentPoint.y].GetComponentInChildren<HexCord>().hexType != HexCord.HexType.CITY &&
                MapGenerator.instance.hexes[currentPoint.x][currentPoint.y].GetComponentInChildren<HexCord>().hexType != HexCord.HexType.TOWN))
                && !tiles.ContainsKey(currentPoint)) { 
                tiles.Add(currentPoint, urbanType);
            }
            //tiles.Add(currentPoint, urbanType);

            if (HexMap.GetDistance(currentPoint, b) == 1) { 
                Debug.Log("Return Tiles");
                return tiles;
            }

            if (iteration > 500) {
                Debug.LogError("Iteration over 500");
                return tiles;
            }
            iteration++;
        }
    
    }


}
