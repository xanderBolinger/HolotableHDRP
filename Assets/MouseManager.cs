using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Mouse Position: "+Input.mousePosition);
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo)) {

            GameObject hitObject = hitInfo.collider.transform.gameObject;

            //Debug.Log("Raycast Hit: "+hitInfo.collider.gameObject.name+", Tag: "+hitObject.tag);
            if (Input.GetMouseButtonDown(0) && hitObject.tag == "Hex") {
                Debug.Log("Clicked Hex: "+ hitObject.GetComponent<HexCord>().x
                    +", "+ hitObject.GetComponent<HexCord>().y);
                //Debug.Log("Set Color");
                //MeshRenderer mr = hitObject.GetComponent<MeshRenderer>();
                //mr.material.color = Color.yellow;
            } else if (Input.GetMouseButtonDown(0)) {
                Debug.Log("Click Miss Hex");
            }


        }

    }
}
