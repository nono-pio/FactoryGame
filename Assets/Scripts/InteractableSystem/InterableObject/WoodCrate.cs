using UnityEngine;

public class WoodCrate : MonoBehaviour, IInteractable
{
    public Message messageInteract => null;

    public void Open()
    {
        Inventory.instance.Open();
    }
}
