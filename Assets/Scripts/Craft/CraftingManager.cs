using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftingManager : MonoBehaviour
{

    #region SerializeField
    
    [Header("UI")]

    [SerializeField] private GameObject craftingUI;
    [SerializeField] private TextMeshProUGUI ItemName, ItemCount;
    [SerializeField] private Image ItemIcon;
    [SerializeField] private TMP_InputField countInput;

    [Header("Parent and Prefab")]

    [SerializeField] private Transform parentItemsNeed;
    [SerializeField] private Transform parentItemsCraft;
    [SerializeField] private GameObject prefabItemNeed, prefabItemCraft;

    #endregion

    private ItemCraft[] crafts;
    private ItemCraft defaultCraft;

    private ItemCraftable[] itemCraftables;

    private ItemCraft curItemCraft;

    private Popup popup;

    private bool inWorkbench = false;

    // instance
    [HideInInspector] public static CraftingManager instance;
    private void Awake()
    {
        instance = this;
        popup = new Popup(craftingUI);
        popup.closeFunction = closeCraft;

        crafts = GameManager.instance.crafts;
        defaultCraft = crafts[GameManager.instance.defaultCraft];
    }

    private void Start() 
    {
        SetItemCraftDisplay(defaultCraft);
        SetItemCraft();
        craftingUI.SetActive(false);

        countInput.text = "1";
    }

    private void SetItemCraft()
    {
        List<ItemCraftable> _itemCraftables = new List<ItemCraftable>();
        foreach (var itemCraft in crafts)
        {
            GameObject newItemCraft = Instantiate(prefabItemCraft, parentItemsCraft);
            ItemCraftable itemCraftable = newItemCraft.GetComponent<ItemCraftable>();
            itemCraftable.InitialisationItemCraft(newItemCraft, itemCraft);
            _itemCraftables.Add(itemCraftable);
        }
        itemCraftables = _itemCraftables.ToArray();
    }

    private void MakeCraft(ItemCraft craft, int craftCount)
    {
        if (HasItems(craft, craftCount))
        {
            UpdateInv(craft, craftCount);
            invCraftLog();
        }
    }

    private bool HasItems(ItemCraft craft, int craftCount)
    {
        foreach (var needItem in craft.needItems)
            if (!Inventory.instance.stockage.HasItemStack(new ItemStack(needItem.item, needItem.count * craftCount))) // if item didn't exist or dosen't have enough
                return false;
    
        return true;
    }

    private void UpdateInv(ItemCraft craft, int craftCount)
    {
        foreach (var needItem in craft.needItems) // remove each item use for the craft 
        {
            Inventory.instance.stockage.RemoveItem(new ItemStack(needItem.item, needItem.count * craftCount));
        }
        int restItem = Inventory.instance.stockage.AddItems(new ItemStack(craft.item, craftCount * craft.count)); // add item created (nbcraft * nbcrafted per time)
        if (restItem > 0)
        {
            ItemManager.instance.dropItem(Player.instance.transform.position, new ItemStack(craft.item, restItem));
        }
    }

    #region DisplayFunction

    public void SetItemCraftDisplay(ItemCraft craft)
    {
        curItemCraft = craft;

        ItemName.text = craft.item.name;
        ItemCount.text = craft.count.ToString();
        ItemIcon.sprite = craft.item.image;

        SetItemNeedDisplay(craft);
    }

    private void SetItemNeedDisplay(ItemCraft craft)
    {
        for (int i = 0; i < parentItemsNeed.childCount; i++)
            Destroy(parentItemsNeed.GetChild(i).gameObject);
        
        for (int i = 0; i < craft.needItems.Length; i++)
        {
            GameObject itemNeedUI = Instantiate(prefabItemNeed, parentItemsNeed);
            itemNeedUI.transform.GetChild(0).GetComponent<Image>().sprite = craft.needItems[i].item.image;
            itemNeedUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = craft.needItems[i].item.name;
            itemNeedUI.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = craft.needItems[i].count.ToString();
        }
    }
    
    public void SetActiveItemCraft(CraftType craftType)
    { 
        if(itemCraftables == null) return;
        foreach (var craft in itemCraftables) //test working bench
        {
            if (inWorkbench)
            {
                craft.gameObject.SetActive(true);
            }
            else
            {
                if (!craft.craft.inCraftingTable)
                {
                    craft.gameObject.SetActive(true);
                } else
                {
                    craft.gameObject.SetActive(false);
                }
            }
        }

        // Test craftType Selected
        if (craftType == CraftType.All) return;
        foreach (var craft in itemCraftables)
        {
            if (craft.craft.typeOfCraft != craftType) craft.gameObject.SetActive(false);
        }
    }

    #endregion

    #region BUTTON

    int count = 1;

    public void SetCount(string inputString)
    {
        int.TryParse(inputString, out count);
    }

    public void MakeCraftBtn()
    {
        MakeCraft(curItemCraft, count);
    }

    public void AddCount()
    {
        count++;
        countInput.text = count.ToString();
    }

    public void RemoveCount()
    {
        if (count > 1)
        {
            count--;
            countInput.text = count.ToString();
        }
    }

    public void MaxCount()
    {
        int maxCount = 999_999_999;
        foreach (var needItem in curItemCraft.needItems)
        {
            int inventoryCount = Inventory.instance.stockage.GetCount(needItem.item);
            int maxCraftForNeedItem = Mathf.FloorToInt((float) inventoryCount / needItem.count);
            if (maxCraftForNeedItem < maxCount) maxCount = maxCraftForNeedItem;
        }

        if (maxCount < 1 || maxCount == 999_999_999) count = 1;
        else count = maxCount;

        countInput.text = count.ToString();
    }

    public void openCraft(bool isWorkbench)
    {
        inWorkbench = isWorkbench;
        if (craftingUI.activeInHierarchy)
            PopupManager.instance.ClosePopup(popup);
        else
        {
            SetActiveItemCraft(CraftType.All);
            PopupManager.instance.OpenPopup(popup);
        }
    }

    public void closeCraft()
    {
        
    }
    
    #endregion

    #region Debug

    private void invCraftLog()
    {
        Inventory.instance.stockage.PrintStockage();
    }
    #endregion
}