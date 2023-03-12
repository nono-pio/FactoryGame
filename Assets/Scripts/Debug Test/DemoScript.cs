using UnityEngine;

public class DemoScript : MonoBehaviour
{
    [SerializeField] private Item[] itemsToPickup;
    [SerializeField] private int indexItemToPickup;
    [SerializeField] private int countToAdd;

    [SerializeField] private bool removeAll;

    [SerializeField] private CircleLayout circleLayout;

    private void Start() {SetInputDeguger(InputManager.instance.GetDebugInput());}

    private void SetInputDeguger(AllInput.DebugActions debugInput)
    {
        debugInput.action0.performed += ctx => Inventory.instance.Open();
        debugInput.action1.performed += ctx => StockageUI.instance.AddItems(itemsToPickup[indexItemToPickup], countToAdd);
        debugInput.action2.performed += ctx => StockageUI.instance.RemoveItem(itemsToPickup[indexItemToPickup], countToAdd);
        debugInput.action3.performed += ctx => ItemManager.instance.dropItem(Vector2.zero, new ItemStack(itemsToPickup[indexItemToPickup], countToAdd));
        debugInput.action4.performed += ctx => Inventory.instance.stockage.PrintStockage();
        debugInput.action5.performed += ctx => MessageManager.instance.AddMessage(new Message("msg 1","bla"));
        debugInput.action6.performed += ctx => {};
        debugInput.action7.performed += ctx => {};
        debugInput.action8.performed += ctx => {};
        debugInput.action9.performed += ctx => {};
    }
}
