using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggableElement : MonoBehaviour, IDragHandler
{
    [SerializeField] private Transform worldCollider;

    private RectTransform canvasElement;

    private void Awake()
    {
        canvasElement = GetComponent<RectTransform>();
    }

    void Update()
    {
        Vector3 position = Camera.main.ScreenToWorldPoint(canvasElement.position);
        position.z = 0;

        worldCollider.position = position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }
}
