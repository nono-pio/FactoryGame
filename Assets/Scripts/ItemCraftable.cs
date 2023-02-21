using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemCraftable : MonoBehaviour
{
    [HideInInspector] public ItemCraft craft;
    private GameObject itemCraftableUI;

    public ItemCraftable InitialisationItemCraft(GameObject _itemCraftableUI, ItemCraft _craft)
    {
        itemCraftableUI = _itemCraftableUI;
        craft = _craft;

        Image icon = _itemCraftableUI.transform.GetChild(0).GetComponent<Image>();
        icon.sprite = craft.item.image;
        TextMeshProUGUI text = _itemCraftableUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        text.text = craft.item.name;
        Button button =  _itemCraftableUI.GetComponent<Button>();
        button.onClick.AddListener(onClick);

        return this;
    }

    private void onClick()
    {
        CraftingManager.instance.SetItemCraftDisplay(craft);
    }
}
