using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    private List<Transform> spawnPoints; // Assuming you have a list of spawn points in your scene
    private Rigidbody rb;
    private Collider areaCollider;
    private LobbyController lobbyController; // Assuming you have a reference to the LobbyController


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        areaCollider = GetComponent<Collider>();
        // spawnPoints = new List<Transform>(GameObject.FindObjectsOfType<SpawnPoint>().Select(sp => sp.transform)); // Assuming SpawnPoint script is attached to spawn points
    }

    void Update()
    {
        // Add your custom logic here
    }

    // void OnCollisionEnter(Collision collision)
    // {
    //      if (collision.gameObject.CompareTag("Player"))
    //     {
    //         PhotonPlayer player = collision.gameObject.GetComponent<PhotonView>().Owner;
    //         string playerNickname = player.NickName;

    //         ScoreManager.Instance.AddPoint(playerNickname);
    //         TeleportBallToRandomSpawnPoint();
    //     }
    // }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Area"))
        {
            TeleportBallToRandomSpawnPoint();
        }
    }

    public void TeleportBallToRandomSpawnPoint()
    {
        Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = randomSpawnPoint.position;
    }
}
