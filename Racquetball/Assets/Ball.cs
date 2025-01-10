using System.Collections;
using UnityEngine;
using TMPro;

public class Ball : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Collider playerCollider;
    [SerializeField] private float bounceForce = 5f;
    [SerializeField] private float forceIncrease = 1f;
    [SerializeField] private float wallBounceForce = 10f;
    [SerializeField] private float minVerticalBounce = 2f;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private TMP_Text livesText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text countdownText;
    [SerializeField] private TMP_Text outOfBoundsMessage;
    [SerializeField] private float launchForce = 10f; // Configurable launch force

    private int playerLives = 3;
    private int playerScore = 0;
    private bool gameOver = false;
    private bool isCountingDown = false; // Prevent multiple countdown instances
    private bool isShowingPrompt = false;
    private Vector3 initialPosition;

    private void Start()
    {
        rb = rb ?? GetComponent<Rigidbody>();
        initialPosition = transform.position;

        if (gameOverUI != null) gameOverUI.SetActive(false);

        rb.isKinematic = true; // Prevent the ball from moving
        UpdateUI();
        outOfBoundsMessage.gameObject.SetActive(false);
        StartCoroutine(LaunchBallWithCountdown());
    }
    private void Update()
    {
        // Check if the ball is out of bounds
        if (transform.position.magnitude > 50f && !gameOver)
        {
            Debug.Log("Ball out of bounds. Resetting...");
            ShowOutOfBoundsMessage(); // Display the UI message
            ResetBall();
        }
    }



    private void OnCollisionEnter(Collision collision)
    {
        if (gameOver) return;

        if (collision.collider == playerCollider)
        {
            float speed = rb.velocity.magnitude;
            Vector3 direction = Vector3.Reflect(rb.velocity.normalized, collision.contacts[0].normal);
            direction.y = Mathf.Max(direction.y, minVerticalBounce);
            rb.velocity = direction * (speed + forceIncrease);

            playerScore++;
            UpdateUI();
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            float speed = rb.velocity.magnitude;
            Vector3 direction = Vector3.Reflect(rb.velocity.normalized, collision.contacts[0].normal);
            direction.y = Mathf.Max(direction.y, minVerticalBounce);
            rb.velocity = direction * wallBounceForce;
        }
        else if (collision.gameObject.CompareTag("Floor"))
        {
            HandleLifeLoss();
        }
    }

    private void HandleLifeLoss()
    {
        if (isCountingDown) return; // Prevent multiple countdowns

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
    }
    private void ResetBall()
    {
        if (isCountingDown) return; // Prevent multiple countdowns

        isCountingDown = true; // Start countdown state
        rb.velocity = Vector3.zero;
        transform.position = initialPosition;
        rb.isKinematic = true; // Disable physics while resetting
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
        isCountingDown = false; // Countdown complete
    }





    private void LaunchBall()
    {
        rb.isKinematic = false; // Enable physics for the ball
        Vector3 initialDirection = Vector3.forward; // Launch towards the wall
        rb.AddForce(initialDirection * launchForce, ForceMode.Impulse);
    }

    private void UpdateUI()
    {
        if (livesText != null) livesText.text = "Lives: " + playerLives;
        if (scoreText != null) scoreText.text = "Score: " + playerScore;
    }

    private void ShowOutOfBoundsMessage()
    {

            outOfBoundsMessage.gameObject.SetActive(true); // Show the message
            outOfBoundsMessage.text = "Out of Bounds... Restarting..."; // Update the message
            StartCoroutine(HideOutOfBoundsMessage()); // Start a coroutine to hide the message after a delay
    }

    private IEnumerator HideOutOfBoundsMessage()
    {
        yield return new WaitForSeconds(2f); // Wait for 2 seconds

            outOfBoundsMessage.gameObject.SetActive(false); // Hide the message

    }


}
