using UnityEngine;
using Photon.Pun;

public class PlayerSetUp : MonoBehaviourPunCallbacks
{
    public GameObject myPlayer;
    public GameObject opponentPlayer;
    public GameObject player;
    public Camera camera;


    private void Start()
    {
        player = myPlayer.transform.parent.gameObject;
        if (photonView.IsMine)
        {
            myPlayer.SetActive(true);
            opponentPlayer.SetActive(false);
            camera.gameObject.SetActive(true);
        }
        else
        {
            myPlayer.SetActive(false);
            opponentPlayer.SetActive(true);
            camera.gameObject.SetActive(false);
        }
    }
}