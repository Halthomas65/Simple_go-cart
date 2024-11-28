using UnityEngine;

public class BallManager : MonoBehaviour
{
    public int maxBalls = 5;
    public Transform[] ballSpawns;
    public GameObject ballPrefab;

    private GameObject[] balls;

    void Start()
    {
        balls = new GameObject[maxBalls];
        SpawnBalls();
    }

    void SpawnBalls()
    {
        for (int i = 0; i < maxBalls; i++)
        {
            balls[i] = Instantiate(ballPrefab, GetRandomSpawnPoint(), Quaternion.identity);
        }
    }

    public void TeleportBall(GameObject player)
    {
        for (int i = 0; i < maxBalls; i++)
        {
            if (!balls[i].activeInHierarchy)
            {
                balls[i].transform.position = GetRandomSpawnPoint();
                balls[i].SetActive(true);
                return;
            }
        }
    }

    private Vector3 GetRandomSpawnPoint()
    {
        int randomIndex = Random.Range(0, ballSpawns.Length);
        return ballSpawns[randomIndex].position;
    }
}