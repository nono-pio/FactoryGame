using UnityEngine;

public class StockageUI : MonoBehaviour
{
    [SerializeField] private GameObject stockageUI;
    [SerializeField] private GameObject itemUIPrefab;

    [SerializeField] public int maxStackItem;
    [SerializeField] private ItemUI[] items;

    private Stockage curStockage;
    private TypeStockage typeStockage;

    private Popup popup;
    
    public static StockageUI instance;
    private void Awake()
    {
        instance = this;
        popup = new Popup(stockageUI);
        popup.closeFunction = () => {curStockage.Set(Get());};
    }

    private void Start() 
    {
        Set(Inventory.instance.stockage, Inventory.instance.typeStockage);
        
        stockageUI.SetActive(false);
    }

    public void Set(Stockage stockage, TypeStockage stockageType)
    {
        if (stockage == curStockage && !stockage.isUpdate) return; // if same stockage don't recharge the UI

        curStockage = stockage;
        typeStockage = stockageType;

        Refresh();
    }

    public void Refresh()
    {
        for (int i = 0; i < items.Length; i++ )
        {
            if (curStockage.stockage[i] != null) // if need to set a item
            {
                items[i].InitialisationItem(curStockage.stockage[i].item, curStockage.stockage[i].count); // set item
            } else 
            {
                if (!items[i].isNull) // is a item
                {
                    items[i].DisableItem();
                }
            }
        }
    }

    public ItemStack[] Get()
    {
        ItemStack[] itemStacks = new ItemStack[items.Length];
        for (int i = 0; i < items.Length; i++)
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
        for (int i = items.Length - 1; i >= 0; i--)
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

    public int AddItems(Item newItem, int count = 1)
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
                        item.InitialisationItem(newItem, countToAdd);
                    } else
                    {
                        item.InitialisationItem(newItem, count);
                        return 0;
                    }
                } else
                {
                    item.InitialisationItem(newItem);
                    count--;
                    if (count <= 0) return 0;
                }
            }
        }
        return count;
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
