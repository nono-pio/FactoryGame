using UnityEngine;

[CreateAssetMenu(menuName = "My project/Craft")]
public class ItemCraft : ScriptableObject
{

    [Header("Item")]
    public Item item = null;
    public int count = 1;
    public bool inCraftingTable;
    public CraftType typeOfCraft = CraftType.Other;

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

public enum CraftType
{
    Food,
    Factory,
    Stockage,
    Other,
    All
}