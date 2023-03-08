using UnityEngine;
using System.Collections;

public class StockageUI : MonoBehaviour
{
    [SerializeField] private GameObject stockageUI;
    [SerializeField] private GameObject itemUIPrefab;

    [SerializeField] public int maxStackItem;
    [SerializeField] private ItemUI[] items;

    [SerializeField] private GridLayout gridLayout;

    private Stockage curStockage;
    private TypeStockage typeStockage;

    private Popup popup;
    
    public static StockageUI instance;
    private void Awake()
    {
        instance = this;
        popup = new Popup(stockageUI);
        popup.closeFunction = () =>
        {
            curStockage.Set(Get());
        };
    }

    private void Start() 
    {
        Set(Inventory.instance.stockage, Inventory.instance.typeStockage);
        gridLayout.refreshEnable = false;
        
        stockageUI.SetActive(false);
    }

    public void Set(Stockage stockage, TypeStockage stockageType)
    {
        if (stockage == curStockage && !stockage.isUpdate) return; // if same stockage don't recharge the UI

        if (curStockage == null) SetSlot(0, stockage.slotCount);
        if (curStockage != null && curStockage.slotCount != stockage.slotCount)
        {
            SetSlot(curStockage.slotCount, stockage.slotCount);
        }

        curStockage = stockage;
        typeStockage = stockageType;

        Refresh();
        StartCoroutine(refreshGrid(0.01f));

        stockage.isUpdate = false;
    }

    public void Refresh()
    {
        for (int i = 0; i < curStockage.slotCount; i++ )
        {
            if (curStockage.stockage[i] != null) // if need to set a item
            {
                SetItem(items[i], curStockage.stockage[i].item, curStockage.stockage[i].count); // set item
            } else 
            {
                if (!items[i].isNull) // is a item
                {
                    items[i].DisableItem();
                }
            }
        }
    }

    private IEnumerator refreshGrid(float time)
    {
        yield return new WaitForSeconds(time);
        gridLayout.Refresh();
    }

    private void SetSlot(int curSlotCount, int newSlotCount)
    {
        if (curSlotCount < newSlotCount)
        {
            for (int i = curSlotCount; i < newSlotCount; i++)
            {
                items[i].transform.parent.gameObject.SetActive(true);
            }
        } else
        {
            for (int i = newSlotCount; i < curSlotCount; i++)
            {
                items[i].transform.parent.gameObject.SetActive(false);
            }
        }
    }

    public ItemStack[] Get()
    {
        ItemStack[] itemStacks = new ItemStack[curStockage.slotCount];
        for (int i = 0; i < curStockage.slotCount; i++)
        {
            if (!items[i].isNull)
                itemStacks[i] = new ItemStack(items[i].item, items[i].count);
            else
                itemStacks[i] = null;
        }
        return itemStacks;
    }

    public void RemoveItem(Item itemToRemove, int countToRemove)
    {
        for (int i = curStockage.slotCount - 1; i >= 0; i--)
        {
            if (items[i].gameObject.activeSelf && items[i].item == itemToRemove)
            {
                if(countToRemove >= items[i].count)
                {
                    countToRemove -= items[i].count;
                    items[i].DisableItem();
                } else
                {
                    items[i].count -= countToRemove;
                    items[i].RefreshCount();
                    return;
                }
            }
        }
    }

    public int AddItems(Item newItem, int count = 1) //Todo
    {
        // add item to all slot with same item for fill them
        foreach (var item in items)
        {
            if (item.gameObject.activeSelf && item.item.stackable && item.item == newItem && item.count < maxStackItem)
            {
                int spaceInSlot = maxStackItem - item.count;
                if (spaceInSlot < count)
                {
                    count -= spaceInSlot;
                    item.count = maxStackItem;
                    item.RefreshCount();
                } else
                {
                    item.count += count;
                    item.RefreshCount();
                    return 0; //no more item to add
                }
            }   
        }

        // add last items in empty slots
        foreach (var item in items)
        {
            int countToAdd;
            if (!item.gameObject.activeSelf) // no item
            {
                if (newItem.stackable)
                {
                    if (count > maxStackItem )
                    {
                        countToAdd = maxStackItem;
                        count -= maxStackItem;
                        SetItem(item, newItem, countToAdd);
                    } else
                    {
                        SetItem(item, newItem, count);
                        return 0;
                    }
                } else
                {
                    SetItem(item, newItem);
                    count--;
                    if (count <= 0) return 0;
                }
            }
        }
        return count;
    }

    private void SetItem(ItemUI item, Item newItem, int count = 1)
    {
        item.InitialisationItem(newItem, count);
        item.transform.parent.GetComponent<OneChildLayout>().Refresh();
        item.GetComponent<OneChildLayout>().Refresh();
    }

    public void Open()
    {
        if (stockageUI.activeInHierarchy)
        {
            PopupManager.instance.ClosePopup(popup);
        } else 
        {
            PopupManager.instance.OpenPopup(popup);
        }
    }
}

public enum TypeStockage
{
    PlayerInventory,
    Chest
}
