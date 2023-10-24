using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WaveFunctionCollapse {
    public class TileMapOutput : IOutputCreator<Tilemap>
    {
        private Tilemap outputImage;
        private ValueManager<TileBase> valueManager;

        public Tilemap OutputImage => outputImage;

        public TileMapOutput(ValueManager<TileBase> vm, Tilemap oi) {
            this.outputImage = oi;
            this.valueManager = vm;
        }


        public void CreateOutput(PatternManager manager, int[][] outputValues, int width, int height)
        {
            if (outputValues.Length == 0) {
                Debug.LogError("Empty output values during create output image");
                return;
            }

            this.outputImage.ClearAllTiles();
            int[][] valueGrid = manager.ConvertPatternToValues<TileBase>(outputValues);

            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    TileBase tile = (TileBase)this.valueManager.GetValueFromIndex(valueGrid[row][col]).Value;
                    this.outputImage.SetTile(row, col, tile);

                }
            }


        }
    }
}


