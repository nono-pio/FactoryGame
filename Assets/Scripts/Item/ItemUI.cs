using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ItemUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    
    [Header("UI")]
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI countText;

    [HideInInspector] public Item item;
    [HideInInspector] public int count = 1;

    [HideInInspector] public Transform parentAfterDrag;

    [HideInInspector] public delegate void DragFunction();
    [HideInInspector] public DragFunction dragFunction;

    public void InitialisationItem(Item newItem, int _count = 1)
    {
        item = newItem;
        image.sprite = newItem.image;
        count = _count;
        RefreshCount();
    }

    public void RefreshCount()
    {
        countText.text = count.ToString();
        countText.gameObject.SetActive(count > 1); // active the display count if is bigger than 1
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        image.raycastTarget = false;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;
        transform.SetParent(parentAfterDrag);
        if (dragFunction != null) dragFunction();
    }
}
