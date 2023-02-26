using UnityEngine;

public class WoodCrate : MonoBehaviour, IInteractable
{
    public Message messageInteract => null;

    [SerializeField] private int slotSize = 34;
    private Stockage stockage;
    private TypeStockage typeStockage = TypeStockage.Chest;
    
    private void Awake() {
        stockage = new Stockage(slotSize);
    }

    public void Open()
    {
        StockageUI.instance.Set(stockage, typeStockage);
        StockageUI.instance.Open();
    }
}
