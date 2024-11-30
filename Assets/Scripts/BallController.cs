using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public List<Transform> spawnPoints; // Assuming you have a list of spawn points in your scene
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        TeleportBall();
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("OuterBound"))
        {
            TeleportBall();
        }
    }

    public void TeleportBall()  // Teleport the ball to random spawn point
    {
        Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = randomSpawnPoint.position;
    }
}
