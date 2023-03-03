using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    private void Start()
    {
        SpawnThePlayer();
    }
    private void SpawnThePlayer()
    {
        int randomNumber = Random.Range(-10, 10);
        if (PhotonNetwork.IsConnectedAndReady) PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(randomNumber, 0, randomNumber), Quaternion.identity);
    }
}
