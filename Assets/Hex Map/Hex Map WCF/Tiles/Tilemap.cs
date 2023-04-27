using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tilemap : MonoBehaviour
{

    public List<List<GameObject>> hexes;
    public int mapWidth;
    public int mapHeight;

    public void initTilemap()
    {
        hexes = PerlinGenerator.instance.hexes;
        mapHeight = PerlinGenerator.instance.mapHeight;
        mapWidth = PerlinGenerator.instance.mapWidth;
    }


}
