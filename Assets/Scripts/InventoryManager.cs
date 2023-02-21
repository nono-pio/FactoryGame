using UnityEngine;
using System.Collections.Generic;
using System;

public class InventoryManager : MonoBehaviour
{

    // create a instance of the inventory
    public static InventoryManager instance;
    private void Awake() 
    {
        instance = this;
        popup = new Popup(inventoryUI);
        popup.closeFunction = closeInventory;
    }

    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private GameObject inventoryItemPrefab;

    [SerializeField] private int maxStackItem;
    [SerializeField] private InventorySlot[] inventorySlots;

    [HideInInspector] public BackStorage backStorage = new BackStorage();

    private Popup popup;

    private void Start()
    {
        inventoryUI.SetActive(false);
    }

    public int AddItems(Item item, int count = 1)
    {
        backStorage.AddItemStack(new ItemStack(item, count));
        // add item to all slot with same item for fill them
        foreach (var slot in inventorySlots)
        {
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && itemInSlot.item.stackable && itemInSlot.item == item && itemInSlot.count < maxStackItem)
            {
                int spaceInSlot = maxStackItem - itemInSlot.count;
                if (spaceInSlot < count)
                {
                    count -= spaceInSlot;
                    itemInSlot.count = maxStackItem;
                    itemInSlot.RefreshCount();
                } else
                {
                    itemInSlot.count += count;
                    itemInSlot.RefreshCount();
                    return 0; //no more item to add
                }
            }   
        }

        // add last items in empty slots
        foreach (var slot in inventorySlots)
        {
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            int countToAdd;
            if (itemInSlot == null)
            {
                if (count > maxStackItem )
                {
                    countToAdd = maxStackItem;
                    count -= maxStackItem;
                    SpawnNewItem(item, slot, countToAdd);
                } else
                {
                    SpawnNewItem(item, slot, count);
                    return 0;
                }
            }
        }

        backStorage.RemoveItemStack(new ItemStack(item, count));
        return count;
    }

    public void refreshItemAndCount()
    {
        backStorage.Clear();
        foreach (var slot in inventorySlots)
        {
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null)
                backStorage.AddItemStack(new ItemStack(itemInSlot.item, itemInSlot.count));
        }
    }

    public void SpawnNewItem(Item item, InventorySlot slot, int count = 1)
    {
        GameObject newItemGameObject = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGameObject.GetComponent<InventoryItem>();
        inventoryItem.InitialisationItem(item, count);
    }

    public void RemoveItem(Item itemToRemove, int countToRemove)
    {
        backStorage.RemoveItemStack(new ItemStack(itemToRemove, countToRemove));
        for (int i = inventorySlots.Length - 1; i >= 0; i--)
        {
            InventoryItem itemInSlot = inventorySlots[i].GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && itemInSlot.item == itemToRemove)
            {
                if(countToRemove >= itemInSlot.count)
                {
                    countToRemove -= itemInSlot.count;
                    Destroy(itemInSlot.gameObject);
                } else
                {
                    itemInSlot.count -= countToRemove;
                    itemInSlot.RefreshCount();
                    return;
                }
            }
        }
    }

    public void openInventory()
    {
        if (inventoryUI.activeInHierarchy)
        {
            PopupManager.instance.ClosePopup(popup);
        } else
        {
            PopupManager.instance.OpenPopup(popup);
        }
    }

    public void closeInventory()
    {
        
    }
}
