using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler
{
    [SerializeField] private TypeSlot typeSlot;

    private ItemUI itemUI;

    private void Awake()
    {
        itemUI = transform.GetChild(0).GetComponent<ItemUI>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount < 1 || itemUI.isNull)
        {
            ItemUI itemPointedUI = eventData.pointerDrag.GetComponent<ItemUI>();
            itemPointedUI.newDropItem = itemUI;
            itemPointedUI.dragFunction = DragFunction;
        }
    }

    private void DragFunction()
    {
        switch (typeSlot)
        {
            case TypeSlot.StockageUI: break;
            case TypeSlot.BuildBar:
                BuildManager.instance.RefreshBuildBar();
                break;
        }
    }
}

enum TypeSlot
{
    StockageUI,
    BuildBar
}