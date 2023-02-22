using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    public Message messageInteract => null;

    public void Open()
    {
        InventoryManager.instance.openInventory();
    }
}
