using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildManager : MonoBehaviour
{
    [SerializeField] private GameObject buildBar;

    [SerializeField] private Tilemap mapForChecker;
    [SerializeField] private Tilemap mapForBuild;
    [SerializeField] private Tilemap mapForCollision;

    [SerializeField] private TileBase tileCollision;
    [SerializeField] private TileBase tileDelete;

    private Vector3Int curCellPointed;
    private TileBase[] selectedTiles;
    private int indexTile = 0;

    private bool isBuildModeActive = false;
    private bool isDeleting = false;

    [HideInInspector] public static BuildManager instance;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        InitateBuildInput(InputManager.instance.getBuildInput());
        buildBar.SetActive(false);
        addSelect();
        GetSelectedTile();
    }

    private void FixedUpdate()
    {
        if (isBuildModeActive)
        {
            Vector3Int newPosCell = mapForBuild.WorldToCell(InputManager.instance.GetMouseWorldPosition());
            if (isDeleting) //mode delete
            {
                if (curCellPointed != newPosCell)
                {
                    mapForChecker.SetTile(curCellPointed, null);

                    if (mapForBuild.GetTile(newPosCell) != null)
                    {
                        mapForChecker.SetTile(newPosCell, tileDelete);
                    }

                    curCellPointed = newPosCell;
                }
                if (InputManager.instance.isClicking)
                {
                    delete();
                }

            } else //mode build
            {
                if (curCellPointed != newPosCell)
                {
                    mapForChecker.SetTile(curCellPointed, null);

                    if (mapForBuild.GetTile(newPosCell) == null && selectedTiles != null)
                    {
                        mapForChecker.SetTile(newPosCell, selectedTiles[indexTile]);
                    }
                    
                    curCellPointed = newPosCell;
                }
                if (InputManager.instance.isClicking)
                {
                    build();
                }
            }
        }
    }

    #region Build Actions

        /*
        private void changePosCell(Vector2 input)
        {
            Vector3Int newCellPos = curCellPointed;
            if (input == null || !isBuildModeActive) return;

            if (input.x > 0) newCellPos.x++; //update hor position
            else newCellPos.x--;

            if (input.y > 0) newCellPos.y++; // update ver position
            else newCellPos.y--;

            moveCell(newCellPos);
        }


        private void moveCell(Vector3Int newPosCell)
        {
            if (isDeleting) //mode delete
            {

            } else //mode build
            {
                if (curCellPointed != newPosCell)
                {
                    mapForChecker.SetTile(curCellPointed, null);

                    if (mapForBuild.GetTile(newPosCell) == null && selectedTiles != null)
                    {
                        mapForChecker.SetTile(newPosCell, selectedTiles[indexTile]);
                    }
                    
                    curCellPointed = newPosCell;
                }
            }
        }*/

        public void build()
        {
            if (isBuildModeActive && mapForBuild.GetTile(curCellPointed) == null && selectedTiles != null)
            {
                mapForBuild.SetTile(curCellPointed, selectedTiles[indexTile]);
                mapForCollision.SetTile(curCellPointed, tileCollision);
                mapForChecker.SetTile(curCellPointed, null);
            }
        }

        public void delete()
        {
            if (isBuildModeActive && mapForBuild.GetTile(curCellPointed) != null)
            {
                mapForBuild.SetTile(curCellPointed, null);
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

        private void SetModeDelete()
        {
            mapForChecker.SetTile(curCellPointed, null);
            if(isDeleting)
            {
                isDeleting = false;
            } else 
            {
                isDeleting = true;
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

    public void RefreshBuildBar()
    {
        GetSelectedTile();
        addSelect();
    }

    #endregion

    #region BuildInput

    private void InitateBuildInput(AllInput.BuildModeActions buildInput)
    {
        buildInput.add.performed += ctx => BuildManager.instance.addIndexBar();
        buildInput.remove.performed += ctx => BuildManager.instance.removeIndexBar();
        buildInput.rotate.performed += ctx => BuildManager.instance.rotate();
        buildInput.delete.performed += ctx => SetModeDelete();
        //buildInput.changeCell.performed += ctx => changePosCell(ctx.ReadValue<Vector2>());
    }
        
    #endregion
}