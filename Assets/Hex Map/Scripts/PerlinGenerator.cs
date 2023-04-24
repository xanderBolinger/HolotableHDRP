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

    public int densityUpper = 5;
    int densityLower = 2; 

    public int upperOffset = 1000;
    public int lowerOffset = 0;
    public float upperMagnification = 7.5f;
    public float lowerMagnification = 6.5f;


    public static PerlinGenerator instance; 

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
    }

    public void CreateTileMap()
    {
        Debug.Log("Mountain: ");
        var mountainMap = NoiseMap();
        Debug.Log("Tree Map: ");
        var treeMap = NoiseMap();

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                int mountain = mountainMap[x][y];
                int tree = treeMap[x][y];
                GameObject hexPrefab = grassPrefab;
                if (mountain == 1)
                    hexPrefab = mountainPrefab;
                else if (tree == 1)
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
                    hex.GetComponentInChildren<HexCord>().x = x;
                    hex.GetComponentInChildren<HexCord>().y = y;
                }
                else
                {
                    hex.GetComponent<HexCord>().x = x;
                    hex.GetComponent<HexCord>().y = y;
                }


                hex.transform.parent = gameObject.transform;

            }
        }

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


    List<List<int>> NoiseMap() {
        List<List<int>> map = new List<List<int>>();

        float magnification = UnityEngine.Random.Range(lowerMagnification, upperMagnification);
        System.Random rand = new System.Random();


        int density = UnityEngine.Random.Range(densityLower, densityUpper); 
        int number = rand.Next(lowerOffset, upperOffset);
        int xOffset = number;
        int yOffset = number;
        Debug.Log("Offset: " + number + ", Magnification: " + magnification+", Density: "+density);

        for (int x = 0; x < mapWidth; x++) {

            List<int> values = new List<int>();

            for (int y = 0; y < mapHeight; y++) {
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

        return map;

    }


}
