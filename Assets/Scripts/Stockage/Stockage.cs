public class Stockage
{
    private BackStorage backStorage = new BackStorage();
    public ItemStack[] stockage;
    public bool isUpdate = false;
    public int slotCount;

    public Stockage(int nbSlot)
    {
        slotCount = nbSlot;
        stockage = new ItemStack[nbSlot];
    }

    public void Set(ItemStack[] newStockage)
    {
        stockage = newStockage;

        backStorage.Clear();
        foreach (var itemStack in stockage)
        {
            if (itemStack != null)
                backStorage.AddItemStack(itemStack);
        }
    }
    public ItemStack[] Get() { return stockage; }

    public int RemoveItem(ItemStack itemStackToRemove)
    {
        isUpdate = true;
        backStorage.RemoveItemStack(itemStackToRemove);
        
        int restCountToRemove = itemStackToRemove.count;
        for (int i = stockage.Length - 1; i >= 0; i--)
        {
            if (stockage[i] != null && stockage[i].item == itemStackToRemove.item)
            {
                if(restCountToRemove >= stockage[i].count)
                {
                    restCountToRemove -= stockage[i].count;
                    stockage[i] = null;
                } else
                {
                    stockage[i].count -= restCountToRemove;
                    return 0;
                }
            }
        }
        return restCountToRemove;
    }

    public int AddItems(ItemStack itemStackToAdd)
    {
        isUpdate = true;
        backStorage.AddItemStack(itemStackToAdd);
        
        int restCountToAdd = itemStackToAdd.count;
        // add item to all slot with same item for fill them
        foreach (var itemStack in stockage)
        {
            if (   itemStack != null // is item
                && itemStack.item.stackable // is stackable 
                && itemStack.item == itemStackToAdd.item // is same item
                && itemStack.count < StockageUI.instance.maxStackItem) // is not full
            {
                int spaceInSlot = StockageUI.instance.maxStackItem - itemStack.count;
                if (spaceInSlot < restCountToAdd)
                {
                    restCountToAdd -= spaceInSlot;
                    itemStack.count = StockageUI.instance.maxStackItem;
                } else
                {
                    itemStack.count += restCountToAdd;
                    return 0; //no more item to add
                }
            }   
        }

        // add last items in empty slots
        for (int i = 0; i < stockage.Length; i++)
        {
            int countToAdd;
            if (stockage[i] == null) // no item
            {
                if (itemStackToAdd.item.stackable)
                {
                    if (restCountToAdd > StockageUI.instance.maxStackItem )
                    {
                        countToAdd = StockageUI.instance.maxStackItem;
                        restCountToAdd -= StockageUI.instance.maxStackItem;
                        stockage[i] = new ItemStack(itemStackToAdd.item, countToAdd);
                    } else
                    {
                        stockage[i] = new ItemStack(itemStackToAdd.item, restCountToAdd);
                        return 0;
                    }
                } else
                {
                    stockage[i] = new ItemStack(itemStackToAdd.item, 1);
                    restCountToAdd--;
                    if (restCountToAdd <= 0) return 0;
                }
            }
        }
        return restCountToAdd;
    }

    public int GetCount(Item item)
    {
        ItemStack itemStack = backStorage.GetItemStack(item);
        return (itemStack == null) ? 0 : itemStack.count;
    }

    public bool HasItemStack(ItemStack itemStack) => backStorage.HasCount(itemStack.item, itemStack.count);
    public void PrintStockage() => backStorage.PrintStockage();
}
