using UnityEngine;

[CreateAssetMenu(menuName = "My project/Craft")]
public class ItemCraft : ScriptableObject
{

    [Header("Item")]
    public Item item = null;
    public int count = 1;
    public bool inCraftingTable;

    [Header("Resource")]
    [SerializeField]
    public ItemWithCount[] needItems;

    [System.Serializable]
    public class ItemWithCount
    {
        public Item item;
        public int count;
    }   
}