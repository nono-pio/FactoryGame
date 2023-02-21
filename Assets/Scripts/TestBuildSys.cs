using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class TestBuildSys : MonoBehaviour
{
    [SerializeField] private TileBase tile;
    [SerializeField] private Tilemap map;

    [HideInInspector] public static TestBuildSys instance;
    private void Awake() {instance = this;}

    public void build(Vector3 position)
    {
        Vector3Int cell = map.WorldToCell(position);
        map.SetTile(cell, tile);
    }

    public void buildSquare(Vector3 posA, int sideLenght)
    {
        Vector3Int cellA = map.WorldToCell(posA);
        Vector3Int[] cells = new Vector3Int[sideLenght*sideLenght];

        for (int x = 0; x < sideLenght; x++)
            for (int y = 0; y < sideLenght; y++)
                cells[x*sideLenght + y] = new Vector3Int(cellA.x + x, cellA.y + y);
        
        TileBase[] tiles = new TileBase[sideLenght*sideLenght];
        Array.Fill<TileBase>(tiles, tile);
        
        map.SetTiles(cells, tiles);
    }
}
