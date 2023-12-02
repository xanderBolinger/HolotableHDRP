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
        if (elevated)
        {
            var step = speed * Time.fixedDeltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, elevatedPosition, step);
        }
    }

    private void OnMouseDown()
    {
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var hits = Physics.BoxCastAll(transform.position, new Vector3(0.5f, 0.5f, 0.5f), Vector3.up, transform.rotation, LayerMask.GetMask("Chit"));

        foreach(var hit in hits) {
            Chit clickedChit = hit.collider.GetComponent<Chit>();
            if (clickedChit != null)
            {
                clickedChit.ApplyClickEffect();
            }
        }
    }

    private void OnMouseUp()
    {
        foreach (var chit in ChitManager.instance.selectedChits) {
            chit.ApplyMouseUpEffects();
        }

        ChitManager.instance.selectedChits.Clear();
    }

    private void ApplyClickEffect()
    {
        var newPos = transform.position;
        newPos.y += 1f;
        elevatedPosition = newPos;
        elevated = true;
        rb.useGravity = false;
        ChitManager.instance.selectedChits.Add(this);
    }

    private void ApplyMouseUpEffects() {
        elevated = false;
        rb.useGravity = true;
    }
}
