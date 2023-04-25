using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PerlinGenerator : MonoBehaviour
{

    public int mapWidth = 25;
    public int mapHeight = 12;

    float xSpacing = 0.19f;
    float ySpacing = 0.165f;

    public GameObject treePrefab;
    public GameObject mountainPrefab;
    public GameObject grassPrefab;
    public GameObject cityPrefab;
    public GameObject townPrefab;
    public GameObject highwayPrefab;
    public GameObject pathPrefab;

    /*int densityUpper = 5;
    int densityLower = 2; */

    int upperOffset = 1000;
    int lowerOffset = 0;
    /*float upperMagnification = 7.5f;
    float lowerMagnification = 6.5f;*/

    public HexFrequency.Frequency mountainFrequency;
    public HexFrequency.Frequency treeFrequency;
    public HexFrequency.Frequency cityFrequency;
    public HexFrequency.Frequency townFrequency;

    public static PerlinGenerator instance;


    List<List<GameObject>> hexes = new List<List<GameObject>>();
    List<List<int>> cityCoordintes = new List<List<int>>();


    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        CreateTileMap();
        
    }

    public void ClearMap() {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        hexes.Clear();
    }

    public void CreateTileMap()
    {
        cityCoordintes.Clear();
        var mountainStats = new HexFrequency(mountainFrequency);
        var treeStats = new HexFrequency(treeFrequency);
        var townStats = new HexFrequency(townFrequency);
        var cityStats = new HexFrequency(cityFrequency);

        Debug.Log("Mountain: ");
        var mountainMap = NoiseMap(mountainStats);
        Debug.Log("Tree Map: ");
        var treeMap = NoiseMap(treeStats);
        Debug.Log("Town Map: ");
        var townMap = NoiseMap(townStats);
        Debug.Log("City Map: ");
        var cityMap = NoiseMap(cityStats);




        for (int x = 0; x < mapWidth; x++)
        {

            var row = new List<GameObject>();

            for (int y = 0; y < mapHeight; y++)
            {
                int mountain = mountainMap[x][y];
                int tree = treeMap[x][y];
                int town = townMap[x][y];
                int city = cityMap[x][y];

                bool urbanHex = false; 

                GameObject hexPrefab = grassPrefab;
                if (city >= cityStats.margin) {
                    cityCoordintes.Add(new List<int> { x, y });
                    hexPrefab = cityPrefab;
                    urbanHex = true;
                }
                else if(town >= townStats.margin)
                    hexPrefab = townPrefab;
                else if(mountain >= mountainStats.margin)
                    hexPrefab = mountainPrefab;
                else if (tree >= treeStats.margin)
                    hexPrefab = treePrefab;

                GameObject hex = Instantiate(hexPrefab);
                
                if (y % 2 == 0)
                {
                    hex.transform.position = new Vector3(x * xSpacing, 0, y * ySpacing);
                }
                else
                {
                    hex.transform.position = new Vector3(x * xSpacing + xSpacing / 2, 0, y * ySpacing);
                }

                hex.name = x + ", " + y;
                if (hex.GetComponent<HexCord>() == null)
                {
                    hex.GetComponentInChildren<HexCord>().urbanHex = urbanHex;
                    hex.GetComponentInChildren<HexCord>().x = x;
                    hex.GetComponentInChildren<HexCord>().y = y;
                }
                else
                {
                    hex.GetComponent<HexCord>().urbanHex = urbanHex;
                    hex.GetComponent<HexCord>().y = y;
                    hex.GetComponent<HexCord>().y = y;
                }

                
                hex.transform.parent = gameObject.transform;
                row.Add(hex);

            }

            hexes.Add(row);
        }

        RoadGenerator.CreateRoads(cityCoordintes, hexes, highwayPrefab);

    }

    /*GameObject GetHex(int x, int y)
    {
        
        float rawPerlin = Mathf.PerlinNoise(
            (x - xOffset) / magnification,
            (y - yOffset) / magnification
        );
        float clampPerlin = Mathf.Clamp01(rawPerlin);
        float scaledPerlin = clampPerlin * hexPrefabs.Count;

        if (scaledPerlin == hexPrefabs.Count)
        {
            scaledPerlin = (hexPrefabs.Count - 1);
        }
        return hexPrefabs[Mathf.FloorToInt(scaledPerlin)];
    }*/

    List<List<int>> NoiseMap(HexFrequency freq)
    {
        int densityLower = freq.densityLower; 
        int densityUpper = freq.densityUpper;
        float magnificationLower = freq.magnifiactionLower;
        float magnificationUpper = freq.magnificationUpper;
        List<List<int>> map = new List<List<int>>();

        float magnification = UnityEngine.Random.Range(magnificationLower, magnificationUpper);
        System.Random rand = new System.Random();


        int density = UnityEngine.Random.Range(densityLower, densityUpper);
        int number = rand.Next(lowerOffset, upperOffset);
        int xOffset = number;
        int yOffset = number;
        Debug.Log("Offset: " + number + ", Magnification: " + magnification + ", Density: " + density);

        CalculatePerlin(xOffset, yOffset, magnification, density, map);

        return map;

    }


    /*List<List<int>> NoiseMap() {
        List<List<int>> map = new List<List<int>>();

        float magnification = UnityEngine.Random.Range(lowerMagnification, upperMagnification);
        System.Random rand = new System.Random();


        int density = UnityEngine.Random.Range(densityLower, densityUpper); 
        int number = rand.Next(lowerOffset, upperOffset);
        int xOffset = number;
        int yOffset = number;
        Debug.Log("Offset: " + number + ", Magnification: " + magnification+", Density: "+density);

        CalculatePerlin(xOffset, yOffset, magnification, density, map);

        return map;

    }*/

    void CalculatePerlin(int xOffset, int yOffset, float magnification, int density, List<List<int>> map) {
        for (int x = 0; x < mapWidth; x++)
        {

            List<int> values = new List<int>();

            for (int y = 0; y < mapHeight; y++)
            {
                float rawPerlin = Mathf.PerlinNoise(
                (x - xOffset) / magnification,
                (y - yOffset) / magnification
            );
                float clampPerlin = Mathf.Clamp01(rawPerlin);
                float scaledPerlin = clampPerlin * density;

                if (scaledPerlin == density)
                {
                    scaledPerlin = (density - 1);
                }
                //Debug.Log(Mathf.FloorToInt(scaledPerlin));
                values.Add(Mathf.FloorToInt(scaledPerlin));

            }

            map.Add(values);

        }
    }

    

}
