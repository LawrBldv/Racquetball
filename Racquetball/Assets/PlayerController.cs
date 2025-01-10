using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 10f; // Speed of the player's movement
    [SerializeField] private Vector2 boundaries = new Vector2(8f, 8f); // X and Z boundaries for player movement

    private void Update()
    {
        // Get input for both X (horizontal) and Z (vertical) directions
        float inputX = Input.GetAxis("Horizontal"); // A/D or Left/Right arrow keys
        float inputZ = Input.GetAxis("Vertical");   // W/S or Up/Down arrow keys

        // Calculate movement based on input
        Vector3 move = new Vector3(inputX * speed * Time.deltaTime, 0, inputZ * speed * Time.deltaTime);

        // Apply movement
        transform.position += move;

        // Clamp player position within boundaries
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, -boundaries.x, boundaries.x),
            transform.position.y,
            Mathf.Clamp(transform.position.z, -boundaries.y, boundaries.y)
        );
    }
}
