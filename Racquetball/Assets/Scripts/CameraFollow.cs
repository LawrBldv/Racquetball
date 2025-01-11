using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;          // The player's transform to follow
    [SerializeField] private float distance = 10f;      // The distance from the player (behind)
    [SerializeField] private float height = 5f;         // The height offset (above the player)
    [SerializeField] private float smoothSpeed = 0.125f; // Smooth follow speed
    [SerializeField] private float rotationSpeed = 3f;  // Speed of mouse-based rotation

    private float currentRotationX = 0f; // Current vertical rotation of the camera
    private float currentRotationY = 0f; // Current horizontal rotation of the camera

    private void LateUpdate()
    {
        // Get mouse input for camera rotation
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

        // Update the camera's vertical and horizontal rotation
        currentRotationX -= mouseY;
        currentRotationX = Mathf.Clamp(currentRotationX, -40f, 80f); // Limit vertical angle (clamping to avoid full inversion)
        currentRotationY += mouseX;

        // Calculate the rotation based on mouse input
        Quaternion rotation = Quaternion.Euler(currentRotationX, currentRotationY, 0);

        // Calculate the desired position behind the player (with height offset)
        Vector3 desiredPosition = player.position - rotation * Vector3.forward * distance + Vector3.up * height;

        // Smoothly transition to the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Update the camera position
        transform.position = smoothedPosition;

        // Apply the rotation to the camera
        transform.rotation = rotation;
    }
}
