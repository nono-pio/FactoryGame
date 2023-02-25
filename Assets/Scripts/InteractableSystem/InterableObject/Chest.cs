using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    public Message messageInteract => null;

    private Stockage stockage = new Stockage(24);
    private TypeStockage typeStockage = TypeStockage.Chest;

    public void Open()
    {
        StockageUI.instance.Open();
        StockageUI.instance.Set(stockage, typeStockage);
    }
}
