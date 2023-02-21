using UnityEngine;
using UnityEngine.InputSystem;

public class DemoScript : MonoBehaviour
{
    [SerializeField] private Item[] itemsToPickup;
    [SerializeField] private int indexItemToPickup;
    [SerializeField] private int countToAdd;

    [SerializeField] private InventorySlot slot;
    [SerializeField] private bool removeAll;

    [SerializeField] private GameObject prefabItemDrop;

    public void PickupItem()
    {
        int restCount = InventoryManager.instance.AddItems(itemsToPickup[indexItemToPickup], countToAdd);
        if (restCount == 0)
            Debug.Log("item added");
        else
            Debug.Log("Inventory full rest " + restCount + " items");
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
            PickupItem();
        else if (Input.GetKeyDown(KeyCode.D))
            InventoryManager.instance.RemoveItem(itemsToPickup[indexItemToPickup], countToAdd);
        else if (Input.GetKeyDown(KeyCode.H))
        {
            GameObject item = Instantiate(prefabItemDrop);
            ItemOnGround scriptItem = item.GetComponentInChildren<ItemOnGround>();
            scriptItem.InstantiateItem(itemsToPickup[indexItemToPickup]);
        } else if (Input.GetKeyDown(KeyCode.J))
            InventoryManager.instance.backStorage.PrintStockage();
    }
}
