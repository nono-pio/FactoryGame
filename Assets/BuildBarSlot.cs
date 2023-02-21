using UnityEngine;
using UnityEngine.EventSystems;

public class BuildBarSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
            inventoryItem.parentAfterDrag = transform;
            inventoryItem.dragFunction = BuildManager.instance.RefreshBuildBar;
        }
    }
}
