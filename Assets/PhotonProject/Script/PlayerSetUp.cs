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
            Debug.Log("myplayer Parent => " + myPlayer.transform.parent.name);
            camera.transform.SetParent(myPlayer.transform.parent.transform);
            camera.transform.position = Vector3.zero;
        }
        else
        {
            myPlayer.SetActive(false);
            opponentPlayer.SetActive(true);
        }
    }
}