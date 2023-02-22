using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "My project/Item", order = 0)]
public class Item : ScriptableObject
{
    
    [Header("Only Gameplay")]
    public TileBase[] tile = null;
    public ItemType type = ItemType.JustItem;
    public GameObject prefabInteractable = null;

    [Header("Only UI")]
    public bool stackable = true;

    [Header("Both")]
    public Sprite image;
}

public enum ItemType
{
    Tool,
    Build,
    JustItem,
    Interactable
}