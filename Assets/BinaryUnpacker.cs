using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinaryUnpacker : MonoBehaviour {

    public LogicHandler logicHandler;
    public GameObject emptyCube;

    const float UNCHANGED = 0.0000006f; //arbitrary number to check how the final vector is affected

    public void ReadPlayer(byte[] binaryData) {
        logicHandler.dataRecieved += binaryData.Length;// + 28; //udp header size (is it included?)
        //print("recieved data of length: " + binaryData);
        logicHandler.UpdatePacketText();
        byte header = binaryData[0];
        int finalDataIndex = 1;
        Vector3 tempPos = new Vector3(UNCHANGED, UNCHANGED, UNCHANGED); //0.0000006 six 0s and then 6, unchanged mod

        if ((byte)(header << 1) >> 7 == 1) {
            tempPos.x = System.BitConverter.ToSingle(binaryData, finalDataIndex);//binaryData[finalDataIndex++];
            //print("tempos.x: " + tempPos.x);
            finalDataIndex += 4;
        }
        if ((byte)(header << 2) >> 7 == 1) {
            tempPos.y = System.BitConverter.ToSingle(binaryData, finalDataIndex);//binaryData[finalDataIndex++];
            finalDataIndex += 4;
        }
        if ((byte)(header << 3) >> 7 == 1) {
            tempPos.z = System.BitConverter.ToSingle(binaryData, finalDataIndex);//binaryData[finalDataIndex++];
            finalDataIndex += 4;
        }

        Vector3 finalVec = FixVector(tempPos, logicHandler.playerStructs[(byte)(header >> 7)].playerGameObject.transform.position);
        //print(finalVec);
        //GameObject fake = Instantiate(emptyCube);
        //emptyCube.transform.position = finalVec;
        logicHandler.playerStructs[(byte)(header>>7)].playerGameObject.transform.position = finalVec;
        InterpolatePlayer(binaryData);
        //print(FixVector(tempPos, logicHandler.binaryPacker.player1.transform.position));
        //logicHandler.binaryPacker.player1.transform.position = FixVector(tempPos, logicHandler.binaryPacker.player1.transform.position);

    }

    Vector3 FixVector(Vector3 vec, Vector3 fixedData) {
        Vector3 temp = fixedData;
        //print("fixed vector: vev.x=" + vec.x + " fixedData.x=" + fixedData.x + " vec.y= " + vec.y + " fixedData.y=" + fixedData.y);
        if(vec.x != UNCHANGED){
            temp.x = vec.x;
        } else {
            temp.x = fixedData.x;
        }

        if (vec.y != UNCHANGED) {
            temp.y = vec.y;
            //print("vecy does: " + vec.y + " not equal unchanged: " + UNCHANGED);
        } else {
            temp.y = fixedData.y;
        }

        if (vec.z != UNCHANGED) {
            temp.z = vec.z;
        } else {
            temp.z = fixedData.z;
        }
        //print("fixeddata.y = " + fixedData.y);
        //print("temp.y = " + temp.y);
        //print("actual y = " + logicHandler.binaryPacker.player1.transform.position.y);

        return temp;
    }

    public Vector2 mostRecentMovementData = new Vector2(0,0); //TODO: PUT IN STRUCT
    public int playerTemp = -1;

    public void InterpolatePlayer(byte[] data) {
        //print("interpolate called!");
        //int isWPressed = (byte)(data[0] << 4)>>7;
        //int isSPressed = (byte)(data[0] << 5)>>7; //only need booleans though
        //int isDPressed = (byte)(data[0] << 6)>>7;
        //int isAPressed = (byte)(data[0] << 7)>>7;

        Vector2 inputs = new Vector2(0, 0);
        //if (isWPressed == 1) {
        if ((byte)(data[0] << 4)>>4 > 0){
            //print("interpolation needed!");
            inputs.x = ((byte)(data[0] << 4) >> 7) - ((byte)(data[0] << 5) >> 7);
            inputs.y = ((byte)(data[0] << 6) >> 7) - ((byte)(data[0] << 7) >> 7);
            //print("inputs x is " + inputs.x);
            //print("inputs y is " + inputs.y);
            //logicHandler.playerMove.Move(inputs, (byte)(data[0]>>7)); //coroutine to move player asynchronously?
        }
        playerTemp = (byte)(data[0] >> 7);
        mostRecentMovementData = inputs;
        //print("mostRecentMovementData is now: " + inputs);
        // MOVE SHOULD CHECK IF PLAYER IS STUCK in a non-physics situation, like when stunned
        // make player move up
        //}

    }

}
