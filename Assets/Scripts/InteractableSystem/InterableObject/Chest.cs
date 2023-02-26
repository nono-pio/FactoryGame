using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    public Message messageInteract => null;
    [SerializeField] private int slotSize = 24;

    private Stockage stockage;
    private TypeStockage typeStockage = TypeStockage.Chest;

    private void Awake() {
        stockage = new Stockage(slotSize);
    }

    public void Open()
    {
        StockageUI.instance.Open();
        StockageUI.instance.Set(stockage, typeStockage);
    }
}
