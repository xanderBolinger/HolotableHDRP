using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChitScrollController : MonoBehaviour
{
    [SerializeField, Range(1f, 360f)]
    float rotateSpeed;
    [SerializeField, Range(1f, 5f)]
    float scaleSpeed;
    [SerializeField, Range(0.1f, 1f)]
    float changeHeightSpeed;

    void Update()
    {
        float scrollDirection = GetScrollInput();

        if (scrollDirection != 0)
        {
            // Handle the scroll input (e.g., zoom in/out)
            Debug.Log("Scroll Direction: " + scrollDirection);
            HandleScrollChit(scrollDirection);
        }
    }

    private void HandleScrollChit(float direction) {
        if (ChitManager.instance.selectedChits.Count <= 0)
            return;

        foreach (var chit in ChitManager.instance.selectedChits) {
           
            if (Input.GetKey(KeyCode.LeftShift))
            {
                IncreaseHeight(chit, direction);
            }
            else if (Input.GetKey(KeyCode.LeftControl))
            {
                Scale(chit, direction);
            }
            else {
                Rotate(chit, direction);
            }
        
        }

    }

    private void IncreaseHeight(Chit chit, float direction) {
        Debug.Log("Change height");
        if (direction > 0)
        {
            chit.elevatedPosition.y += chit.boxCollider.size.y * changeHeightSpeed;
        }
        else {
            chit.elevatedPosition.y -= chit.boxCollider.size.y * changeHeightSpeed;
        }
    }

    private void Rotate(Chit chit, float direction)
    {
        Debug.Log("Rotate");
        float rotationAmount = rotateSpeed * direction;
        chit.transform.Rotate(Vector3.up, rotationAmount);
    }

    private void Scale(Chit chit, float direction)
    {
        Debug.Log("Scale");
        float scaleAmount = scaleSpeed * direction * Time.deltaTime;

        Vector3 newScale = chit.transform.localScale + new Vector3(scaleAmount, scaleAmount, scaleAmount);

        // Ensure the new scale is within a reasonable range
        newScale = Vector3.Max(newScale, new Vector3(0.1f, 0.1f, 0.1f));
        newScale = Vector3.Min(newScale, new Vector3(3f, 3f, 3f));

        chit.transform.localScale = newScale;
    }

    private float GetScrollInput()
    {
        float scrollInput = Input.mouseScrollDelta.y;

        if (scrollInput > 0)
        {
            return 1f; // Scroll Up
        }
        else if (scrollInput < 0)
        {
            return -1f; // Scroll Down
        }

        return 0f; // No Scroll
    }

}
