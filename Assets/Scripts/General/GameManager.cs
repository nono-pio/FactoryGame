using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake() {instance = this;}

    public Item[] items;

    public ItemCraft[] crafts;
    public int defaultCraft;
}