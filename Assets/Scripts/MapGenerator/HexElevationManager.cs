using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexElevationManager : MonoBehaviour
{
    public int setElevationManual;
    public bool pasteElevation;

    private void Update()
    {
        if (!pasteElevation || !Input.GetMouseButton(0))
            return;

        var hit = GetClickedObject();
        if (hit != null && hit.tag == "Hex")
        {
            Debug.Log("Set elevation: " + setElevationManual);
            var cord = HexCord.GetHexCord(hit);
            cord.elevation = setElevationManual;
        }
    }

    private GameObject GetClickedObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo))
        {
            return hitInfo.collider.transform.gameObject;
        }

        return null;
    }

    public void ShowElevation() { 
    
        var hexCords = FindObjectsOfType<HexCord>();
        
        foreach(var hexCord in hexCords) 
            hexCord.ShowElevationText();
    
    }

    public void HideElevation()
    {

        var hexCords = FindObjectsOfType<HexCord>();

        foreach (var hexCord in hexCords)
            hexCord.HideElevationText();

    }


}
