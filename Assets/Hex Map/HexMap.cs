using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMap : MonoBehaviour
{

    public GameObject hexPrefab;
    public int mapWidth = 25;
    public int mapHeight = 12;
    
    float xSpacing = 0.19f;
    float zSpacing = 0.165f;

    // Start is called before the first frame update
    void Start()
    {
        CreateTileMap();
    }

    void CreateTileMap() {


        for (int x = 0; x <= mapWidth; x++) {
            for (int z = 0; z <= mapHeight; z++) {

                GameObject hex = Instantiate(hexPrefab);

                if (z % 2 == 0)
                {
                    hex.transform.position = new Vector3(x * xSpacing, 0, z * zSpacing);
                }
                else {
                    hex.transform.position = new Vector3(x*xSpacing + xSpacing / 2, 0, z*zSpacing);
                }

            
            }
        }

    }
}
