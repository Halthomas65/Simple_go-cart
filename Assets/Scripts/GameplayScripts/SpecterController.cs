using Photon.Pun;
using UnityEngine;

public class DeadPlayerManager : MonoBehaviour
{
    public GameObject cam;
    public CarController carController;

    void Start()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            cam.SetActive(false);
            carController.enabled = false;
        }
    }
}