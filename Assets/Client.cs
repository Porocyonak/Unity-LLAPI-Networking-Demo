using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Text;

public class Client : MonoBehaviour {

    int connectionID;
    int maxConnections = 2;
    int reliableChannelID;
    int hostID;
    int socketPort = 24322;
    byte error;

    public string testName = "unchanged";

    bool isServer = false;

    void Start() {
        NetworkTransport.Init();
    }

    public void InitAndConnectClient() {
        //NetworkTransport.Init();
        ConnectionConfig config = new ConnectionConfig();
        reliableChannelID = config.AddChannel(QosType.ReliableSequenced);
        HostTopology topology = new HostTopology(config, maxConnections);
        hostID = NetworkTransport.AddHost(topology, 0);
        Debug.Log("Client socket open!  connecting..");
        connectionID = NetworkTransport.Connect(hostID, "127.0.0.1", socketPort, 0, out error);
        print("client connect2host connectionID= " + connectionID);
    }

    public void ClientSendMessage(string message) {
        byte[] buffer = Encoding.Unicode.GetBytes(message);
        NetworkTransport.Send(hostID, connectionID, reliableChannelID, buffer, message.Length * sizeof(char), out error);
    }


    void Update() {
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
                    string message = Encoding.Unicode.GetString(recBuffer, 0, dataSize);
                    print("Recieved: " + message);
                    testName = message;
                    break;
                case NetworkEventType.DisconnectEvent:
                    print("d/c from host!");
                    break;

            }
    }

    public void Disconnect() {
        NetworkTransport.Shutdown();
    }

}