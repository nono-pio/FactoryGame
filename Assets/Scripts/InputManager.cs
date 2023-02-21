using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] private AllInput input;
    [SerializeField] private Camera mainCamera;

    [HideInInspector] public static InputManager instance;

    public bool isClicking = false;

    private void Awake()
    {
        instance = this;
        input = new AllInput();
        SetUIInput();
        input.Mouse.StartClick.performed += ctx => isClicking = true;
        input.Mouse.EndClick.performed += ctx => isClicking = false;
        /*input.Mouse.Click.performed += ctx => 
        {
            Debug.Log("Click");
            Vector3 posA = GetMouseWorldPosition();
            TestBuildSys.instance.buildSquare(posA, 20);
        };*/
    }

    #region UIKey
    
    private void SetUIInput()
    {
        input.UI.Inventory.performed += ctx => InventoryManager.instance.openInventory();
        input.UI.Crafting.performed += ctx => CraftingManager.instance.openCraft();
        input.UI.PauseMenu.performed += ctx => PauseMenu.instance.openPauseMenu();
        input.UI.BuildMode.performed += ctx => BuildManager.instance.ActiveBuildMode();
    }

    #endregion

    #region Enable - Disable

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    #endregion

    #region GetMapActions

    public AllInput.PlayerActions getPlayerInput() => input.Player;

    public AllInput.BuildModeActions getBuildInput() => input.BuildMode;

    public AllInput.DebugActions GetDebugInput() => input.Debug;

    #endregion

    #region Other
    
    public Vector3 GetMouseWorldPosition()
    {
        Vector3 pos = mainCamera.ScreenToWorldPoint(input.Mouse.Position.ReadValue<Vector2>());
        pos.z = 0;
        return pos;
    }

    #endregion

}
