using UnityEngine;
using UnityEngine.EventSystems;

public class BuildBarSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            ItemUI inventoryItem = eventData.pointerDrag.GetComponent<ItemUI>();
            inventoryItem.parentAfterDrag = transform;
            inventoryItem.dragFunction = BuildManager.instance.RefreshBuildBar;
        }
    }
}
