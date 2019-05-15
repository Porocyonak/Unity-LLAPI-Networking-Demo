using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinaryPacker : MonoBehaviour {

    // first 1 bit is the ID of the object we are syncing
    // next 3 bits are identifiers for whether or not the value changed (delta compression i think)
    // next 4 bits are the movement buttons pressed
    // the next (0, 1, 2, or 3)*32 bytes are the float values for the vectors we are syncing

    public GameObject player1;
    public GameObject player0;
    public LogicHandler logicHandler;

    public byte[] PackMovementData(short playerID, Vector2 inputs) {
        List<byte[]> floatBuffer = new List<byte[]>();
        byte firstByte = 0;
        byte sizeChecker = 0;
        int finalSize = 0;

        //float tempFuzzValue = .25f;
        //byte whatever = firstByte | playerID << 7;
        firstByte = (byte)(firstByte | (playerID << 7));

        //send position and movement input data

        //floats are wonky so do a precision check of decimals. only send data that changed locally to keep it efficient.
        if (Math.Abs(logicHandler.playerStructs[playerID].prevPosition.x - (float)Math.Round(logicHandler.playerStructs[playerID].playerGameObject.transform.position.x, 4)) > .0001f) {
            firstByte = (byte)(firstByte | (1 << 6));
            sizeChecker = (byte)(firstByte | (1 << 7));
            finalSize++;
            floatBuffer.Add(BitConverter.GetBytes(logicHandler.playerStructs[playerID].playerGameObject.transform.position.x));
        }
        if (Math.Abs(logicHandler.playerStructs[playerID].prevPosition.y - (float)Math.Round(logicHandler.playerStructs[playerID].playerGameObject.transform.position.y, 4)) > .0001f) {
            //prevPlayer1Position = player1.transform.position;
            firstByte = (byte)(firstByte | (1 << 5));
            sizeChecker = (byte)(firstByte | (1 << 6));
            finalSize++;
            floatBuffer.Add(BitConverter.GetBytes(logicHandler.playerStructs[playerID].playerGameObject.transform.position.y));
        }
        if (Math.Abs(logicHandler.playerStructs[playerID].prevPosition.z - (float)Math.Round(logicHandler.playerStructs[playerID].playerGameObject.transform.position.z, 4)) > .0001f) {
            //prevPlayer1Position = player1.transform.position;
            firstByte = (byte)(firstByte | (1 << 4));
            sizeChecker = (byte)(firstByte | (1 << 5)); // will bitshift check leftmost 3, XYZ00000 to check what to pack
            finalSize++;
            floatBuffer.Add(BitConverter.GetBytes(logicHandler.playerStructs[playerID].playerGameObject.transform.position.z));
        }

        logicHandler.playerStructs[playerID].prevPosition = new Vector3(
            (float)Math.Round(logicHandler.playerStructs[playerID].playerGameObject.transform.position.x, 4),
            (float)Math.Round(logicHandler.playerStructs[playerID].playerGameObject.transform.position.y, 4),
            (float)Math.Round(logicHandler.playerStructs[playerID].playerGameObject.transform.position.z, 4));
        

        //print("Inputs: " + inputs);
        // send inputs to eventually interpolate and look pretty and not glitchy
        if (inputs.x == 1) {
            firstByte = (byte)(firstByte | (1 << 3));
        }
        if (inputs.x == -1) {
            firstByte = (byte)(firstByte | (1 << 2));
        }
        if (inputs.y == 1) {
            firstByte = (byte)(firstByte | (1 << 1));
        }
        if (inputs.y == -1) {
            firstByte = (byte)(firstByte | (1 << 0));
        }

            
        int bufferIndex = 1;

        byte[] finalBuffer = new byte[1 + finalSize * sizeof(float)];
        finalBuffer[0] = firstByte;

        foreach (byte[] byteArray in floatBuffer) {
            for (int i = 0; i < byteArray.Length; i++) {
                finalBuffer[bufferIndex++] = byteArray[i];
            }
        }
        byte[] empty = new byte[0];

        if (inputs.x != logicHandler.playerStructs[playerID].prevInputs.x || inputs.y != logicHandler.playerStructs[playerID].prevInputs.y) {
            // if previous is different to what we have now, send the data
            // prevents infinite movement since 0,0 isnt sent if position didnt change
            logicHandler.playerStructs[playerID].prevInputs = inputs;
            return finalBuffer;
        } else {
            logicHandler.playerStructs[playerID].prevInputs = inputs;
            return (finalBuffer[0] == playerID * 128 ? empty : finalBuffer);
        }
        
    }

}
