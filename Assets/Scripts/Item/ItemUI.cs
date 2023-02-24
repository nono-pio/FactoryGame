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
    [HideInInspector] public bool isNull = true;

    private void Awake() {
        parentTransform = transform.parent;
    }

    public void InitialisationItem(Item newItem, int _count = 1)
    {
        InitialisationItemWithoutActive(newItem, _count);
        ActiveItemUI();
    }

    public void InitialisationItemWithoutActive(Item newItem, int _count = 1)
    {
        item = newItem;
        image.sprite = newItem.image;
        count = _count;
        RefreshCount();
    }

    public void ActiveItemUI()
    {
        isNull = false;
        gameObject.SetActive(true);
    }

    public void DisableItem()
    {
        isNull = true;
        gameObject.SetActive(false);
    }

    public void RefreshCount()
    {
        countText.text = count.ToString();
        countText.gameObject.SetActive(count > 1); // active the display count if is bigger than 1
    }

    #region DragSystem

    [HideInInspector] public ItemUI newDropItem;

    [HideInInspector] public Transform parentTransform;

    [HideInInspector] public delegate void DragFunction();
    [HideInInspector] public DragFunction dragFunction;
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        image.raycastTarget = false;
        transform.SetParent(transform.root);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;
        transform.SetParent(parentTransform);

        if (newDropItem != null)
        {
            DisableItem();
            newDropItem.InitialisationItem(item, count);
            dragFunction();
            newDropItem = null;
        }
    }

    #endregion
}
