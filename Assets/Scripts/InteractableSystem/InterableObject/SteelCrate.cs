using UnityEngine;

public class SteelCrate : MonoBehaviour, IInteractable
{
    public Message messageInteract => null;

    public void Open()
    {
        Inventory.instance.Open();
    }
}
