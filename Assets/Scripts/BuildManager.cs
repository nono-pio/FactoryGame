using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class BuildManager : MonoBehaviour
{
    [SerializeField] private GameObject buildBar;

    [SerializeField] private Tilemap mapForChecker;
    [SerializeField] private Tilemap mapForBuild;
    [SerializeField] private Tilemap mapForCollision;

    [SerializeField] private TileBase tileCollision;
    private TileBase[] selectedTiles;
    private int indexTile = 0;

    private Vector3Int curCellPointed;

    private bool isBuildModeActive;

    [HideInInspector] public static BuildManager instance;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        buildBar.SetActive(false);
        addSelect();
        GetSelectedTile();
    }

    private void FixedUpdate()
    {
        if (isBuildModeActive)
        {
            Vector3 mousePos = InputManager.instance.GetMouseWorldPosition();
            Vector3Int cell = mapForBuild.WorldToCell(mousePos);
            if (curCellPointed != cell)
            {
                mapForChecker.SetTile(curCellPointed, null);

                if (mapForBuild.GetTile(cell) == null && selectedTiles != null)
                {
                    mapForChecker.SetTile(cell, selectedTiles[indexTile]);
                }
                
                curCellPointed = cell;
            }
            if(InputManager.instance.isClicking)
                build();
        }
    }

    #region Build Actions

        public void build()
        {
            if (isBuildModeActive && mapForBuild.GetTile(curCellPointed) == null && selectedTiles != null)
            {
                mapForBuild.SetTile(curCellPointed, selectedTiles[indexTile]);
                mapForCollision.SetTile(curCellPointed, tileCollision);
                mapForChecker.SetTile(curCellPointed, null);
            }
        }

        public void rotate()
        {
            if (selectedTiles != null)
            {
                indexTile = (indexTile + 1) % selectedTiles.Length;
                mapForChecker.SetTile(curCellPointed, selectedTiles[indexTile]);
            }
        }

    #endregion

    

    #region active-disactive

    public void ActiveBuildMode()
    {
        if(buildBar.activeInHierarchy)
            DisactiveBuildMode();
        else
        {
            buildBar.SetActive(true);
            isBuildModeActive = true;
        }
    }

    public void DisactiveBuildMode()
    {
        buildBar.SetActive(false);
        isBuildModeActive = false;
        mapForChecker.ClearAllTiles();
    }
        
    #endregion
    
    #region BuildBar

    [Header("Build Bar")]
    [SerializeField] private GameObject[] barSlots;
    [SerializeField] private Vector2 selectedScale;
    private int indexBar = 0;

    private void GetSelectedTile()
    {
        InventoryItem itemInSlot = barSlots[indexBar].GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null || itemInSlot.item.tile == null)
                selectedTiles = null;
            else
            {
                indexTile = 0;
                selectedTiles = itemInSlot.item.tile;
            }
    }

    public void addIndexBar()
    {
        if (isBuildModeActive)
        {
            removeSelect();
            indexBar = (indexBar + 1) % barSlots.Length;
            addSelect();
            GetSelectedTile();
        }
    }
    
    public void removeIndexBar()
    {
        if (isBuildModeActive)
        {
            removeSelect();
            indexBar = (indexBar - 1 + barSlots.Length) % barSlots.Length;
            addSelect();
            GetSelectedTile();
        }
    }

    private void removeSelect()
    {
        if (barSlots[indexBar].transform.childCount > 0)
            barSlots[indexBar].transform.GetChild(0).localScale = Vector2.one;
    }

    private void addSelect()
    {
        if (barSlots[indexBar].transform.childCount > 0)
            barSlots[indexBar].transform.GetChild(0).localScale = selectedScale;
    }

    #endregion
}