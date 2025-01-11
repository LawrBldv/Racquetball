using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Ball : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Collider playerCollider;
    [SerializeField] private float minVerticalBounce = 2f;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private TMP_Text livesText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text countdownText;
    [SerializeField] private TMP_Text outOfBoundsMessage;
    [SerializeField] private float launchForce = 30f;

    [SerializeField] private float minZForce = 5f; // Minimum value for the random Z force
    [SerializeField] private float maxZForce = 15f; // Maximum value for the random Z force

    private int playerLives = 3;
    public static int playerScore = 0;
    private bool gameOver = false;
    private bool isCountingDown = false;
    private Vector3 initialPosition;

   [SerializeField] private float zVelocityIncreaseAmount = 1f; // Amount to add to the Z velocity on each collision
    private float zVelocityIncrease = 0f; // Tracks the current increase in Z velocity

    [SerializeField] private SFXManager sfx;

    [SerializeField] private SingleAudioPlayer sfx321;

    private void Start()
    {
        rb = rb ?? GetComponent<Rigidbody>();
        initialPosition = transform.position;

        if (gameOverUI != null) gameOverUI.SetActive(false);

        rb.isKinematic = true;
        UpdateUI();
        outOfBoundsMessage.gameObject.SetActive(false);
        StartCoroutine(LaunchBallWithCountdown());
    }

    private void Update()
    {
        if (transform.position.magnitude > 200f && !gameOver)
        {
            Debug.Log("Ball out of bounds. Resetting...");
            ShowOutOfBoundsMessage();
            ResetBall();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (gameOver) return;

        if (collision.collider == playerCollider)
        {
            HandlePlayerCollision(collision); // Apply player collision logic here
        }
        else if (collision.gameObject.CompareTag("Floor"))
        {
            HandleLifeLoss();
        }
    }

    private void HandlePlayerCollision(Collision collision)
    {
        sfx.PlaySFX(0);
        // Apply a constant Z force within the specified range
        float constantZForce = Random.Range(minZForce, maxZForce);

        // Apply a flat increase to Z velocity after each successful collision
        float newZVelocity = rb.velocity.z + (rb.velocity.z >= 0 ? constantZForce : -constantZForce) + zVelocityIncrease;

        // Randomize the X direction with bias
        float bias = transform.position.x < 0 ? 0.6f : -0.6f; // Bias towards positive or negative X
        float randomX = Random.Range(-15f + 15f * bias, 15f + 15f * bias);

        // Ensure the ball bounces upward (min vertical speed)
        float newYVelocity = rb.velocity.y < minVerticalBounce ? minVerticalBounce : rb.velocity.y;

        // Set the new velocity with the biased X, constant Z force, and adjusted Y velocity
        rb.velocity = new Vector3(randomX, newYVelocity, newZVelocity);

        // Increase Z velocity by the flat amount
        zVelocityIncrease += zVelocityIncreaseAmount;

        // Increase score on player collision
        playerScore++;
        UpdateUI();
    }

    private void HandleLifeLoss()
    {
        if (isCountingDown) return;

        playerLives--;
        UpdateUI();

        if (playerLives <= 0)
        {
            TriggerGameOver();
        }
        else
        {
            ResetBall();
        }
    }

    private void TriggerGameOver()
    {
        Debug.Log("Game Over! No more lives.");
        gameOver = true;

        if (gameOverUI != null) gameOverUI.SetActive(true);
        rb.velocity = Vector3.zero;

         SceneManager.LoadScene("EndMenu");
    }

   private void ResetBall()
    {
        sfx321.PlaySound();
        
        if (isCountingDown) return;

        isCountingDown = true;
        rb.velocity = Vector3.zero;
        transform.position = initialPosition;
        rb.isKinematic = true;

        // Reset the Z velocity increase factor when resetting the ball
        zVelocityIncrease = 0f;

        StartCoroutine(LaunchBallWithCountdown());
    }

    private IEnumerator LaunchBallWithCountdown()
    {
        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(true);
        }

        countdownText.text = "3";
        yield return new WaitForSeconds(1f);
        countdownText.text = "2";
        yield return new WaitForSeconds(1f);
        countdownText.text = "1";
        yield return new WaitForSeconds(1f);

        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(false);
        }

        LaunchBall();
        isCountingDown = false;
    }

    private void LaunchBall()
    {
        rb.isKinematic = false;
        // Set a fixed direction (forward along the Z-axis)
        Vector3 initialDirection = new Vector3(0f, 0f, 1f).normalized;
        rb.AddForce(initialDirection * launchForce, ForceMode.Impulse); // Launch forward
    }

    public void UpdateUI()
    {
        if (livesText != null) livesText.text = "Lives: " + playerLives;
        if (scoreText != null) scoreText.text = "Score: " + playerScore;
    }

    private void ShowOutOfBoundsMessage()
    {
        outOfBoundsMessage.gameObject.SetActive(true);
        outOfBoundsMessage.text = "Out of Bounds... Restarting...";
        StartCoroutine(HideOutOfBoundsMessage());
    }

    private IEnumerator HideOutOfBoundsMessage()
    {
        yield return new WaitForSeconds(2f);
        outOfBoundsMessage.gameObject.SetActive(false);
    }
}
