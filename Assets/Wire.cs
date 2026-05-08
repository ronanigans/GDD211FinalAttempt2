using UnityEngine;
using UnityEngine.EventSystems;

public class Wire : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public string wireID;
    public RectTransform rect; // Assign the stretching wire's RectTransform
    public PlayerScript playerScript;
    public bool isLocked = false;

    // Call this to reset the wire to its starting state
    public void ResetWire()
    {
        isLocked = false;
        if (rect != null)
        {
            rect.sizeDelta = new Vector2(0, rect.sizeDelta.y);
            rect.rotation = Quaternion.identity;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isLocked) return;
        Vector2 direction = eventData.position - (Vector2)transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rect.rotation = Quaternion.Euler(0, 0, angle);
        float distance = direction.magnitude / GetComponentInParent<Canvas>().scaleFactor;
        rect.sizeDelta = new Vector2(distance, rect.sizeDelta.y);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isLocked) return;
        
        GameObject[] sockets = GameObject.FindGameObjectsWithTag("WireSocket");
        foreach (GameObject socketGO in sockets)
        {
            float distance = Vector2.Distance(Input.mousePosition, socketGO.transform.position);
            Wire socket = socketGO.GetComponent<Wire>();
            if (socket != null && socket.wireID == this.wireID && distance < 50f)
            {
                Vector2 finalDir = (Vector2)socketGO.transform.position - (Vector2)transform.position;
                rect.sizeDelta = new Vector2(finalDir.magnitude / GetComponentInParent<Canvas>().scaleFactor, rect.sizeDelta.y);
                isLocked = true;
                playerScript.RegisterConnection();
                return;
            }
        }
        rect.sizeDelta = new Vector2(0, rect.sizeDelta.y);
    }
}