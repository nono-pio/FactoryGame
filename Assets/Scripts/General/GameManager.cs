using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake() {instance = this;}

    [Header("Item")]
    [SerializeField] public Item[] items;

    [Header("Crafts")]
    [SerializeField] public ItemCraft[] crafts;
    public int defaultCraft;
}