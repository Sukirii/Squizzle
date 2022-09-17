using UnityEngine;

public class TouchManager : MonoBehaviour
{
    public Transform joystick_inner;

    public GameObject jumpButton;

    InputManager inputManager;
    GameManager gameManager;
    Player player;

    [HideInInspector]
    public Vector2 touchPos;

    Vector2 pointA, pointB;

    bool touch_started, isPressingJump;

    private void Awake()
    {
        inputManager = FindObjectOfType<InputManager>();
        gameManager = FindObjectOfType<GameManager>();
        player = FindObjectOfType<Player>();
    }

    private void Start()
    {
        jumpButton.SetActive(PlatformManager.isMobile);
        gameObject.SetActive(PlatformManager.isMobile);
    }

    private void Update()
    {
        if (isPressingJump)
            player.Jump();
    }

    void StartJoystick()
    {
        touch_started = true;

        transform.position = touchPos;
        pointA = transform.position;
    }

    public void StopJoystick()
    {
        if (touchPos.x > Screen.width / 2f || gameManager.isDead)
            return;

        touch_started = false;

        pointB = joystick_inner.position;
        joystick_inner.localPosition = Vector3.zero;

        inputManager.movement = 0f;
    }

    public void HandleTouches(Vector2 _pos)
    {
        touchPos = _pos;

        if (touchPos.x > Screen.width / 2f || gameManager.isDead)
            return;

        if (!touch_started)
            StartJoystick();

        pointB = touchPos;
        MoveJoystickInner();
    }

    void MoveJoystickInner()
    {
        Vector2 offset = pointB - pointA;
        Vector2 direction = Vector2.ClampMagnitude(offset, 100f);

        joystick_inner.position = pointA + direction;

        if (Mathf.Abs(direction.x) / 100f > 0.2f)
            inputManager.movement = direction.x / 100f;
        else
            inputManager.movement = 0f;
    }

    public void StartJump()
    {
        if (gameManager.isDead)
            return;

        isPressingJump = true;
        player.StartJump();
    }
    public void StopJump()
    {
        if (gameManager.isDead)
            return;

        isPressingJump = false;
        player.StopJump();
    }
}