using UnityEngine;

public class Workbench : MonoBehaviour, IInteractable
{
    public Message messageInteract => null;

    public void Open()
    {
        CraftingManager.instance.openCraft(true);
    }
}
