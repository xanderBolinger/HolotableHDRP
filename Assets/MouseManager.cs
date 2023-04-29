using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{

    GameObject selectedHexPrefab;
    public GameObject hillHexPrefab;
    public GameObject grassHexPrefab;
    public GameObject treeHexPrefab;
    public GameObject cityHexPrefab;
    public GameObject townHexPrefab;
    public GameObject highwayHexPrefab;
    public GameObject pathHexPrefab;

    Vector2Int b = new Vector2Int(-1,-1);

    void Start()
    {
        selectedHexPrefab = grassHexPrefab;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedHexPrefab = grassHexPrefab;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedHexPrefab = treeHexPrefab;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            selectedHexPrefab = hillHexPrefab;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            selectedHexPrefab = cityHexPrefab;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            selectedHexPrefab = townHexPrefab;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            selectedHexPrefab = highwayHexPrefab;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            selectedHexPrefab = pathHexPrefab;
        }
        else if (Input.GetKeyDown(KeyCode.Space)) {
            Debug.Log("Refresh Map");
            PerlinGenerator.instance.ClearMap();
            
            PerlinGenerator.instance.CreateTileMap();
            PerlinGenerator.instance.GetComponent<Tilemap>().initTilemap();
            PerlinGenerator.instance.GetComponent<Tilemap>().read();
        }
 

        

        //Debug.Log("Mouse Position: "+Input.mousePosition);
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo)) {

            GameObject hitObject = hitInfo.collider.transform.gameObject;

            //Debug.Log("Raycast Hit: "+hitInfo.collider.gameObject.name+", Tag: "+hitObject.tag);
            if (Input.GetMouseButtonDown(0) && hitObject.tag == "Hex") {
                int x = hitObject.GetComponent<HexCord>().x;
                int y = hitObject.GetComponent<HexCord>().y;

                Debug.Log("Clicked Hex: " + x
                    + ", " + y);

                if (b.x == -1)
                {
                    b = new Vector2Int(x, y);
                }
                else {

                    int x0 = b.x - (int)Mathf.Floor(b.y / 2);
                    int y0 = b.y;
                    int x1 = x - (int)Mathf.Floor(y / 2);
                    int y1 = y;
                    int dx = x1 - x0;
                    int dy = y1 - y0;
                    Debug.Log("Distance: "+Mathf.Max(Mathf.Abs(dx), Mathf.Abs(dy), Mathf.Abs(dx + dy)));

                    b.x = -1; 
                }
                
                
                HexMap.SwapHex(selectedHexPrefab, hitObject);


                //Debug.Log("Set Color");
                //MeshRenderer mr = hitObject.GetComponent<MeshRenderer>();
                //mr.material.color = Color.yellow;
            } else if (Input.GetMouseButtonDown(0)) {
                Debug.Log("Click Miss Hex");
            }


        }

    }
}
