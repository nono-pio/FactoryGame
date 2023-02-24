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

    private void SetActiveItemCraft(bool isWorkbench)
    {
        foreach (var craft in itemCraftables)
        {
            if (isWorkbench) craft.gameObject.SetActive(true);
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
    }

    private void GetInventoryItems()
    {
        invCraftLog();
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
        Inventory.instance.stockage.AddItems(new ItemStack(craft.item, craftCount * craft.count)); // add item created (nbcraft * nbcrafted per time)
        Inventory.instance.stockage.PrintStockage();
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

    public void openCraft(bool isWorkbench)
    {
        if (craftingUI.activeInHierarchy)
            PopupManager.instance.ClosePopup(popup);
        else
        {
            GetInventoryItems();
            SetActiveItemCraft(isWorkbench);
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