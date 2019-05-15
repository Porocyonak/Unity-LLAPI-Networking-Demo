using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Text;
using System.Collections.Generic;

public class Server : MonoBehaviour {

    List<int> connectionID = new List<int>();
    int maxConnections = 2;
    int reliableChannelID;
    int hostID;
    int socketPort = 24322;
    byte error;

    bool isServer = false;

    void Start() {
        NetworkTransport.Init();
    }
    public void InitSever() {
        //NetworkTransport.Init();
        ConnectionConfig config = new ConnectionConfig();
        reliableChannelID = config.AddChannel(QosType.ReliableSequenced);
        HostTopology topology = new HostTopology(config, maxConnections);
        hostID = NetworkTransport.AddHost(topology, socketPort, null);
        Debug.Log("server opened!");
        isServer = true;
        
    }

    public void HostSendMessage(int theID) {
        try {
            int temp = connectionID.Find(x => x == theID);
            byte[] buffer = Encoding.Unicode.GetBytes(theID.ToString());
            NetworkTransport.Send(hostID, temp, reliableChannelID, buffer, theID.ToString().Length * sizeof(char), out error);
        } catch {
            Debug.Log("Error sending message! Probably couldn't find connectionID, or used an invalid connection!");
        }
    }

    void Update() {
        if (isServer) {
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
                    connectionID.Add(recConnectionId); //now communicating with client from host.
                    break;
                case NetworkEventType.DataEvent:
                    string msg = Encoding.Unicode.GetString(buffer, 0, dataSize);
                    Debug.Log("Message: " + msg);
                    break;
                case NetworkEventType.DisconnectEvent:
                    connectionID.Remove(recConnectionId);
                    Debug.Log("Removed connection: " + recConnectionId.ToString() + "!");
                    break;
            }

        } 
    }

    public void Disconnect() { //tbd
        NetworkTransport.Shutdown();
    }

}
