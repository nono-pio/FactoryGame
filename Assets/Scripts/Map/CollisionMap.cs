using UnityEngine;
using UnityEngine.Tilemaps;

public class CollisionMap : MonoBehaviour
{
    private TilemapRenderer tilemapRenderer;

    [SerializeField]
    private bool showCollision;

    void Start()
    {
        tilemapRenderer = GetComponent<TilemapRenderer>();
        tilemapRenderer.enabled = showCollision;
    }
}
