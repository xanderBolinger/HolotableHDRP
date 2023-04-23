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


        for (int x = 0; x <= mapWidth; x++) {
            for (int y = 0; y <= mapHeight; y++) {
                GameObject hex = Instantiate(hexPrefab);

                if (y % 2 == 0)
                {
                    hex.transform.position = new Vector3(x * xSpacing, 0, y * ySpacing);
                }
                else {
                    hex.transform.position = new Vector3(x*xSpacing + xSpacing / 2, 0, y*ySpacing);
                }

                hex.name = x + ", " + y;
                hex.GetComponent<HexCord>().x = x;
                hex.GetComponent<HexCord>().y = y;
                hex.transform.parent = gameObject.transform;

            }
        }

    }
}
