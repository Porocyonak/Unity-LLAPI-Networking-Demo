using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogicHandler : MonoBehaviour {

    public CameraFollow cameraFollow;
    public PlayerMove playerMove;
    public BinaryPacker binaryPacker;
    public BinaryUnpacker binaryUnpacker;

    public struct PlayerStruct {
        public Vector3 prevPosition;
        public GameObject playerGameObject;
        public Vector2 prevInputs;
        public Rigidbody playerRb;
        

        public void Init(Vector3 _prevPosition, GameObject _playerGameObject) {
            prevPosition = _prevPosition;
            playerGameObject = _playerGameObject;
            prevInputs = new Vector2(0, 0);
            playerRb = playerGameObject.GetComponent<Rigidbody>();
        }
    }

    public PlayerStruct[] playerStructs;

    public float dataRecieved = 0;
    public float dataSent = 0;
    public Text packetUI;

    int whichPlayer = -1;

    public LobbyData lobbyData;
    private Vector2 moveInputs = new Vector4(0,0); // 0 is d,a and 1 is w,s. +1, -1 respectively

    float networkFrequency = (1000f / 16f)/1000f; //16 tick packet rate
    float networkTimer;// = 1000f / 16f;

    // use vector4 if you want to constantly sync input despite it cancelling out
    bool willMove = false;
    bool fixedInProgress = false;

    public byte[] bytesPackedForP1;

    void Awake() {
        try {
            lobbyData = GameObject.Find("LobbyData").GetComponent<LobbyData>();
            lobbyData.logicHandler = this;
            
        } catch {
            print("CRITICAL ERROR LOBBYDATA NOT FOUND");
        }
    }

	// Use this for initialization
	void Start () {

        playerStructs = new PlayerStruct[2];
        playerStructs[0].Init(new Vector3(0, 0, 0), binaryPacker.player0);
        //playerStructs[0].playerRb = playerStructs[0].playerGameObject.GetComponent<Rigidbody>();
        playerStructs[1].Init(new Vector3(0, 0, 0), binaryPacker.player1);
        //playerStructs[1].playerRb = playerStructs[1].playerGameObject.GetComponent<Rigidbody>();

        if (!lobbyData.isServer) {
            cameraFollow.target = playerStructs[0].playerGameObject;
            whichPlayer = 0;
        } else {
            whichPlayer = 1;
        }

        networkTimer = networkFrequency;
        packetUI = GameObject.Find("Canvas/PacketCounter").GetComponent<Text>();
        //DontDestroyOnLoad(gameObject);
    }
	
	// Update is called once per frame
    // Take player input and convert to movements in fixed update
    // Hardcoding for simplicity
	void Update () {
        networkTimer -= Time.deltaTime;
        if (networkTimer <= 0){
            networkTimer = networkFrequency;

            if (lobbyData.isServer) {
                bytesPackedForP1 = binaryPacker.PackMovementData(1, moveInputs);
                if (bytesPackedForP1.Length >= 1) {
                    lobbyData.SendMovementDataToClient(bytesPackedForP1);
                    dataSent += bytesPackedForP1.Length + 28;
                    UpdatePacketText();
                    //packetUI.text = "Total data sent (uploaded): " + dataSent + " bytes\nTotal data recieved(downloaded): " + dataRecieved + " bytes\n";
                }
            } else {
                bytesPackedForP1 = binaryPacker.PackMovementData(0, moveInputs); //bad var name
                if (bytesPackedForP1.Length >= 1) {
                    lobbyData.SendMovementDataToServer(bytesPackedForP1);
                    dataSent += bytesPackedForP1.Length + 28;
                    UpdatePacketText();
                    //packetUI.text = "Total data sent (uploaded): " + dataSent + " bytes\nTotal data recieved(downloaded): " + dataRecieved + " bytes\n";
                    //print("sending data of length " + bytesPackedForP1.Length);
                }
            }
        }
        moveInputs = new Vector2(0, 0);

        if (Input.GetKey(KeyCode.W)) {
            moveInputs = new Vector2(moveInputs.x, 1);
        }
        if (Input.GetKey(KeyCode.S)) {
            if (moveInputs.y == 1) {
                moveInputs = new Vector2(moveInputs.x, 0);
            } else { moveInputs = new Vector2(moveInputs.x, -1); }
        }

        if (Input.GetKey(KeyCode.D)) {
                moveInputs = new Vector4(1, moveInputs.y);
            }
            if (Input.GetKey(KeyCode.A)) {
                if (moveInputs.x == 1) {// cancel out forward and back
                    moveInputs = new Vector2(0, moveInputs.y);
                } else { moveInputs = new Vector2(-1, moveInputs.y); }
            }

    }

    void FixedUpdate() {
        //fixedInProgress = true; //lock needed?  idk if fixed is ever called before update (threading?)
        if (moveInputs.magnitude!=0) {
            playerMove.Move(moveInputs, whichPlayer);
            //willMove = false;
        }
        if (binaryUnpacker.mostRecentMovementData.magnitude != 0) {
            //print("moving player " + binaryUnpacker.playerTemp);
            playerMove.Move(binaryUnpacker.mostRecentMovementData, binaryUnpacker.playerTemp);
        }
        //fixedInProgress = false;
    }

    void LateUpdate() {
        cameraFollow.FollowTarget();
    }

    public void UpdatePacketText() {
        packetUI.text = "Total data sent (uploaded): " + dataSent + " bytes\nTotal data recieved(downloaded): " + dataRecieved + " bytes\n";
    }
}
