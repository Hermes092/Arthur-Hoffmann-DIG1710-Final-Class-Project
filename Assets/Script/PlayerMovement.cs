using UnityEngine; // Import Unity engine functionality

[RequireComponent(typeof(CharacterController))] // Ensures CharacterController exists
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f; // Walking speed
    public float sprintSpeed = 9f; // Sprinting speed when holding Shift
    public float jumpHeight = 2f; // Jump height
    public float gravity = -9.81f; // Gravity force

    [Header("Rotation Settings")]
    public float rotationSpeed = 100f; // How fast the player rotates when pressing Q/E

    [Header("Ground Check Settings")]
    public Transform groundCheck; // Object below player feet
    public float groundDistance = 0.4f; // Sphere radius for ground check
    public LayerMask groundMask; // Which layer counts as ground

    private CharacterController controller; // Reference to CharacterController
    private Vector3 velocity; // Player’s current velocity
    private bool isGrounded; // True if touching ground

    void Start()
    {
        controller = GetComponent<CharacterController>(); // Get CharacterController
    }

    void Update()
    {
        // Ground check using a small invisible sphere under the player
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // If grounded and moving down, reset Y velocity
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Keeps player grounded
        }

        // --- ROTATION ---
        float rotationInput = 0f; // Start with no rotation
        if (Input.GetKey(KeyCode.Q)) rotationInput = -1f; // Rotate left
        if (Input.GetKey(KeyCode.E)) rotationInput = 1f;  // Rotate right

        // Apply rotation around Y-axis (up axis)
        transform.Rotate(Vector3.up * rotationInput * rotationSpeed * Time.deltaTime);

        // --- MOVEMENT ---
        float x = Input.GetAxis("Horizontal"); // A/D or Left/Right
        float z = Input.GetAxis("Vertical");   // W/S or Up/Down

        // Calculate movement direction relative to facing direction
        Vector3 move = transform.right * x + transform.forward * z;

        // Sprint when Left Shift is held
        bool isSprinting = Input.GetKey(KeyCode.LeftShift);
        float currentSpeed = isSprinting ? sprintSpeed : walkSpeed;

        // Move player horizontally
        controller.Move(move * currentSpeed * Time.deltaTime);

        // --- JUMP ---
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            // Add upward velocity for jump
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Apply gravity continuously
        velocity.y += gravity * Time.deltaTime;

        // Apply vertical motion (gravity and jump)
        controller.Move(velocity * Time.deltaTime);
    }
}
