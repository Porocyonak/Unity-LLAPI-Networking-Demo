using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

    public LogicHandler logicHandler;
    //private Rigidbody player1Rb;
    //private Rigidbody player0Rb;

    // Use this for initialization
    void Start () {
        //player1Rb = logicHandler.binaryPacker.player1.GetComponent<Rigidbody>();
        //player0Rb = logicHandler.binaryPacker.player0.GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Move(Vector2 inputs, int playerNum) {
        float hor = inputs.x;
        float vert = inputs.y;

        Rigidbody playerRb = logicHandler.playerStructs[playerNum].playerRb;

        Vector3 move = new Vector3(hor, 0, vert).normalized*.1f;

        playerRb.MovePosition(playerRb.position + playerRb.transform.TransformDirection(move) * 50f * Time.fixedDeltaTime); //not really sure if this is the right way to do it, dont do this in your game lol

    }

}
