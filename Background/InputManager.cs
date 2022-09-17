using UnityEngine;

public class InputManager: MonoBehaviour
{
    PlayerInput playerInput;

    TouchManager touchManager;

    Player player;

    [HideInInspector]
    public float movement;
    [HideInInspector]
    public bool pressingJump;

    private void Awake()
    {
        playerInput = new PlayerInput();

        player = FindObjectOfType<Player>();
        touchManager = FindObjectOfType<TouchManager>();

        playerInput.Gameplay.Movement.performed += ctx => SetMovement(ctx.ReadValue<float>());
        playerInput.Gameplay.Movement.canceled += ctx => SetMovement(0f);

        playerInput.Gameplay.Jump.started += ctx => StartJump();
        playerInput.Gameplay.Jump.canceled += ctx => StopJump();

        playerInput.Gameplay.TouchPos.performed += ctx => SetTouchPos(ctx.ReadValue<Vector2>());
        playerInput.Gameplay.Touch.canceled += ctx => StopTouch();
    }

    private void Update()
    {
        if (pressingJump)
            Jump();
    }

    void SetTouchPos(Vector2 _touchPos)
    {
        touchManager.HandleTouches(_touchPos);
    }
    void StopTouch()
    {
        touchManager.StopJoystick();
    }

    void SetMovement(float _movement)
    {
        movement = _movement;
    }

    public void StartJump()
    {
        pressingJump = true;
        player.StartJump();
    }

    void Jump()
    {
        player.Jump();
    }

    public void StopJump()
    {
        if (player != null)
            player.StopJump();

        pressingJump = false;
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }
    private void OnDisable()
    {
        playerInput.Disable();
    }
}