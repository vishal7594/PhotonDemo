using UnityEngine;
using Photon.Pun;
using Photon.Realtime; //for create the room
using UnityEngine.SceneManagement;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    #region UNITY_METHODS

    public GameManager gameManagerInstance;
    public System.Collections.Generic.List<RoomInfo> roomDataList;
    public System.Collections.Generic.List<GameObject> roomListGameObject;
    public System.Collections.Generic.List<GameObject> playerListGameObject;

    [SerializeField] private GameObject playButton;

    private void Start()
    {
        InvokeRepeating(nameof(CheckInterNetConnection), 1, 1);
        gameManagerInstance.ActivateMyPane(gameManagerInstance.enterNamePanel.name);
        roomDataList = new System.Collections.Generic.List<RoomInfo>();
        roomListGameObject = new System.Collections.Generic.List<GameObject>();

        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space)) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    #endregion


    #region MY_METHODS

    private void CheckInterNetConnection()
    {
        //Debug.Log("<color>InterNet Connection  " + PhotonNetwork.NetworkClientState + "</color>");
    }

    public void OnClickSubmit()
    {
        string name = gameManagerInstance.enterNameInputField.text;

        if (!string.IsNullOrEmpty(name))
        {
            PhotonNetwork.LocalPlayer.NickName = name;
            PhotonNetwork.ConnectUsingSettings();
            gameManagerInstance.ActivateMyPane(gameManagerInstance.connectionPanel.name);
        }
        else
        {
            Debug.Log("Enter the name....");
        }
    }

    public void OnClickCreateRoom() //create your personal room
    {
        string roomName = gameManagerInstance.roomNameInputField.text;

        if (string.IsNullOrEmpty(roomName))
        {
            roomName += Random.Range(0, 100);
        }
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = (byte)int.Parse(gameManagerInstance.maxPlayerInputField.text); // set the max player for room
        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }

    public void OnClickRoomListButton()
    {
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }
        gameManagerInstance.ActivateMyPane(gameManagerInstance.showRoomListPanel.name);
    }

    public void OnClickRoomJoinButton(string roomName)
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
            PhotonNetwork.JoinRoom(roomName);
        }
    }

    public void OnClickBackButton()
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }
        gameManagerInstance.ActivateMyPane(gameManagerInstance.lobbyPanel.name);
    }

    public void BackFromPlayerList()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
        gameManagerInstance.ActivateMyPane(gameManagerInstance.lobbyPanel.name);
    }

    public void GenertaeRoomListData(System.Collections.Generic.List<RoomInfo> refrenseRoomList)
    {
        Debug.Log(" child count || loop  => " + refrenseRoomList.Count);
        for (int i = 0; i < refrenseRoomList.Count; i++)
        {
            string roomName = refrenseRoomList[i].Name;
            GameObject spawnRoomListData = Instantiate(gameManagerInstance.roomListPanelPrefab, gameManagerInstance.roomListPanelPrent.transform);
            spawnRoomListData.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = "Room name : " + refrenseRoomList[i].Name;
            spawnRoomListData.transform.GetChild(1).GetComponent<TMPro.TMP_Text>().text = "Max player : " + refrenseRoomList[i].MaxPlayers;
            spawnRoomListData.transform.GetChild(2).GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => OnClickRoomJoinButton(roomName));//refrenseRoomList[i].Name));
            roomListGameObject.Add(spawnRoomListData);
        }
    }

    public void ResetRoomListData()
    {
        if (roomListGameObject.Count > 0)
        {
            for (int i = 0; i < roomListGameObject.Count; i++)
            {
                Destroy(roomListGameObject[i]);
            }
            roomListGameObject.Clear();
        }
    }

    public void OnClickPlay()
    {
        // SceneManager.LoadScene(0); // we dont use this because we have to load the scene of all player who join the room, so we use belove photon scene load
        if (PhotonNetwork.IsMasterClient) PhotonNetwork.LoadLevel(1);

    }

    #endregion


    #region PHOTON_CALLBACKS

    public override void OnConnected()
    {
        Debug.Log("Photon connected to internet => ");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + "   photon connect to master => ");
        gameManagerInstance.ActivateMyPane(gameManagerInstance.lobbyPanel.name);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Create the room || current Room =>  " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Join the room || Join Room =>  " + PhotonNetwork.LocalPlayer.NickName);

        playButton.SetActive(PhotonNetwork.IsMasterClient);

        gameManagerInstance.ActivateMyPane(gameManagerInstance.insideRoomPanel.name);

        if (playerListGameObject == null)
        {
            playerListGameObject = new System.Collections.Generic.List<GameObject>();
        }

        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            GameObject spawnRoomListData = Instantiate(gameManagerInstance.playerDataItemPrefab, gameManagerInstance.playerDataItemPrefabParent.transform);

            spawnRoomListData.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = "Room name : " + PhotonNetwork.PlayerList[i].NickName;

            //actorenumber is unique number provided all player who join the room & it is every time change when leave and join once again.            
            spawnRoomListData.transform.GetChild(1).gameObject.SetActive((PhotonNetwork.PlayerList[i].ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber) ? true : false);
            playerListGameObject.Add(spawnRoomListData);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        GameObject spawnRoomListData = Instantiate(gameManagerInstance.playerDataItemPrefab, gameManagerInstance.playerDataItemPrefabParent.transform);

        spawnRoomListData.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = "Room name : " + newPlayer.NickName;

        spawnRoomListData.transform.GetChild(1).gameObject.SetActive((newPlayer.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber) ? true : false); //actorenumber is unique number provided all player who join the room & it is every time change when leave and join once again.
        playerListGameObject.Add(spawnRoomListData);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Destroy(playerListGameObject[otherPlayer.ActorNumber]);
        playerListGameObject.Remove(playerListGameObject[otherPlayer.ActorNumber]);

        playButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnLeftRoom()
    {
        gameManagerInstance.ActivateMyPane(gameManagerInstance.lobbyPanel.name);
        for (int i = 0; i < playerListGameObject.Count; i++)
        {
            Destroy(playerListGameObject[i]);
            playerListGameObject.Remove(playerListGameObject[i]);
        }
    }

    public override void OnRoomListUpdate(System.Collections.Generic.List<RoomInfo> roomList)
    {
        ResetRoomListData(); //reset the all room list data 

        for (int i = 0; i < roomList.Count; i++)
        {
            Debug.Log("<color=yellow>Roomlist name => </color>" + roomList[i].Name);

            if (!roomList[i].IsOpen || !roomList[i].IsVisible || roomList[i].RemovedFromList)
            {
                if (roomDataList.Contains(roomList[i])) roomDataList.Remove(roomList[i]);
            }
            else
            {
                if (roomDataList.Contains(roomList[i])) roomDataList[i] = roomList[i];

                else roomDataList.Add(roomList[i]);
            }
        }
        GenertaeRoomListData(roomList);
    }

    public override void OnLeftLobby()
    {
        ResetRoomListData();
        roomDataList.Clear();
    }

    #endregion
}
