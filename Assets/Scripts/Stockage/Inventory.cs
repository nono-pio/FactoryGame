using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private int slotSize = 24;
    [HideInInspector] public Stockage stockage;
    [HideInInspector] public TypeStockage typeStockage = TypeStockage.PlayerInventory;

    public static Inventory instance;
    private void Awake()
    {
        stockage = new Stockage(slotSize);
        instance = this;
    }

    public void Open()
    {
        StockageUI.instance.Open();
        SetActiveUI();
    }

    public void SetActiveUI()
    {
        StockageUI.instance.Set(stockage, typeStockage);
    }
}