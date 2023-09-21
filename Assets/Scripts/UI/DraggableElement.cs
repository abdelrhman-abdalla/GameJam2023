using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableElement : MonoBehaviour, IDragHandler
{
    [SerializeField] private Transform worldCollider;

    void Update()
    {
        Vector3 position = Camera.main.ScreenToWorldPoint(transform.position);
        position.z = 0;

        worldCollider.position = position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }
}
