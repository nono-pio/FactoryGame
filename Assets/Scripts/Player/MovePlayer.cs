using UnityEngine;
using UnityEngine.InputSystem;

public class MovePlayer : MonoBehaviour
{
    //componant
    private Rigidbody2D rbody;
    private RendererPlayer rendererPlayer;

    // input sys
    [SerializeField] private bool useInputSystem;
    private AllInput.PlayerActions playerControls;
    private InputAction move;

    [HideInInspector] public bool canMove = true;

    [Header("Speed")]
    [SerializeField] private float speed = 10;
    [SerializeField] private float runSpeed = 15;
    private bool isRunning = false;

    private Vector2 curInput;
    private Vector2 smoothInputVelocity;
    [SerializeField] private float smoothInputSpeed;

    private void OnEnable()
    {
        move = playerControls.Move;
        playerControls.Enable();
    }
    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        rendererPlayer = GetComponentInChildren<RendererPlayer>();

        playerControls = InputManager.instance.getPlayerInput();
        playerControls.Run.started += ctx => isRunning = true;
        playerControls.Run.canceled += ctx => isRunning = false;
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            Vector2 input = move.ReadValue<Vector2>();
            curInput = Vector2.SmoothDamp(curInput, input, ref smoothInputVelocity, smoothInputSpeed);

            Vector2 movement;
            if (isRunning)
                movement = curInput * runSpeed;
            else
                movement = curInput * speed;

            // calcul the new pos and set to the rbody
            Vector2 newPos = rbody.position + movement * Time.fixedDeltaTime;
            rbody.MovePosition(newPos);

            rendererPlayer.SetSprite(curInput, isRunning);
        }
    }
}