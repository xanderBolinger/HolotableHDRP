using Operation;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HexCord;

public class MouseManager : MonoBehaviour
{
    public HexType hexType;

    public bool mouseManagerOn = true;
    public bool creatingTiles = true;

    public GameObject hillHexPrefab;
    public GameObject grassHexPrefab;
    public GameObject treeHexPrefab;
    public GameObject cityHexPrefab;
    public GameObject townHexPrefab;
    public GameObject highwayHexPrefab;
    public GameObject pathHexPrefab;
    public GameObject unitPrefab;

    GameObject selectedUnit;
    GridMover gridMover;

    void Start()
    {

        gridMover = GetComponent<GridMover>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!mouseManagerOn)
            return;

        if (!creatingTiles)
            MoveUnits();
        else
            CreateTiles();
        

    }

    private void MoveUnits()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        GameObject hitObject = GetClickedObject();

        if (hitObject == null || (hitObject.tag != "Unit" && hitObject.tag != "Hex")) 
            return;

        bool shift = Input.GetKey(KeyCode.LeftShift);

        if (hitObject.tag == "Hex" && shift) {
            GameObject newUnitObj = Instantiate(unitPrefab);

            OperationUnit newUnit = new OperationUnit
            {
                unitName = newUnitObj.name,
                unitGameobject = newUnitObj
            };
            var cord = GetCordFromHex(hitObject);
            newUnit.hexPosition = cord;
            gridMover.MoveUnit(newUnit, cord, newUnitObj.transform.position, hitObject.transform.position);
        }
        else if (hitObject.tag == "Unit")
        {
            selectedUnit = hitObject;
        } else if (hitObject.tag == "Hex" && selectedUnit != null) {
            var cord = GetCordFromHex(hitObject);
            var unit = gridMover.GetClickedUnit(selectedUnit);
            var oldPosition = MapGenerator.instance.hexes[unit.hexPosition.x][unit.hexPosition.y].transform.position;
            gridMover.MoveUnit(unit, cord, oldPosition, hitObject.transform.position);
        }

    }

    private Vector2Int GetCordFromHex(GameObject hex) {
        int x = hex.GetComponent<HexCord>().x;
        int y = hex.GetComponent<HexCord>().y;
        return new Vector2Int(x, y);
    }


    private GameObject GetClickedObject() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo))
        {
            return hitInfo.collider.transform.gameObject;
        }

        return null;
    }

    private void CreateTiles() {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo))
        {

            GameObject hitObject = hitInfo.collider.transform.gameObject;

            //Debug.Log("Raycast Hit: "+hitInfo.collider.gameObject.name+", Tag: "+hitObject.tag);
            if (Input.GetMouseButton(0) && hitObject.tag == "Hex")
            {
                var hexCord = hitObject.GetComponent<HexCord>();
                int x = hexCord.x;
                int y = hexCord.y;

                Debug.Log("Clicked Hex: " + x
                    + ", " + y);

                GameObject newHex = HexMap.SwapHex(HexMap.GetPrefab(hexType), hexCord.hexObject);

                if (MapGenerator.instance != null)
                {
                    MapGenerator.instance.hexes[x][y] = newHex;
                    Debug.Log("Set Hex");
                }

                //Debug.Log("Set Color");
                //MeshRenderer mr = hitObject.GetComponent<MeshRenderer>();
                //mr.material.color = Color.yellow;
            }
            else if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Click Miss Hex");
            }


        }
    }

}
