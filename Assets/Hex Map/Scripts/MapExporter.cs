using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MapExporter
{

    public static void ExportMap() {
        var map = new Map();

        Debug.Log("File Saved: "+Application.dataPath + "/MapJson/Map_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".json");
        System.IO.File.WriteAllText(Application.dataPath + "/MapJson/Map_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".json", 
            JsonConvert.SerializeObject(map));
    }


    private class Map {
        public List<Tile> tiles = new List<Tile>();

        public Map() {
            var hexes = MapGenerator.instance.hexes;

            for (int x = 0; x < hexes.Count; x++) {
                for (int y = 0; y < hexes[x].Count; y++) {
                    var obj = hexes[x][y];
                    HexCord hexCord = obj.GetComponent<HexCord>() != null ? obj.GetComponent<HexCord>() 
                        : obj.GetComponentInChildren<HexCord>();
                    tiles.Add(new Tile(x,y,0,hexCord.hexType.ToString()));
                }
            }


        }

    }

    private class Tile {
        public int x;
        public int y;
        public int elevation;
        public string tileType;

        public Tile(int x, int y, int elevation, string tileType) {
            this.x = x;
            this.y = y;
            this.elevation = elevation;
            this.tileType = tileType;
        }

    }

}
