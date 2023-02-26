using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    
    [SerializeField] private GameObject itemOnGroundPrefab;
    [SerializeField] private Transform parentItemOnGround;

    [HideInInspector] public static ItemManager instance;
    private void Awake() {
        instance = this;
    }

    public void dropItem(Vector2 worldPosition, ItemStack itemDrop)
    {
        GameObject item = Instantiate(itemOnGroundPrefab, parentItemOnGround);
        item.transform.position = worldPosition;

        ItemOnGround scriptItem = item.GetComponentInChildren<ItemOnGround>();
        scriptItem.InstantiateItem(itemDrop.item, itemDrop.count);
    }
}
