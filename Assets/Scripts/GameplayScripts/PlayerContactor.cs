using Photon.Pun;
using UnityEngine;

public class PlayerContactor : MonoBehaviour
{
    int playerId;
    int playerScore;
    public AudioClip nearBoundWarning;

    void Start()
    {
        // playerId = PhotonNetwork.LocalPlayer.ActorNumber;

    }

    private void OnTriggerEnter(Collider other)
    {
        // - Every time a player touch a ball, they get 1 point and the ball will be teleported to another spot.
        if (other.CompareTag("Ball"))
        {
            playerScore++;
            // Teleport the ball to another spot
            other.GetComponent<BallController>().TeleportBall();
        }
        else if (other.CompareTag("InnerBound"))
        {
            // Play near bound warning sound using player's audio source
            AudioSource audioSource = other.GetComponent<AudioSource>();
            audioSource.PlayOneShot(nearBoundWarning);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("OuterBound"))
        {
            // Player died, show score summary screen
            // Assuming you have a method named ShowScoreSummaryScreen()
            // ShowScoreSummaryScreen();
        }
    }

    void Die()
    {
        // Disable this gameobject
        gameObject.SetActive(false);

        // ShowScoreSummaryScreen();
    }
}