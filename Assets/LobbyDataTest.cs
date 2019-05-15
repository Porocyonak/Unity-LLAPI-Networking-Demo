using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyDataTest : MonoBehaviour {

    public Button clientSendButton;
    public Button hostSendButton;
    public LobbyData lobbyData;

    void Awake() {
        lobbyData = GameObject.Find("LobbyData").GetComponent<LobbyData>();
        lobbyData.clientSendButton = clientSendButton;
        //lobbyData.clientSendButton.onClick.AddListener(() => lobbyData.ClientSendMessage("fuck"));
        lobbyData.hostSendButton = hostSendButton;
        //lobbyData.hostSendButton.onClick.AddListener(() => lobbyData.HostSendMessage(1));
        print("assigned!");
    }

    public void StartGame() {
        SceneManager.LoadScene("SampleScene");
        lobbyData.SendMovementDataToClient(new byte[1]);
        lobbyData.isInGame = true;
        lobbyData.isInLobby = false;
    }

}
