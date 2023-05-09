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
        SwapOriginalHexes();

        foreach (var point in urbanPoints) {
            GameObject prefab;

            if (point.Value == UrbanType.City)
                prefab = MapGenerator.instance.cityPrefab;
            else
                prefab = MapGenerator.instance.townPrefab;

            GameObject original = MapGenerator.instance.hexes[point.Key.y][point.Key.x];

            HexCord.HexType hexType;

            if (original.GetComponent<HexCord>() != null)
                hexType = original.GetComponent<HexCord>().hexType;
            else
                hexType = original.GetComponentInChildren<HexCord>().hexType;

            originalHexes.Add(point.Key, HexMap.GetPrefab(hexType));

            HexMap.SwapHex(prefab, original);
        }

    }

    public void SwapOriginalHexes() {
        foreach (var hex in originalHexes)
        {
            HexMap.SwapHex(hex.Value, MapGenerator.instance.hexes[hex.Key.y][hex.Key.x]);
        }

        originalHexes.Clear();
    }

    public void CalculateUrbanPoints() {

        urbanPoints.Clear();

        int numberOfCities = GetNumber(cityAmount);
        int numberOfTowns = GetNumber(townAmount);

        AddUrbanPoints(numberOfCities, mapWidth, mapHeight, UrbanType.City);
        AddUrbanPoints(numberOfTowns, mapWidth, mapHeight, UrbanType.Town);

        /*foreach (var point in urbanPoints) {
            Debug.Log("Point: "+point.Value+", "+point.Key);
        }*/

    }

    public void AddSurroundingPoints(Density density, UrbanType urbanType) {

        surroundingPoints.Clear();

        foreach (var point in urbanPoints) {
            if (point.Value != urbanType)
                continue;

            int amount = GetDensityAmount(density);
            

        }

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

    public void DrawUrbanAreas() { 
    
    }

}
