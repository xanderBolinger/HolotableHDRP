using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{

    GameObject selectedHexPrefab;
    public GameObject hillHexPrefab;
    public GameObject grassHexPrefab;
    public GameObject treeHexPrefab;

    void Start()
    {
        selectedHexPrefab = grassHexPrefab;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            selectedHexPrefab = grassHexPrefab;
        } else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            selectedHexPrefab = treeHexPrefab;
        } else if (Input.GetKeyDown(KeyCode.Alpha3)) {
            selectedHexPrefab = hillHexPrefab;
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

                Debug.Log("Clicked Hex: "+ x
                    +", "+ y);

                GameObject newHex = Instantiate(selectedHexPrefab);
                newHex.transform.position = hitInfo.collider.transform.position;
                

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

                if (hitObject.transform.parent != null &&
                    hitObject.transform.parent.gameObject != null &&
                    hitObject.transform.parent.gameObject.tag == "Hex")
                {
                    newHex.name = hitObject.transform.parent.gameObject.name;
                    newHex.transform.parent = hitObject.transform.parent.transform.parent;
                    Destroy(hitObject.transform.parent.gameObject);
                }
                else {
                    newHex.name = hitObject.transform.gameObject.name;
                    newHex.transform.parent = hitObject.transform.parent;
                    Destroy(hitObject);
                }


                //Debug.Log("Set Color");
                //MeshRenderer mr = hitObject.GetComponent<MeshRenderer>();
                //mr.material.color = Color.yellow;
            } else if (Input.GetMouseButtonDown(0)) {
                Debug.Log("Click Miss Hex");
            }


        }

    }
}
