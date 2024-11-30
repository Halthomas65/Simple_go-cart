using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviourPunCallbacks
{
    public string playerName;
    public int playerId;
    public int score = 0;

    private Rigidbody rb;
    private AudioSource audioSource;
    public AudioClip warningSound;

    public Transform innerBoundary;
    public Transform outerBoundary;
    private bool isOutOfInnerBoundary = false;

    private CubeController cameraController;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        cameraController = GetComponentInChildren<CubeController>();

        if (photonView.IsMine)
        {
            playerName = PhotonNetwork.NickName;
            playerId = PhotonNetwork.LocalPlayer.ActorNumber;
        }
    }

    void Update()
    {
        if (!photonView.IsMine) return;

        CheckBoundaries();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!photonView.IsMine) return;

        if (collision.gameObject.CompareTag("Ball"))
        {
            score++;
            photonView.RPC("UpdateScore", RpcTarget.All, score);

            BallController ball = collision.gameObject.GetComponent<BallController>();
            if (ball != null)
            {
                ball.TeleportBall();
            }
        }
    }

    void CheckBoundaries()
    {
        if (!isOutOfInnerBoundary && !IsWithinBoundary(innerBoundary))
        {
            isOutOfInnerBoundary = true;
            PlayWarningSound();
        }
        else if (isOutOfInnerBoundary && IsWithinBoundary(innerBoundary))
        {
            isOutOfInnerBoundary = false;
        }

        if (!IsWithinBoundary(outerBoundary))
        {
            EnterSpectatorMode();
        }
    }

    bool IsWithinBoundary(Transform boundary)
    {
        Vector3 playerPos = transform.position;
        Vector3 boundaryPos = boundary.position;
        Vector3 boundaryScale = boundary.localScale;

        return Mathf.Abs(playerPos.x - boundaryPos.x) < boundaryScale.x / 2 &&
               Mathf.Abs(playerPos.z - boundaryPos.z) < boundaryScale.z / 2;
    }

    void PlayWarningSound()
    {
        if (audioSource != null && warningSound != null)
        {
            audioSource.PlayOneShot(warningSound);
        }
    }

    void EnterSpectatorMode()
    {
        rb.isKinematic = true;
        GetComponent<Collider>().enabled = false;
        transform.position = Vector3.zero; // Teleport back to the center of the field

        if (cameraController != null)
        {
            cameraController.enabled = true;
        }

        // Disable other components that control player movement/actions
        // For example:
        // GetComponent<PlayerMovement>().enabled = false;
    }

    [PunRPC]
    void UpdateScore(int newScore)
    {
        score = newScore;
    }
}