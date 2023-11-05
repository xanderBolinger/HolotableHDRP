using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HexElevationManager : MonoBehaviour
{
    public static HexElevationManager hexElevationManager;

    [SerializeField] Transform elevationCanvas;
    [SerializeField] GameObject textPrefab;
    public int setElevationManual;
    public bool pasteElevation;


    Dictionary<Vector2Int, TextMeshProUGUI> elevationMarkers;

    private void Awake()
    {
        hexElevationManager = this;
        elevationMarkers = new Dictionary<Vector2Int, TextMeshProUGUI>();
    }


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

    public void SetText(Vector2Int cord, int elevation) {
        if (!elevationMarkers.ContainsKey(cord))
            return;
        elevationMarkers[cord].text = elevation.ToString();
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
        ClearMarkers();

        var hexCords = FindObjectsOfType<HexCord>();


        foreach (var cord in hexCords) {

            var pos = cord.gameObject.transform.position;
            pos.y += 0.1f;
            var vector2 = cord.GetCord();
            var newMarker = Instantiate(textPrefab, pos, Quaternion.identity, elevationCanvas);
            var uiText = newMarker.GetComponent<TextMeshProUGUI>();
            uiText.text=cord.elevation.ToString();
            elevationMarkers.Add(vector2, uiText);
        }

    }

    public void HideElevation()
    {
        ClearMarkers();
    }

    void ClearMarkers() {
        foreach (var value in elevationMarkers.Values) {
            Destroy(value.gameObject);
        }
        elevationMarkers.Clear();
    }

}
