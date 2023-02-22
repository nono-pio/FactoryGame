using System.Collections.Generic;
using UnityEngine;

public class BackStorage
{
    private List<Item> items = new List<Item>();
    private List<int> counts = new List<int>();

    public bool isItem(Item item) => items.Contains(item);
    public ItemStack GetItemStack(Item item)
    {
        int index = items.IndexOf(item);
        if (index < 0) return null;
        else return new ItemStack(items[index], counts[index]);
    }
    public void AddItemStack(ItemStack itemStack)
    {
        int index = items.IndexOf(itemStack.item);
        if (index < 0)
        {
            items.Add(itemStack.item);
            counts.Add(itemStack.count);
        } else
        {
            counts[index] += itemStack.count;
        }
    }
    public int RemoveItemStack(ItemStack itemStack)
    {
        int index = items.IndexOf(itemStack.item);
        if (index < 0) return itemStack.count;
        else
        {
            counts[index] -= itemStack.count;
            if(counts[index] <= 0)
            {
                int restCount = -counts[index];
                items.RemoveAt(index);
                counts.RemoveAt(index);
                return restCount;
            }
            return 0;
        }
    }
    public void SetItemsCounts(Item[] _items, int[] _counts)
    {
        items.Clear();
        counts.Clear();
        for (int i = 0; i < _items.Length; i++)
            AddItemStack(new ItemStack(_items[i], _counts[i]));
    }
    public void Clear()
    {
        items.Clear();
        counts.Clear();
    }
    public bool HasCount(Item item, int needCount)
    {
        int index = items.IndexOf(item);
        if (index < 0 || counts[index] < needCount) return false;
        return true;
    }
    public int Length() => items.Count;

    public void PrintStockage()
    {
        string msg = "", msg1 = "";
        for (int j = 0; j < items.Count; j++)
        {
            msg += items[j].name + "/";
            msg1 += counts[j] + "/";
        }
        Debug.Log(msg);
        Debug.Log(msg1);
    }
}

public class ItemStack
    {
        public Item item;
        public int count;
        public ItemStack(Item _item, int _count)
        {
            item = _item;
            count = _count;
        }
    }