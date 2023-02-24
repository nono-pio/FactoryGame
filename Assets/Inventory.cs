using UnityEngine;

public class Inventory : MonoBehaviour
{
    [HideInInspector] public Stockage stockage = new Stockage(24);
    [HideInInspector] public TypeStockage typeStockage = TypeStockage.PlayerInventory;

    public static Inventory instance;
    private void Awake() {
        instance = this;
    }

    public void Open()
    {
        SetActiveUI();
        StockageUI.instance.Open();
    }

    public void SetActiveUI()
    {
        StockageUI.instance.Set(stockage, typeStockage);
    }
}