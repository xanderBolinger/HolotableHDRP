using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MapExporter
{

    public static void ExportMap() {
        var map = new Map();

        Debug.Log("File Saved: "+Application.dataPath + "/MapJson/"+map.name+"_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") +"_" 
            +map.width+"x"+map.height+"_.json");
        System.IO.File.WriteAllText(Application.dataPath + "/MapJson/"+map.name+"_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + "_"
            + map.width + "x" + map.height + "_.json", 
            JsonConvert.SerializeObject(map));
    }


    private class Map {
        public List<Tile> tiles = new List<Tile>();
        public List<RoadPoint> pathPoints = new List<RoadPoint>();
        public List<RoadPoint> highwayPoints = new List<RoadPoint>();
        public int width;
        public int height;
        public string name;

        public Map() {
            var hexes = MapGenerator.instance.hexes;

            name = MapGenerator.instance.mapName;
            width = MapGenerator.instance.mapWidth;
            height = MapGenerator.instance.mapHeight;

            for (int x = 0; x < hexes.Count; x++) {
                for (int y = 0; y < hexes[x].Count; y++) {
                    var obj = hexes[x][y];
                    HexCord hexCord = obj.GetComponent<HexCord>() != null ? obj.GetComponent<HexCord>() 
                        : obj.GetComponentInChildren<HexCord>();
                    tiles.Add(new Tile(x,y,hexCord.elevation,hexCord.hexType.ToString()));

                }
            }


        }

    }

    private class RoadPoint {
        public int x;
        public int y;

        public RoadPoint(int x, int y) {
            this.x = x;
            this.y = y;
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
