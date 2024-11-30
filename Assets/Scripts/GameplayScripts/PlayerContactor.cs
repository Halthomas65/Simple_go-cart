using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerContactor : MonoBehaviour
{
    int playerId;
    public AudioSource audioSource;
    public AudioClip nearBoundWarning,
                hitBallSound,
                dieSound;

    void Start()
    {
        // Assign a unique ID for each player
        playerId = PhotonNetwork.LocalPlayer.ActorNumber;

        // Add the player to the ScoreManager
        ScoreManager.Instance.AddPlayer(playerId);
    }

    void FixedUpdate()
    {
        // start Game
        // Get input enter key
        if (Input.GetKeyDown(KeyCode.Return))
        {
            GameManager.Instance.StartGame();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // - Every time a player touch a ball, they get 1 point and the ball will be teleported to another spot.
        if (other.CompareTag("Ball"))
        {
            audioSource.PlayOneShot(hitBallSound);

            ScoreManager.Instance.UpdateScore(playerId, 1);
            // Teleport the ball to another spot
            other.GetComponent<BallController>().TeleportBall();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("OuterBound"))
        {
            // Player died, show score summary screen
            Die();
        }
        else if (other.CompareTag("InnerBound"))
        {
            // Play near bound warning sound using player's audio source
            audioSource.PlayOneShot(nearBoundWarning);
        }
    }

    void Die()
    {
        // Find the Main camera and let it play the die sound
        AudioSource camAudio = Camera.main.GetComponent<AudioSource>();
        if (camAudio != null)
        {
            camAudio.PlayOneShot(dieSound);
        }
        else
        {
            camAudio = Camera.main.AddComponent<AudioSource>();
            camAudio.PlayOneShot(dieSound);
        }

        // Disable this gameobject
        gameObject.SetActive(false);

        // ShowScoreSummaryScreen();
    }
}