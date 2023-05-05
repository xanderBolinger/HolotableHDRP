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

        return newHex;
    }

}
