using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InstructionsPanel : MonoBehaviour
{
    [SerializeField] private GameObject panel; // Reference to the panel GameObject
    [SerializeField] private Button continueButton; // Reference to the button that resumes the game
    [SerializeField] private Button mainMenuButton; // Reference to the button that loads the main menu

    [SerializeField] private SingleAudioPlayer sfx321;

    private bool isPaused = true; // Track if the game is paused
    private PlayerController playerMovement;
    private CameraFollow cameraFollow;

    private bool hasPlayedSound = false; // Static flag to ensure the sound plays only once per session

    private void Start()
    {
        playerMovement = GameObject.Find("NASA").GetComponent<PlayerController>();
        cameraFollow = GameObject.Find("Main Camera").GetComponent<CameraFollow>();

        // Ensure the panel and button references are assigned
        if (panel == null || continueButton == null || mainMenuButton == null)
        {
            Debug.LogError("Panel, continue button, or main menu button is not assigned.");
            return;
        }

        // Pause the game by setting time scale to 0
        PauseGame();

        // Show the panel
        panel.SetActive(true);

        // Add listeners to the buttons
        continueButton.onClick.AddListener(ResumeGame);
        mainMenuButton.onClick.AddListener(LoadMainMenu);
    }

    private void Update()
    {
        // Check for Escape key press
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    private void ResumeGame()
    {
        if (!hasPlayedSound)
        {
            sfx321.PlaySound();
            hasPlayedSound = true; // Set the flag to true after playing the sound
        }
        
        Cursor.visible = false;
        // Hide the panel
        panel.SetActive(false);

        // Resume the game by setting time scale to 1
        Time.timeScale = 1f;

        if (playerMovement != null) playerMovement.enabled = true;
        if (cameraFollow != null) cameraFollow.enabled = true;

        isPaused = false;
    }

    private void PauseGame()
    {
        Cursor.visible = true;
        // Show the panel
        panel.SetActive(true);

        // Pause the game by setting time scale to 0
        Time.timeScale = 0f;

        if (playerMovement != null) playerMovement.enabled = false;
        if (cameraFollow != null) cameraFollow.enabled = false;

        isPaused = true;
    }

    private void LoadMainMenu()
    {
        Cursor.visible = true;
        // Load the MainMenu scene
        SceneManager.LoadScene("MainMenu");
    }
}
