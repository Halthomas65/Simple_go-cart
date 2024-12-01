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

        // Check if ScoreManager instance is null
        if (ScoreManager.Instance == null)
        {
            Debug.LogError("ScoreManager instance is null. Please ensure the ScoreManager prefab is added to the scene.");
            return;
        }

        // Add the player to the ScoreManager
        ScoreManager.Instance.AddPlayer(playerId);
    }

    void FixedUpdate()
    {
        // start Game
        // Check if the player is masterClient
        // bool isMasterClient = PhotonNetwork.IsMasterClient;

        // Get input enter key
        if (Input.GetKeyDown(KeyCode.Return)) // && isMasterClient)
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

        // Set the player state in the ScoreManager to dead
        ScoreManager.Instance.UpdateState(playerId, false);

        // Disable this gameobject
        gameObject.SetActive(false);

        // TODO: ShowScoreSummaryScreen();
    }
}