using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;

    [HideInInspector] public MovePlayer movePlayer;
    [HideInInspector] public Inventory inventory;

    private void Awake()
    {
        instance = this;
        movePlayer = GetComponent<MovePlayer>();
        inventory = transform.Find("Inventory").GetComponent<Inventory>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            ItemOnGround item = collision.GetComponentInChildren<ItemOnGround>();
            int restItem = Inventory.instance.stockage.AddItems(new ItemStack(item.item, item.count));
            if (restItem == 0)
            {
                MessageManager.instance.AddMessage(new Message("Some " + item.item.name, "You got " + item.count + " " + item.item.name + "."));
                Destroy(collision.gameObject);
            }
            else
            {
                item.count = restItem;
                MessageManager.instance.AddMessage(new Message("Inventory Full", "You can't get " + restItem + " " + item.item.name + " !!"));
            }
        }
    }
}
