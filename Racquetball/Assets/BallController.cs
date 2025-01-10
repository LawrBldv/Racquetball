using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] private float initialForce = 5f; // Initial force applied to the ball
    [SerializeField] private float bounceForceIncrease = 1.1f; // Force multiplier when bouncing off the player
    [SerializeField] private PlayerController player; // Reference to the player
    private Rigidbody rb;

    private bool hasLaunched = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        LaunchBall(); // Launch the ball initially
    }

    private void LaunchBall()
    {
        if (!hasLaunched)
        {
            Vector3 initialDirection = Vector3.forward; // Launch toward the wall
            rb.AddForce(initialDirection * initialForce, ForceMode.Impulse);
            hasLaunched = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == player.gameObject)
        {
            Vector3 newVelocity = rb.velocity * bounceForceIncrease; // Increase the bounce force
            rb.velocity = newVelocity; // Apply the new velocity
        }

        if (collision.gameObject.CompareTag("Floor"))
        {
            Debug.Log("The ball hit the floor.");
        }
    }
}
