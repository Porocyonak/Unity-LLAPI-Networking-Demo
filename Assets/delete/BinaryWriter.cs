using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinaryWriter : MonoBehaviour {

    //attached to GameObject with rigidbody (ill do inputs later, just default inputs to 00 00)

    Vector3 oldPos = new Vector3();
    Vector3 oldVel = new Vector3();
    Quaternion oldRot = new Quaternion();
    int oldHinput = 0;
    int oldVInput = 0; //i think its 00 and then 00 for h and v, idk can always easy fix

    int horizontalInput = 0;
    int verticalInput = 0;

    public Rigidbody rb;
    public BinaryReader reader;

    // Use this for initialization
    void Start() {
        AddToDictionary();
    }

    void AddToDictionary() {
        lookup.Add(0, oldPos.x == transform.position.x);
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown("k")) { //welp, it works, need to implement the input and the rotation, and im done. wew.
            PlayerDataToBytes();
        }
        //CreatePlayerHeader();
        //print(lookup[0]);
        //oldPos.x = 10f;
        //print(lookup[0]);
        //print("should be poo " + (oldPos.x == transform.position.x));
        /*
        byte hi = 255;
        byte pi = 0;
        byte[] test = new byte[2];
        test[0] = hi; //yeah so the 0 index is all the way on the right... as its supposed to
        test[1] = pi;
        byte[] test2 = BitConverter.GetBytes(Convert.ToInt16(255));
        print(test[0] + " " + test2[0]);
        print(test[1] + " " + test2[1]);
        */
    }

    List<byte[]> byteBuffer = new List<byte[]>();

    Dictionary<int, bool> lookup = new Dictionary<int, bool>(){};

    int CalculatePayloadByteSize() { //could remove if statements by mapping each oldPos -> transform.positon, since theyre both vec3 (and so on)
        int result = 0;
        if(oldPos.x != transform.position.x) {
            result += 4;
            byteBuffer.Add(BitConverter.GetBytes(transform.position.x));
        }
        if (oldPos.y != transform.position.y) {
            result += 4;
            byteBuffer.Add(BitConverter.GetBytes(transform.position.y));
        }
        if (oldPos.z != transform.position.z) {
            result += 4;
            byteBuffer.Add(BitConverter.GetBytes(transform.position.z));
        }

        if (oldVel.x != rb.velocity.x) {
            result += 4;
            byteBuffer.Add(BitConverter.GetBytes(rb.velocity.x));
        }
        if (oldVel.y != rb.velocity.y) {
            result += 4;
            byteBuffer.Add(BitConverter.GetBytes(rb.velocity.y)); //I'm sorry, but it works and its readable. I regret nothing.
        }
        if (oldVel.z != rb.velocity.z) {
            result += 4;
            byteBuffer.Add(BitConverter.GetBytes(rb.velocity.z));
        }

        if (oldRot.x != transform.rotation.x) {
            result += 4;
            byteBuffer.Add(BitConverter.GetBytes(transform.rotation.x));
        }
        if (oldRot.y != transform.rotation.y) {
            result += 4;
            byteBuffer.Add(BitConverter.GetBytes(transform.rotation.y));
        }
        if (oldRot.z != transform.rotation.z) {
            result += 4;
            byteBuffer.Add(BitConverter.GetBytes(transform.rotation.z));
        }
        if (oldRot.w != transform.rotation.w) {
            result += 4;
            byteBuffer.Add(BitConverter.GetBytes(transform.rotation.w)); //could also just get size of each array in list, but idc lol
            //maybe this way is more efficient, since i dont have to iterate again, so thats my excuse hehe
        }

        if ((horizontalInput != oldHinput) && (verticalInput != oldVInput)) {
            result += 1;
            //ADD TO BYTEBUFFER PROPER SYNTAX FOR INPUTS, SKIPPING FOR NOW!
            byteBuffer.Add(BitConverter.GetBytes(0));
        }
        else if (((horizontalInput != oldHinput) && (verticalInput == oldVInput)) || ((horizontalInput == oldHinput) && (verticalInput != oldVInput))) {
            //IM SO SORRY, this WILL be removed on actual implementation.
            //if one or the other
            result += 1;
            byteBuffer.Add(BitConverter.GetBytes(0));
        }

        return result;
    }

    void PlayerDataToBytes() {
        byteBuffer.Clear();
        byte[] packetHeader = CreatePlayerHeader();
        //byteBuffer.Add(packetHeader);
        int payloadSize = CalculatePayloadByteSize();
        byte[] finalPayload = new byte[payloadSize + packetHeader.Length];
        byteBuffer.Add(packetHeader); //add to the very left, since its the header
        print("prevheader!: " + packetHeader.Length);
        //turn all the values to byte arrays, combine them in order and copy them into the final payload.
        int index = 0;
        foreach (byte[] byteArray in byteBuffer) {
            for(int i = 0; i < byteArray.Length; i++) {
                finalPayload[index] = byteArray[i];
                index++;
            }
        }
        //"send packet" to binary reader
        print("Final!: " + finalPayload.Length);
        reader.incomingPacket = finalPayload;
    }

    byte[] CreatePlayerHeader() {
        int buffer = 0; //32 bit int
        int TEMPPLAYERID = 1;
        byte[] finalBytes = new byte[3];
        //basically, given the 32 bit int, bitwiseOR the playerID>>8 onto the buffer, convert int to byte array, and then recreate it
        //by ignoring the last? byte in the array.

        //for each part of binaryreader dictionary
        //do the appropriate thingy
        /*
        for(int i = 0; i < reader.playerPacket.Count; i++) {
            if (reader.playerPacket[0].type.Equals("Vector3")) {
                C
            }
        }
        */
        //LOAD THE BUFFER BACKWARDS SO IT FITS RIGHT, START WITH INPUTS, GO TO POSITION.
        //buffer = buffer << 2; //no input rn so just shift 0
        buffer += CompareRotations(oldRot, transform.rotation) << 4; //inputs use 4 bits so keep em at 0
        //print("headerrot: " + CompareRotations(oldRot, transform.rotation));
        //print("headervel: " + CompareVectors(oldVel, rb.velocity));
        //print("headerpos: " + CompareVectors(oldPos, transform.position));
        buffer += CompareVectors(oldVel, rb.velocity) << 8; 
        buffer += CompareVectors(oldPos, transform.position) << 11;
        buffer = (buffer | TEMPPLAYERID << 14); //place the id 
        byte[] temp = BitConverter.GetBytes(buffer);
        for (int i = 0; i < finalBytes.Length; i++) {
            finalBytes[i] = temp[i];
        }
        //print(BitConverter.ToInt32(temp,0));
        return finalBytes;
    }
    int CompareVectors(Vector3 vec1, Vector3 vec2) { //returns proper bitmask
        int result= 0;
        if(vec1.x != vec2.x) {
            result += 4; //100
        }
        if(vec1.y != vec2.y) {
            result += 2; //010
        }
        if(vec1.z != vec2.z) {
            result += 1; //001
        }
        return result;
    }

    int CompareRotations(Quaternion rot1, Quaternion rot2) { //returns proper bitmask
        int result = 0;
        if (rot1.x != rot2.x) {
            //print("X IS DIFF!"); //idk quaternions are weird
            result += 8; //1000
        }
        if (rot1.y != rot2.y) {
            result += 4; //0100
        }
        if (rot1.z != rot2.z) {
            result += 2; //0010
        }
        if (rot1.w != rot2.w) {
            result += 1; //0001
        }
        return result;
    }

}

class BinaryTypeAssigner {

    //int CompareVectors()
}