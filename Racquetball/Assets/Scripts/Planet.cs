using UnityEngine;

public class Planet : MonoBehaviour
{
    private GameObject player; // Assignable Player GameObject in the Inspector

    private Ball pluto;

    private SFXManager sfx;

    private void Start()
    {
        pluto = GameObject.Find("Pluto").GetComponent<Ball>();
    }

    private void Update(){
        player = GameObject.Find("NASA");
        sfx = GameObject.Find("RacquetBallArena").GetComponent<SFXManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == player) // Check if the collided object is the assigned Player GameObject
        {
            sfx.PlaySFX(1);
            string objectName = gameObject.name;

            switch (objectName)
            {
                case "Mercury(Clone)":
                    Ball.playerScore += 2;
                    break;
                case "Venus(Clone)":
                    Ball.playerScore += 3;
                    break;
                case "Earth(Clone)":
                    Ball.playerScore += 4;
                    break;
                case "Mars(Clone)":
                    Ball.playerScore += 5;
                    break;
                case "Jupiter(Clone)":
                    Ball.playerScore *= 4;
                    break;
                case "Saturn(Clone)":
                    Ball.playerScore *= 3;
                    break;
                case "Uranus(Clone)":
                    Ball.playerScore *= 2;
                    break;
                case "Neptune(Clone)":
                    Ball.playerScore *= 2;
                    break;
                default:
                    Debug.Log("This is an unidentified object.");
                    break;
            }

            pluto.UpdateUI();
            Destroy(gameObject);
        }

        // Check if the collided object has the tag "Floor"
        if (collision.gameObject.CompareTag("Floor"))
        {
            // Destroy the game object after 1.5 seconds
            Destroy(gameObject, 1.5f);
        }
    }
}
