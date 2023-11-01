using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WaveFunctionCollapse; 

public class Tilemap : MonoBehaviour
{

    public List<List<GameObject>> hexes;

    [HideInInspector]
    public int mapWidth;
    [HideInInspector]
    public int mapHeight;

    Dictionary<Vector2Int, TileBase> tiles = new Dictionary<Vector2Int, TileBase>();

    public void initTilemap()
    {
        hexes = MapGenerator.instance.hexes;
        mapHeight = MapGenerator.instance.mapHeight;
        mapWidth = MapGenerator.instance.mapWidth;
        
    }

    public void ClearAllTiles() {
        hexes = MapGenerator.instance.hexes;
        tiles.Clear();
    }

    internal void SetTile(int row, int col, TileBase tile)
    {
        tiles.Add(new Vector2Int(row, col), tile);
    }

    public void CreateTiles(int height, int width, int offsetX = 0, int offsetY=0) {
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
            hexPrefabs[tile.Key.y][tile.Key.x] = HexMap.GetPrefab(tile.Value.hexType);
        }

        for (int x = 0; x < hexPrefabs.Count; x++) {

            for (int y = 0; y < hexPrefabs[x].Count; y++) {
                HexMap.SwapHex(hexPrefabs[x][y], MapGenerator.instance.hexes[x+ offsetX][y+ offsetY]);
            }
        
        }

        

        //MapGenerator.instance.swap
        //MapGenerator.instance.InstantiateHexes(hexPrefabs, height, width);

    }

    public void SwapTiles() {
        Debug.Log("Number of Tiles: "+tiles.Count);
        foreach (var tile in tiles) {
            GameObject prefab = HexMap.GetPrefab(tile.Value.hexType);
            GameObject newHex = HexMap.SwapHex(prefab, MapGenerator.instance.hexes[tile.Key.y][tile.Key.x]);

            MapGenerator.instance.hexes[tile.Key.y][tile.Key.x] = newHex;
            hexes = MapGenerator.instance.hexes;
        }
    }

    
}
