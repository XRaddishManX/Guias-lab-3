using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    private PlayerInput playerInput;
    InputAction moveAction;
    InputAction cameraMoveAction;
    public float moveSpeed = 5f;
    public LayerMask groundMask;
    public Transform cameraTransform;
    public float cameraSpeed = 2f;
    public Transform cameraCapsule;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions.FindAction("Move");
        cameraMoveAction = playerInput.actions.FindAction("Look");
        
        moveAction.performed += contexto => moveInput =
        contexto.ReadValue<Vector2>();
        moveAction.canceled += contexto => moveInput = Vector2.zero;
        cameraMoveAction.performed += contexto =>
        MoveCamera(contexto.ReadValue<Vector2>());
    }
    void FixedUpdate()
    {
        // Movimiento horizontal
        Vector3 moveDirection = new Vector3(moveInput.x, 0f,
       moveInput.y).normalized;
        Vector3 moveVelocity = moveDirection * moveSpeed;
        rb.velocity = new Vector3(moveVelocity.x, rb.velocity.y,
       moveVelocity.z);
    }
    void MoveCamera(Vector2 direction)
    {
        // Movimiento de la cápsula de la cámara
        cameraCapsule.Rotate(Vector3.up, direction.x * cameraSpeed *
       Time.deltaTime, Space.World);
        // Rotación de la cámara
        cameraTransform.Rotate(Vector3.right, -direction.y * cameraSpeed *
       Time.deltaTime, Space.Self);
    }
    void OnEnable()
    {
        moveAction.Enable();
        cameraMoveAction.Enable();
    }
    void OnDisable()
    {
        moveAction.Disable();
        cameraMoveAction.Disable();
    }
}