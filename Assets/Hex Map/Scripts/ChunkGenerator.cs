using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class ChunkGenerator : MonoBehaviour
{

    public List<GameObject> chunkPrefabs = new List<GameObject>();
    public int mapWidth = 2;
    public int mapHeight = 2;
    float xSpacing = 0.19f;
    float ySpacing = 0.165f;

    private void Start()
    {

        CreateTileMap();

    }

    void CreateTileMap()
    {


        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                GameObject chunk = GetChunk();

                List<List<GameObject>> chunks = new List<List<GameObject>>();

                for (int i = 0; i < 10; i++)
                {
                    List<GameObject> row = new List<GameObject>();
                    for (int j = 0; j < 10; j++)
                    {
                        row.Add(null);
                    }
                    chunks.Add(row);
                }

                for (int i = 0; i < 100; i++) {
                    GameObject hex = chunk.transform.GetChild(i).gameObject;
                    Regex regex = new Regex(@"[\d]+");
                    var matches = regex.Matches(hex.name);
                    chunks[Int32.Parse(matches[0].Value)][Int32.Parse(matches[1].Value)] = hex;
                }

                /*for (int i = 0; i < 10; i++) {
                        List<GameObject> row = new List<GameObject>();
                    for (int j = 0; j < 10; j++) {
                        //chunk.transform.GetChild
                        row.Add(chunk.transform.GetChild(count).gameObject);
                        count++;
                    }
                    chunks.Add(row);
                }*/


                int count = 0; 
                for (int i = 0; i < chunks.Count; i++) {
                    var row = chunks[i];

                    for (int j = 0; j < row.Count; j++) {
                        //Debug.Log("Draw Hex");
                        DrawHex(row[j], j+(x*10), i+(y*10));
                        count++;

                    }

                }

                //Debug.Log("Draw Hex Count: " + count);
            }
        }

    }

    void DrawHex(GameObject hex, int x, int y) {

        if (y % 2 == 0)
        {
            hex.transform.position = new Vector3(x * xSpacing, 0, y * ySpacing);
        }
        else
        {
            hex.transform.position = new Vector3(x * xSpacing + xSpacing / 2, 0, y * ySpacing);
        }

        hex.name = x + ", " + y;
        hex.name = x + ", " + y;
        if (hex.GetComponent<HexCord>() == null)
        {
            hex.GetComponentInChildren<HexCord>().x = x;
            hex.GetComponentInChildren<HexCord>().y = y;
        }
        else
        {
            hex.GetComponent<HexCord>().x = x;
            hex.GetComponent<HexCord>().y = y;
        }
        hex.transform.parent = gameObject.transform;
    }

    GameObject GetChunk() {
        GameObject chunkPrefab = chunkPrefabs[UnityEngine.Random.Range(0, chunkPrefabs.Count - 1)];

        // Prevents duplicate chunks from being added
        chunkPrefabs.Remove(chunkPrefab);
        
        GameObject chunk = Instantiate(chunkPrefab);
        chunk.transform.position = new Vector3(0, 0, 0);
        return chunk;
    }


}
