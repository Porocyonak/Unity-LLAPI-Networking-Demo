using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyData : MonoBehaviour {

    public Button hostSendButton;
    public Button clientSendButton;

    public LogicHandler logicHandler;

    public bool isServer = false;
    int reliableChannelID;
    int hostID;
    int connectionID;
    int socketPort = 24322;
    int maxConnections = 2;
    byte error;
    public bool isInLobby = false;
    public bool isInGame = false;

    // Use this for initialization
    void Start () {
        print(sizeof(char));
        print(sizeof(byte));
        DontDestroyOnLoad(gameObject);
	}
	
	public void HostLobby() {
        isServer = true;
        //try to host lobby, if successful switch scenes
        NetworkTransport.Init();
        ConnectionConfig config = new ConnectionConfig();
        reliableChannelID = config.AddChannel(QosType.ReliableSequenced);
        HostTopology topology = new HostTopology(config, maxConnections);
        hostID = NetworkTransport.AddHost(topology, socketPort, null);
        Debug.Log("server opened!");
        SceneManager.LoadScene("hostlobby");
        //hostSendButton = GameObject.Find("Canvas/hostSendButton").GetComponent<Button>();
        //hostSendButton.onClick.AddListener(() => HostSendMessage(1));
        isInLobby = true;
    }

    public void JoinLobby() {
        isServer = false;
        NetworkTransport.Init();
        ConnectionConfig config = new ConnectionConfig();
        reliableChannelID = config.AddChannel(QosType.ReliableSequenced);
        HostTopology topology = new HostTopology(config, maxConnections);
        hostID = NetworkTransport.AddHost(topology, 0);
        Debug.Log("Client socket open!  connecting..");
        connectionID = NetworkTransport.Connect(hostID, "127.0.0.1", socketPort, 0, out error);
        print("client connect2host connectionID= " + connectionID);
        print("error?: " + error);
        //try to join lobby, if successful switch scenes
        SceneManager.LoadScene("hostlobby");
        //clientSendButton = GameObject.Find("Canvas/clientSendButton").GetComponent<Button>();
        //clientSendButton.onClick.AddListener(() => ClientSendMessage("fuck"));
        isInLobby = true;
    }

    void Update() {
        if (isInGame) {

            //sorry this is kinda disgusting. basically have two functions if its in a lobby or not, and if its the server or not.

            if (!isServer) {
                int recHostID;
                int recConnectionID;
                int recChannelID;
                byte[] recBuffer = new byte[1024];
                int bufferSize = 1024;
                int dataSize;
                NetworkEventType recNetworkEvent = NetworkTransport.Receive(out recHostID, out recConnectionID, out recChannelID, recBuffer, bufferSize, out dataSize, out error);
                switch (recNetworkEvent) {
                    case NetworkEventType.ConnectEvent:
                        print("connection made to host: " + recHostID.ToString());
                        break;
                    case NetworkEventType.DataEvent:
                        byte[] newBuffer = new byte[dataSize];
                        for (int i = 0; i < dataSize; i++) {
                            newBuffer[i] = recBuffer[i];
                        }
                        logicHandler.binaryUnpacker.ReadPlayer(newBuffer);
                        break;
                    case NetworkEventType.DisconnectEvent:
                        print("d/c from host!");
                        break;

                }
            } else {
                int recHostId;
                int recConnectionId;
                int recChannelId;
                int bufferSize = 1024;
                byte[] buffer = new byte[1024];
                int dataSize;
                byte error;

                NetworkEventType networkEvent = NetworkTransport.Receive(out recHostId, out recConnectionId, out recChannelId, buffer, bufferSize, out dataSize, out error);
                switch (networkEvent) {
                    case NetworkEventType.ConnectEvent:
                        Debug.Log("Player " + recConnectionId.ToString() + " connected to: " + recHostId.ToString() + "! addding as connectionID for host"); //server id is 0, player is anything between 1 and 3 (max 4 players)
                        break;
                    case NetworkEventType.DataEvent:
                        byte[] newBuffer = new byte[dataSize];
                        for (int i = 0; i < dataSize; i++) {
                            newBuffer[i] = buffer[i];
                        }
                        logicHandler.binaryUnpacker.ReadPlayer(newBuffer); //0
                        break;
                    case NetworkEventType.DisconnectEvent:
                        Debug.Log("Removed connection: " + recConnectionId.ToString() + "!");
                        break;
                }
            }


        } else if (isInLobby) {
            if (!isServer) {
                int recHostID;
                int recConnectionID;
                int recChannelID;
                byte[] recBuffer = new byte[1024];
                int bufferSize = 1024;
                int dataSize;
                NetworkEventType recNetworkEvent = NetworkTransport.Receive(out recHostID, out recConnectionID, out recChannelID, recBuffer, bufferSize, out dataSize, out error);
                switch (recNetworkEvent) {
                    case NetworkEventType.ConnectEvent:
                        print("connection made to host: " + recHostID.ToString());
                        break;
                    case NetworkEventType.DataEvent:
                        print("game starting cuz host said so");
                        isInGame = true;
                        isInLobby = false;
                        SceneManager.LoadScene("SampleScene");
                        break;
                    case NetworkEventType.DisconnectEvent:
                        print("d/c from host!");
                        break;

                }
            } else {
                int recHostId;
                int recConnectionId;
                int recChannelId;
                int bufferSize = 1024;
                byte[] buffer = new byte[1024];
                int dataSize;
                byte error;

                NetworkEventType networkEvent = NetworkTransport.Receive(out recHostId, out recConnectionId, out recChannelId, buffer, bufferSize, out dataSize, out error);
                switch (networkEvent) {
                    case NetworkEventType.ConnectEvent:
                        Debug.Log("Player " + recConnectionId.ToString() + " connected to: " + recHostId.ToString() + "! addding as connectionID for host"); //server id is 0, player is anything between 1 and 3 (max 4 players)
                        break;
                    case NetworkEventType.DataEvent:
                        //print("game starting cuz I, the server, said so");
                        //SceneManager.LoadScene("SampleScene");
                        //SendMovementDataToClient(new byte[1]); //doesnt matter, basically send something to say ur starting
                        break;
                    case NetworkEventType.DisconnectEvent:
                        Debug.Log("Removed connection: " + recConnectionId.ToString() + "!");
                        break;
                }
            }

        }
    }

    public void SendMovementDataToServer(byte[] data) {
        //print("data length: " + data.Length);
        NetworkTransport.Send(hostID, connectionID, reliableChannelID, data, data.Length, out error); // figure out len
    } //is sizeof in bytes or bits

    public void SendMovementDataToClient(byte[] data) {
        NetworkTransport.Send(hostID, 1, reliableChannelID, data, data.Length, out error); //send to connection 1, player 1
    } // if more than one player, ideally more code has to be added to handle what data needs to be sent to who
      // for now i've left it hardcoded since the scope of this project is small


}


// fix this so its cleaner, make server send data to start game for all connected
// and do checks to join/quit like see if join fails or not