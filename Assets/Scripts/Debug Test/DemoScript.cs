using UnityEngine;

public class DemoScript : MonoBehaviour
{
    [SerializeField] private Item[] itemsToPickup;
    [SerializeField] private int indexItemToPickup;
    [SerializeField] private int countToAdd;

    [SerializeField] private InventorySlot slot;
    [SerializeField] private bool removeAll;

    [SerializeField] private GameObject prefabItemDrop;

    private void Start() {SetInputDeguger(InputManager.instance.GetDebugInput());}

    private void SetInputDeguger(AllInput.DebugActions debugInput)
    {
        debugInput.action0.performed += ctx => InventoryManager.instance.AddItems(itemsToPickup[indexItemToPickup], countToAdd);
        debugInput.action1.performed += ctx => InventoryManager.instance.RemoveItem(itemsToPickup[indexItemToPickup], countToAdd);;
        debugInput.action2.performed += ctx => {
            GameObject item = Instantiate(prefabItemDrop);
            ItemOnGround scriptItem = item.GetComponentInChildren<ItemOnGround>();
            scriptItem.InstantiateItem(itemsToPickup[indexItemToPickup]);
            };
        debugInput.action3.performed += ctx => InventoryManager.instance.backStorage.PrintStockage();
        debugInput.action4.performed += ctx => MessageManager.instance.AddMessage(new Message("msg 1","bla"));
        debugInput.action5.performed += ctx => MessageManager.instance.AddMessage(new Message("msg 2","alb"));
        debugInput.action6.performed += ctx => CraftingManager.instance.openCraft(true);
        debugInput.action7.performed += ctx => {};
        debugInput.action8.performed += ctx => {};
        debugInput.action9.performed += ctx => {};
    }
}
