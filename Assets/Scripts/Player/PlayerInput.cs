using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour, GameInput.IMovementActions
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private CameraMovement cameraMovement;
    [SerializeField] private Shooting playerShooting;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float deadzoneVelocityRadius = 4;
    [SerializeField] private float maxVelocityRadius = 8;

    GameInput input;

    void Start()
    {
        input = new();
        input.Movement.AddCallbacks(this);
        input.Movement.Enable();
    }

    public enum InputMode
    {
        MouseNKeyboard,
        Gamepad
    }
    public InputMode CurrentInputMode { get; private set; }
    private Vector2 DirectMoveInput;
    private Vector2 DirectAimInput;
    private Vector2 PointerPosition;
    private Vector2 PointerDirection
    {
        get
        {
            var ray = playerCamera.ScreenPointToRay(PointerPosition);
            var t = (ray.origin.y - 1) / ray.direction.y;
            var xzIntersection = ray.origin + ray.direction * -t;
            var direction = xzIntersection - cameraMovement.transform.position;
            return direction._xz();
        }
    }

    private Vector2 PointerMoveDirection
    {
        get
        {
            var direction = PointerDirection;
            var mag = direction.magnitude;
            direction = direction.normalized;
            mag = Mathf.Clamp(mag - deadzoneVelocityRadius, 0, maxVelocityRadius - deadzoneVelocityRadius);
            mag /= maxVelocityRadius - deadzoneVelocityRadius;
            return direction * mag;
        }
    }

    void FixedUpdate()
    {
        playerMovement.InputDirection = DirectMoveInput;
        cameraMovement.LookAheadDirection = playerMovement.RB.velocity._xz() / playerMovement.Velocity; //TODO: try setting this to input instead of velocity?

        var shootingDirection = CurrentInputMode switch
        {
            InputMode.MouseNKeyboard => PointerDirection,
            InputMode.Gamepad => DirectAimInput,
            _ => Vector2.zero
        };
        if (shootingDirection.magnitude > 0) playerShooting.FireDirection = shootingDirection.normalized;
    }

    public void OnDirectMovement(InputAction.CallbackContext context)
    {
        DirectMoveInput = context.ReadValue<Vector2>();
    }

    public void OnPointerAim(InputAction.CallbackContext context)
    {
        CurrentInputMode = InputMode.MouseNKeyboard;
        Cursor.visible = true;
        PointerPosition = context.ReadValue<Vector2>();
    }

    public void OnDirectAim(InputAction.CallbackContext context)
    {
        CurrentInputMode = InputMode.Gamepad;
        Cursor.visible = false;
        DirectAimInput = context.ReadValue<Vector2>();
    }
}
