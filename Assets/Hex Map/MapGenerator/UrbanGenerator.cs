using HexMapper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UrbanGenerator : MonoBehaviour
{
    public enum UrbanType { 
        City,Town
    }

    public enum Amount { 
        None,Low,Medium,High
    }

    public enum Density { 
        Low,Medium,High
    }

    public Amount townAmount = Amount.Low;
    public Amount cityAmount = Amount.Low;

    public Density townDensity = Density.Low;
    public Density cityDensity = Density.Low;

    public int mapWidth = 100;
    public int mapHeight = 100;

    Dictionary<Vector2Int, UrbanType> urbanPoints = new Dictionary<Vector2Int,UrbanType>();
    Dictionary<Vector2Int, UrbanType> surroundingPoints = new Dictionary<Vector2Int, UrbanType>();

    Dictionary<Vector2Int, GameObject> originalHexes = new Dictionary<Vector2Int, GameObject>();

    public void Generate() {
        CalculateUrbanPoints();
        CalculateSurroundingPoints();
        SwapOriginalHexes();
        SwapUrbanHexes(urbanPoints);
        SwapUrbanHexes(surroundingPoints);
        CreatePaths();
        CreateHighways();

    }

    public void CreatePaths()
    {
        List<RoadPoint> pathPoints = new List<RoadPoint>();

        foreach (var point in urbanPoints)
        {
            if (point.Value != UrbanType.Town)
                continue;

            pathPoints.Add(new RoadPoint(point.Key, point.Value == UrbanType.City ? HexCord.HexType.CITY : HexCord.HexType.TOWN));
        }

        List<RoadPoint> tour = UrbanRoadGenerator.CalculateTour(pathPoints);

        Dictionary<Vector2Int, UrbanType> pathTiles = UrbanRoadGenerator.GetRoadTiles(tour, UrbanType.Town);

        SwapUrbanHexes(pathTiles, true);
    }

    public void CreateHighways() {
        List<RoadPoint> highwayPoints = new List<RoadPoint>();

        foreach (var point in urbanPoints)
        {
            if (point.Value != UrbanType.City)
                continue;

            highwayPoints.Add(new RoadPoint(point.Key, point.Value == UrbanType.City ? HexCord.HexType.CITY : HexCord.HexType.TOWN));
        }

        List<RoadPoint> tour = UrbanRoadGenerator.CalculateTour(highwayPoints);

        Dictionary<Vector2Int, UrbanType> highwayTiles = UrbanRoadGenerator.GetRoadTiles(tour, UrbanType.City);

        SwapUrbanHexes(highwayTiles, true);
    }

    public void SwapUrbanHexes(Dictionary<Vector2Int, UrbanType> urbanPoints, bool road=false) {
        foreach (var point in urbanPoints)
        {
            GameObject prefab;

            if (point.Value == UrbanType.City)
                prefab = road == false ? MapGenerator.instance.cityPrefab : MapGenerator.instance.highwayPrefab;
            else
                prefab = road == false ? MapGenerator.instance.townPrefab : MapGenerator.instance.pathPrefab;

            if (point.Key.x < 0 || point.Key.y < 0 || point.Key.x >= MapGenerator.instance.hexes.Count
                || point.Key.y >= MapGenerator.instance.hexes[0].Count)
                continue;

            GameObject original = MapGenerator.instance.hexes[point.Key.x][point.Key.y];

            HexCord.HexType hexType;

            if (original.GetComponent<HexCord>() != null)
                hexType = original.GetComponent<HexCord>().hexType;
            else
                hexType = original.GetComponentInChildren<HexCord>().hexType;

            originalHexes.TryAdd(point.Key, HexMap.GetPrefab(hexType));

            HexMap.SwapHex(prefab, original);
        }
    }

    public void SwapOriginalHexes() {
        foreach (var hex in originalHexes)
        {
            HexMap.SwapHex(hex.Value, MapGenerator.instance.hexes[hex.Key.x][hex.Key.y]);
        }

        originalHexes.Clear();
    }

    public void CalculateUrbanPoints() {

        urbanPoints.Clear();

        int numberOfCities = GetNumber(cityAmount);
        int numberOfTowns = GetNumber(townAmount);

        AddUrbanPoints(numberOfCities, mapWidth, mapHeight, UrbanType.City);
        AddUrbanPoints(numberOfTowns, mapWidth, mapHeight, UrbanType.Town);

    }

    public void CalculateSurroundingPoints() {
        surroundingPoints.Clear();
        AddSurroundingPoints(cityDensity, UrbanType.City);
        AddSurroundingPoints(townDensity, UrbanType.Town);

    }

    public void AddSurroundingPoints(Density density, UrbanType urbanType) {



        foreach (var point in urbanPoints) {
            if (point.Value != urbanType)
                continue;
            
            int amount = GetDensityAmount(density);
            List<Vector2Int> points = GetHexPoints(amount, point.Key);

            foreach (var surroundingPoint in points) {

                if (urbanType == UrbanType.City) {
                    Debug.Log("Add surrounding point city");
                }

                surroundingPoints.Add(surroundingPoint, urbanType);
            }

        }

    }


    // First 1/2 of surrounding hexes, random 6 directions
    // Others go to random closest hex to original point
    // 50/50 chance to go in a random direction
    public List<Vector2Int> GetHexPoints(int startingAmount, Vector2Int originalPoint) {
        List<Vector2Int> points = new List<Vector2Int>();

        Vector2Int currentPoint = originalPoint;
        int amount = startingAmount;
        while (amount > 0)
        {


            int iteration = 0;

            var neigbour = GetNeighbour(amount <= startingAmount / 2, originalPoint, currentPoint);

            while ((points.Contains(neigbour) || urbanPoints.ContainsKey(neigbour) || surroundingPoints.ContainsKey(neigbour)) && iteration < 10)
            {
                neigbour = GetNeighbour(amount <= startingAmount / 2, originalPoint, currentPoint);
                iteration++; 
            }


            if (!points.Contains(neigbour) && !urbanPoints.ContainsKey(neigbour) && !surroundingPoints.ContainsKey(neigbour))
            {
                points.Add(neigbour);
                currentPoint = neigbour;
            }

            amount--;
        }

        return points;
    }

    public Vector2Int GetNeighbour(bool closest, Vector2Int originalPoint, Vector2Int currentPoint) {

        var neighbours = HexDirection.GetHexNeighbours(currentPoint);

        if (!closest || DiceRoller.Roll(1,100) <= 50)
            return neighbours[DiceRoller.Roll(0, 5)];

        var closestPoint = neighbours[DiceRoller.Roll(0, 5)];
        int closestPointDistance = HexMap.GetDistance(closestPoint, originalPoint);

        foreach (var point in neighbours) {
            if (point == closestPoint)
                continue;
            if (HexMap.GetDistance(point, originalPoint) < closestPointDistance) {
                closestPoint = point;
                closestPointDistance = HexMap.GetDistance(closestPoint, originalPoint);
            }
        }

        List<Vector2Int> closestPoints = new List<Vector2Int>();

        foreach (var point in neighbours) {
            if (HexMap.GetDistance(point, originalPoint) == closestPointDistance)
                closestPoints.Add(point);
        }

        return closestPoints[DiceRoller.Roll(0,closestPoints.Count-1)];

    }

    public int GetDensityAmount(Density density) {
        int amount;

        switch (density)
        {
            case Density.Low:
                amount = DiceRoller.Roll(1, 3);
                break;
            case Density.Medium:
                amount = DiceRoller.Roll(1, 6);
                break;
            case Density.High:
                amount = DiceRoller.Roll(1, 9);
                break;
            default:
                throw new System.Exception("Amount not found: " + cityAmount);
        }

        return amount;
    }

    public void AddUrbanPoints(int number, int mapWidth, int mapHeight, UrbanType urbanType) {
        for (int i = 0; i < number; i++)
        {

            int iteration = 0;

            while (iteration < 10)
            {

                int x = DiceRoller.Roll(0, mapWidth - 1);
                int y = DiceRoller.Roll(0, mapHeight - 1);

                if (CheckConflicts(x, y))
                {
                    iteration++;
                    continue;
                }

                urbanPoints.Add(new Vector2Int(x, y), urbanType);
                break;
            }

        }
    }

    public bool CheckConflicts(int x, int y) {
        foreach (var val in urbanPoints) {
            if (val.Key.x == x && val.Key.y == y) {
                return true;
            }
        } 

        return false;         
    }

    public int GetNumber(Amount amount) {
        if (amount == Amount.None)
            return 0;

        switch (amount)
        {
            case Amount.Low:
                return DiceRoller.Roll(1,3);
            case Amount.Medium:
                return DiceRoller.Roll(4, 7);
            case Amount.High:
                return DiceRoller.Roll(8, 15);
            default:
                throw new System.Exception("Amount not found: " + cityAmount);
        }
    }

}
