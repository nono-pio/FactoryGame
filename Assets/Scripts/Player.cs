using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;

    [HideInInspector] public MovePlayer movePlayer;

    private void Awake()
    {
        instance = this;
        movePlayer = GetComponent<MovePlayer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(" Something Enter ");
        if (collision.CompareTag("Item"))
        {
            ItemOnGround item = collision.GetComponentInChildren<ItemOnGround>();
            Debug.Log("Item Enter");
            int restItem = InventoryManager.instance.AddItems(item.item, item.count);
            if (restItem == 0)
                Destroy(collision.gameObject);
            else
                item.count = restItem;
        }
    }
}
