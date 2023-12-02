using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChitScrollController : MonoBehaviour
{
    [SerializeField]
    float rotateSpeed;
    [SerializeField]
    float scaleSpeed;
    [SerializeField]
    float changeHeightSpeed;

    void Update()
    {
        float scrollDirection = GetScrollInput();

        if (scrollDirection != 0)
        {
            // Handle the scroll input (e.g., zoom in/out)
            Debug.Log("Scroll Direction: " + scrollDirection);
        }
    }

    private void HandleScrollChit(float direction) {
        if (ChitManager.instance.selectedChits.Count <= 0)
            return;

        foreach (var chit in ChitManager.instance.selectedChits) {

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                IncreaseHeight(chit, direction);
            }
            else if (Input.GetKeyDown(KeyCode.LeftControl))
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
            chit.elevatedPosition.y += chit.boxCollider.size.y;
        }
        else {
            chit.elevatedPosition.y -= chit.boxCollider.size.y;
        }
    }

    private void Rotate(Chit chit, float direction) { 
    
    }

    private void Scale(Chit chit, float direction) { 
    
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
