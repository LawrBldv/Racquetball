using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneButtonManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    // Start is called before the first frame update
    void Start()
    {
         Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (scoreText != null)
        {
            scoreText.text = "And so, NASA fended Pluto off for " + Ball.playerScore + "\nyears before it crashed the planet party.";
        }
    }

    public void QuitGame()
        {
        Application.Quit();
    }

    public void LoadGame()
    {
        Ball.playerScore = 0;
        SceneManager.LoadScene("RacquetBall");
    }

    public void LoadRetry()
    {
        SceneManager.LoadScene("EndMenu");
    }

     public void LoadMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}