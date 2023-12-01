using UnityEngine;

public class Chit : MonoBehaviour
{
    [SerializeField]
    float speed = 1f;

    Rigidbody rb;
    
    Vector3 elevatedPosition;
    bool elevated;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (elevated) {
            var step = speed * Time.fixedDeltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, elevatedPosition, step);

            /*if (Vector3.Distance(elevatedPosition, transform.position) > 0.001f)
                rb.velocity = Vector3.up * step * 100f;
            else { 
                rb.velocity = Vector3.zero;
                transform.position = elevatedPosition;
            }*/

            //transform.position = elevatedPosition;
        }
    }

    private void OnMouseDown()
    {
        var newPos = transform.position;
        newPos.y += 1f;
        elevatedPosition = newPos;
        elevated = true;
        rb.useGravity = false;
    }

    private void OnMouseUp()
    {
        elevated = false;
        rb.useGravity = true;
    }

}
