using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Helpers;
using System;

namespace WaveFunctionCollapse {
    public class InputReader : IInputReader<TileBase>
    {

        private Tilemap _inputTilemap;

        public InputReader(Tilemap input) {
            _inputTilemap = input;
        }

        public IValue<TileBase>[][] ReadInputToGrid() {

            var grid = ReadInputTileMap();

            TileBaseValue[][] gridOfValues = null;

            if (grid != null) {
                gridOfValues = MyCollectionExtension.CreateJaggedArray<TileBaseValue[][]>(grid.Length, grid[0].Length);
                for (int rowIndex = 0; rowIndex < grid.Length; rowIndex++) {

                    for (int colIndex = 0; colIndex < grid[0].Length; colIndex++) {
                        gridOfValues[rowIndex][colIndex] = new TileBaseValue(grid[rowIndex][colIndex]);
                    }

                }
            }

            return gridOfValues;

        }

        private TileBase[][] ReadInputTileMap() {

            return CreateTileBasedGrid(_inputTilemap);

        }

        private TileBase[][] CreateTileBasedGrid(Tilemap tilemap) {
            var tileBases = getBases();

            TileBase[][] gridOfInputTiles = MyCollectionExtension.CreateJaggedArray<TileBase[][]>(tilemap.mapWidth, tilemap.mapHeight);

            for (int rowIndex = 0; rowIndex < tilemap.hexes.Count; rowIndex++) {

                var row = tilemap.hexes[rowIndex];

                for (int colIndex = 0; colIndex < row.Count; colIndex++) {
                    HexCord.HexType hexType;
                    Debug.Log("Row: "+rowIndex+", Col: "+colIndex);
                    if (tilemap.hexes[rowIndex][colIndex].GetComponent<HexCord>() != null)
                    {
                        hexType = tilemap.hexes[rowIndex][colIndex].GetComponent<HexCord>().hexType;
                    }
                    else {
                        hexType = tilemap.hexes[rowIndex][colIndex].GetComponentInChildren<HexCord>().hexType;
                    }

                    gridOfInputTiles[rowIndex][colIndex] = findBase(tileBases, hexType);
                }
            
            }

            Debug.Log("Input Rows: " + gridOfInputTiles.Length);
            Debug.Log("Input Cols: " + gridOfInputTiles[0].Length);

            /*if (gridOfInputTiles[0][0] == gridOfInputTiles[0][1])
                Debug.Log("tiles are equal");
            else
                Debug.Log("tiles are not equal");*/

            return gridOfInputTiles;

        }

        TileBase findBase(List<TileBase> bases, HexCord.HexType hexType) {
            foreach (var tb in bases) {
                if (tb.hexType == hexType)
                    return tb;
            }

            throw new Exception("Tilebase not found, type: " + hexType);
        }

        List<TileBase> getBases() {

            List<TileBase> bases = new List<TileBase>();

            foreach (HexCord.HexType hexType in Enum.GetValues(typeof(HexCord.HexType))) {

                bases.Add(new TileBase(hexType));

            }


            return bases;
        }

    }
}


