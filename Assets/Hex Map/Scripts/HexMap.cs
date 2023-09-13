using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMap : MonoBehaviour
{

    public GameObject hexPrefab;
    public int mapWidth = 25;
    public int mapHeight = 12;
    
    float xSpacing = 0.19f;
    float ySpacing = 0.165f;

    // Start is called before the first frame update
    void Start()
    {
        CreateTileMap();
    }

    void CreateTileMap() {


        for (int x = 0; x < mapWidth; x++) {
            for (int y = 0; y < mapHeight; y++) {
                GameObject hex = Instantiate(hexPrefab);

                if (y % 2 == 0)
                {
                    hex.transform.position = new Vector3(x * xSpacing, 0, y * ySpacing);
                }
                else {
                    hex.transform.position = new Vector3(x*xSpacing + xSpacing / 2, 0, y*ySpacing);
                }

                hex.name = x + ", " + y;
                if (hex.GetComponent<HexCord>() == null)
                {
                    hex.GetComponentInChildren<HexCord>().x = x;
                    hex.GetComponentInChildren<HexCord>().y = y;
                }
                else { 
                    hex.GetComponent<HexCord>().x = x;
                    hex.GetComponent<HexCord>().y = y;
                }


                hex.transform.parent = gameObject.transform;

            }
        }

    }


    public static GameObject SwapHex(GameObject hexPrefab, GameObject hex) {


        GameObject newHex = Instantiate(hexPrefab);
        newHex.transform.position = hex.transform.position;

        int x, y;
        if (hex.GetComponent<HexCord>() == null)
        {
            x = hex.GetComponentInChildren<HexCord>().x;
            y = hex.GetComponentInChildren<HexCord>().y;
        }
        else
        {
            x = hex.GetComponent<HexCord>().x;
            y = hex.GetComponent<HexCord>().y;
        }

        if (newHex.GetComponent<HexCord>() == null)
        {
            newHex.GetComponentInChildren<HexCord>().x = x;
            newHex.GetComponentInChildren<HexCord>().y = y;
        }
        else
        {
            newHex.GetComponent<HexCord>().x = x;
            newHex.GetComponent<HexCord>().y = y;
        }

        if (hex.transform.parent != null &&
            hex.transform.parent.gameObject != null &&
            hex.transform.parent.gameObject.tag == "Hex")
        {
            newHex.name = hex.transform.parent.gameObject.name;
            newHex.transform.parent = hex.transform.parent.transform.parent;
            
            Destroy(hex.transform.parent.gameObject);
        }
        else
        {
            newHex.name = hex.transform.gameObject.name;
            newHex.transform.parent = hex.transform.parent;
            Destroy(hex);
        }

        if (MapGenerator.instance != null) {
            MapGenerator.instance.hexes[x][y] = newHex;
        }

        return newHex;
    }

    public static int GetDistance(Vector2Int a, Vector2Int b)
    {
        int x0 = a.x - (int)Mathf.Floor(a.y / 2);
        int y0 = a.y;
        int x1 = b.x - (int)Mathf.Floor(b.y / 2);
        int y1 = b.y;
        int dx = x1 - x0;
        int dy = y1 - y0;
        return Mathf.Max(Mathf.Abs(dx), Mathf.Abs(dy), Mathf.Abs(dx + dy));
    }

    public static GameObject GetPrefab(HexCord.HexType hexType)
    {
        return hexType switch
        {
            HexCord.HexType.Clear => MapGenerator.instance.grassPrefab,
            HexCord.HexType.HeavyWoods => MapGenerator.instance.heavyTreePrefab,
            HexCord.HexType.MediumWoods => MapGenerator.instance.mediumWoodsPrefab,
            HexCord.HexType.LightWoods => MapGenerator.instance.lightWoodsPrefab,
            HexCord.HexType.Brush => MapGenerator.instance.brushPrefab,
            HexCord.HexType.HeavyBrush => MapGenerator.instance.heavyBrushPrefab,
            HexCord.HexType.MOUNTAIN => MapGenerator.instance.mountainPrefab,
            HexCord.HexType.BigBuilding => MapGenerator.instance.cityPrefab,
            HexCord.HexType.Building => MapGenerator.instance.townPrefab,
            HexCord.HexType.PATH => MapGenerator.instance.pathPrefab,
            HexCord.HexType.HIGHWAY => MapGenerator.instance.highwayPrefab,
            _ => throw new Exception("Tile prefab not found for hex type: " + hexType),
        };
    }

}
