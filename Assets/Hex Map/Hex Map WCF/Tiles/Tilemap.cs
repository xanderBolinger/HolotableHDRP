using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WaveFunctionCollapse; 

public class Tilemap : MonoBehaviour
{

    public List<List<GameObject>> hexes;
    public int mapWidth;
    public int mapHeight;

    Dictionary<Vector2Int, TileBase> tiles = new Dictionary<Vector2Int, TileBase>();

    public void initTilemap()
    {
        hexes = PerlinGenerator.instance.hexes;
        mapHeight = PerlinGenerator.instance.mapHeight;
        mapWidth = PerlinGenerator.instance.mapWidth;
        
    }

    public void ClearAllTiles() {
        hexes = PerlinGenerator.instance.hexes;
        tiles.Clear();
    }

    internal void SetTile(int row, int col, TileBase tile)
    {
        tiles.Add(new Vector2Int(row, col), tile);
    }

    public void CreateTiles(int height, int width) {
        Debug.Log("Tiles Count: "+tiles.Count+", Height: "+height+", Width: "+width);
        List<List<GameObject>> hexPrefabs = new List<List<GameObject>>();
        for (int i = 0; i < height; i++) {
            List<GameObject> row = new List<GameObject>();

            for (int j = 0; j < width; j++) {
                row.Add(null);
            }
            hexPrefabs.Add(row);
        }

        foreach (var tile in tiles)
        {
            hexPrefabs[tile.Key.y][tile.Key.x] = GetPrefab(tile.Value.hexType);
        }

        PerlinGenerator.instance.InstantiateHexes(hexPrefabs, height, width);

    }

    public void SwapTiles() {
        Debug.Log("Number of Tiles: "+tiles.Count);
        foreach (var tile in tiles) {
            GameObject prefab = GetPrefab(tile.Value.hexType);
            GameObject newHex = HexMap.SwapHex(prefab, PerlinGenerator.instance.hexes[tile.Key.y][tile.Key.x]);

            PerlinGenerator.instance.hexes[tile.Key.y][tile.Key.x] = newHex;
            hexes = PerlinGenerator.instance.hexes;
        }
    }

    private GameObject GetPrefab(HexCord.HexType hexType)
    {
        return hexType switch
        {
            HexCord.HexType.CLEAR => PerlinGenerator.instance.grassPrefab,
            HexCord.HexType.WOODS => PerlinGenerator.instance.treePrefab,
            HexCord.HexType.MOUNTAIN => PerlinGenerator.instance.mountainPrefab,
            HexCord.HexType.CITY => PerlinGenerator.instance.cityPrefab,
            HexCord.HexType.TOWN => PerlinGenerator.instance.townPrefab,
            HexCord.HexType.PATH => PerlinGenerator.instance.pathPrefab,
            HexCord.HexType.HIGHWAY => PerlinGenerator.instance.highwayPrefab,
            _ => throw new Exception("Tile prefab not found for hex type: " + hexType),
        };
    }
}
