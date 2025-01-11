using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float sideSpeed = 200f; // Speed of the player's side-to-side movement
    [SerializeField] private float forwardSpeed = 200f; // Speed of the player's forward/backward movement
    [SerializeField] private float jumpForce = 5f; // Force applied when the player jumps
    [SerializeField] private float rotationSpeed = 100f; // Speed of rotation using mouse scroll

    [SerializeField] private float minX = -10f; // Minimum X bound
    [SerializeField] private float maxX = 10f; // Maximum X bound
    [SerializeField] private float minZ = -10f; // Minimum Z bound
    [SerializeField] private float maxZ = 10f; // Maximum Z bound

    [SerializeField] private float sprintMultiplier = 1.5f; // Multiplier for sprint speed

    private Rigidbody rb; // Reference to the Rigidbody component
    private bool isGrounded; // To check if the player is on the ground

    private void Start()
    {
        // Get the Rigidbody component attached to the player
        rb = GetComponent<Rigidbody>();

        // Set Rigidbody collision detection to continuous for high-speed objects
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    private void Update()
    {
        // Get input for the X (horizontal) and Z (vertical) directions
        float inputX = Input.GetAxis("Horizontal"); // A/D or Left/Right arrow keys
        float inputZ = Input.GetAxis("Vertical");   // W/S or Up/Down arrow keys

        // Calculate movement direction based on horizontal and vertical input with respective speeds
        Vector3 direction = CalculateMovementDirection(inputX, inputZ);

        // If there's any movement input, move the player
        if (direction != Vector3.zero)
        {
            // Move the player in the direction (side to side and forward/backward)
            MoveCharacter(direction);
        }

        // Apply jump logic: check if the player presses space and if the player is grounded
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // Apply upward force to make the player jump
            isGrounded = false; // Reset the grounded status after jumping
        }

        // Rotate the game object based on mouse scroll
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        RotateCharacter(scrollInput);
    }

    private Vector3 CalculateMovementDirection(float inputX, float inputZ)
    {
        // Determine if the shift key is pressed
        bool isSprinting = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        // Apply the sprint multiplier if sprinting
        float speedMultiplier = isSprinting ? sprintMultiplier : 1f;

        // Combine horizontal (X) and vertical (Z) movement with respective speeds and multiplier
        float movementX = inputX * sideSpeed * speedMultiplier;
        float movementZ = inputZ * forwardSpeed * speedMultiplier;
        return new Vector3(movementX, 0, movementZ);
    }

    private void MoveCharacter(Vector3 direction)
    {
        // Move the player based on input (side-to-side and forward/backward)
        Vector3 movement = direction * Time.deltaTime; // Calculate movement
        rb.MovePosition(rb.position + movement);

        // Apply the bounds to the player's position
        float clampedX = Mathf.Clamp(rb.position.x, minX, maxX);
        float clampedZ = Mathf.Clamp(rb.position.z, minZ, maxZ);

        // Update the player's position with the clamped values
        rb.position = new Vector3(clampedX, rb.position.y, clampedZ);
    }

    private void RotateCharacter(float scrollInput)
    {
        // Rotate the game object on the X-axis based on the scroll input
        float rotationAmount = scrollInput * rotationSpeed * Time.deltaTime;
        transform.Rotate(rotationAmount, 0, 0);
    }

    // Check if the player is grounded (can be adjusted based on your character's colliders)
    private void OnCollisionStay(Collision collision)
    {
        // Check if the player is colliding with the floor
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = true; // Set the player as grounded when touching the floor
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Check if the player is no longer touching the floor
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = false; // Reset grounded status when leaving the floor
        }
    }
}
