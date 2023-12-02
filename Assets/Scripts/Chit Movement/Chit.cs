using UnityEngine;

public class Chit : MonoBehaviour
{
    [SerializeField]
    float speed = 1f;

    Rigidbody rb;

    [HideInInspector]
    public BoxCollider boxCollider;

    [HideInInspector]
    public Vector3 elevatedPosition;
    
    bool elevated;
    bool drawGizmos;

    Vector3 mousePosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
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
        var hits = Physics.BoxCastAll(boxCollider.bounds.center, boxCollider.bounds.size/2, Vector3.up, transform.rotation, LayerMask.GetMask("Chit"));
        Debug.DrawRay(transform.position, Vector3.up*3, Color.red, 1f);
        foreach(var hit in hits) {

            Chit clickedChit = hit.collider.GetComponent<Chit>();
            if (clickedChit != null && clickedChit.transform.position.y >= transform.position.y)
            {
                //Debug.Log("Apply click effects, hit: "+hit.transform.gameObject.name);
                clickedChit.ApplyClickEffect();
            }
        }
        
    }

    private Vector3 GetMousePos() {
        return Camera.main.WorldToScreenPoint(transform.position);
    }

    private void OnMouseUp()
    {
        foreach (var chit in ChitManager.instance.selectedChits) {
            chit.ApplyMouseUpEffects();
        }

        ChitManager.instance.selectedChits.Clear();
    }

    private void OnMouseDrag()
    {
        foreach (var chit in ChitManager.instance.selectedChits) {
            var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition - chit.mousePosition);
            pos.y = chit.transform.position.y;
            chit.transform.position = pos;
        }
    }

    private void ApplyClickEffect()
    {
        var newPos = transform.position;
        newPos.y += 1f;
        elevatedPosition = newPos;
        elevated = true;
        rb.useGravity = false;
        ChitManager.instance.selectedChits.Add(this);
        drawGizmos = true;
        mousePosition = Input.mousePosition - GetMousePos();
    }

    private void ApplyMouseUpEffects() {
        elevated = false;
        rb.useGravity = true;
        drawGizmos = false;
    }

    private void OnDrawGizmos()
    {
        if (!drawGizmos)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center, boxCollider.bounds.size);
    }

}
