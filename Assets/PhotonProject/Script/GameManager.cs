using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject lobbyPanel;
    public GameObject gamePlayPanel;
    public GameObject connectionPanel;
    public GameObject enterNamePanel;
    public GameObject showRoomListPanel;
    public GameObject insideRoomPanel;

    public GameObject roomListPanelPrefab;
    public GameObject playerDataItemPrefab;
    public GameObject playerDataItemPrefabParent;
    public GameObject roomListPanelPrent;

    public TMPro.TMP_InputField enterNameInputField;
    public TMPro.TMP_InputField roomNameInputField;
    public TMPro.TMP_InputField maxPlayerInputField;

    #region MY_GAMEMANAGER_METHOD

    public void ActivateMyPane(string panelName)
    {
        lobbyPanel.SetActive(panelName.Equals(lobbyPanel.name));
        gamePlayPanel.SetActive(panelName.Equals(gamePlayPanel.name));
        connectionPanel.SetActive(panelName.Equals(connectionPanel.name));
        enterNamePanel.SetActive(panelName.Equals(enterNamePanel.name));
        showRoomListPanel.SetActive(panelName.Equals(showRoomListPanel.name));
        insideRoomPanel.SetActive(panelName.Equals(insideRoomPanel.name));
    }

    #endregion
}