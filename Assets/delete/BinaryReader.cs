using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

class NewVar {

    public NewVar(float theVar, short theBitSize, string theName) {
        newVar = theVar;
        bitSize = theBitSize;
        name = theName;

    }
    public short bitSize;
    public string name;
    public bool isNew = false;
    public float newVar;
    
    const float UNCHANGED = 999999;


    private Vector3 _newVec = new Vector3(UNCHANGED, UNCHANGED, UNCHANGED);
    public Vector3 newVec {
        get { return _newVec; }
        set { _newVec = value; }
    }
    /*
    public float newVar {
        get { return _newVar; }
        set { _newVar = value; }
    }*/
    public Quaternion GetType(Quaternion? unused) {
        return new Quaternion();
    }
    public Vector3 GetType(Vector3? unused) {
        return new Vector3();
    }
}


public class BinaryReader : MonoBehaviour {

    short vecEmpty = Convert.ToInt16("000", 2);
    short vecX = Convert.ToInt16("100", 2);
    short vecY = Convert.ToInt16("010", 2);
    short vecZ = Convert.ToInt16("001", 2);
    short vecXY = Convert.ToInt16("110", 2);
    short vecXZ = Convert.ToInt16("101", 2);
    short vecYZ = Convert.ToInt16("011", 2);
    short vecXYZ = Convert.ToInt16("111", 2);

    short posPos = Convert.ToInt16("11100000000000", 2);
    short velPos = Convert.ToInt16("00011100000000", 2);
    short rotPos = Convert.ToInt16("00000011110000", 2);
    short hInputPos = Convert.ToInt16("00000000001100", 2);
    short vInputPos = Convert.ToInt16("00000000000011", 2);


    const float UNCHANGED = 999999; //999999 aka 6 9s will denote NONE


    //static NewVar newPosX = new NewVar(UNCHANGED);
    /*
    float newPosX = UNCHANGED;
    float newPosY = UNCHANGED;
    float newPosZ = UNCHANGED;

    float newVelX = UNCHANGED;
    float newVelY = UNCHANGED;
    float newVelZ = UNCHANGED;

    float newRotX = UNCHANGED;
    float newRotY = UNCHANGED;
    float newRotZ = UNCHANGED;
    float newRotW = UNCHANGED;

    float newHInput = UNCHANGED;
    float newVInput = UNCHANGED;
    /// <summary>
    /// //
    /// 
    /// </summary>

    float oldPosX = UNCHANGED;
    float oldPosY = UNCHANGED;
    float oldPosZ = UNCHANGED;

    float oldVelX = UNCHANGED;
    float oldVelY = UNCHANGED;
    float oldVelZ = UNCHANGED;

    float oldRotX = UNCHANGED;
    float oldRotY = UNCHANGED;
    float oldRotZ = UNCHANGED;
    float oldRotW = UNCHANGED;

    float oldHInput = UNCHANGED;
    float oldVInput = UNCHANGED;
    */

    /// <summary>
    /// 
    /// </summary>

    //static Vector3 newPos = new Vector3();
    static NewVar test = new NewVar(UNCHANGED, 32, "newPosX");
    //static Quaternion empty = new Quaternion();
    Quaternion whatevert = test.GetType(new Quaternion()); //gets quaternion, can also get vector
    /*
    Dictionary<int, NewVar> newVars = new Dictionary<int, NewVar> { //CHANGE INTO JUST TRANSFORM, ROTATION, WHATYEVER. USE BIT SHIFTS 3 TIMES CHECKING X Y AND Z
        // OR CHECKING X Y Z AND W wait no that doesnt work because of how dynamic the system is, and sporadic everything is. i would have to store a seperate database
        // of all the types, so like every iteration has a CheckValueAtBitPosition which has a database it queries to check how it should check and modify data.
        {0, new NewVar(UNCHANGED,32,"newPosX")}, //should work?
        {1, new NewVar(UNCHANGED,32,"newPosY")},
        {2, new NewVar(UNCHANGED,32,"newPosX")},

        {3, new NewVar(UNCHANGED,32,"newVelX")},
        {4, new NewVar(UNCHANGED,32,"newVelY")},
        {5, new NewVar(UNCHANGED,32,"newVelZ")},

        {6, new NewVar(UNCHANGED,32,"newRotX")},
        {7, new NewVar(UNCHANGED,32,"newRotY")},
        {8, new NewVar(UNCHANGED,32,"newRotZ")},
        {9, new NewVar(UNCHANGED,32,"newRotW")},

        {10, new NewVar(UNCHANGED,2,"newInputH-")},
        {11, new NewVar(UNCHANGED,2,"newInputH+")},

        {12, new NewVar(UNCHANGED,2,"newInputV-")},
        {13, new NewVar(UNCHANGED,2,"newInputV+")},
    };
    */

    // Use this for initialization

    public byte[] payload1;
    public byte[] header1;
    void Start () {
        //print(newVars[0].newVar);
        //newVars[0].newVar = 420f;
        //print(newVars[0].newVar);
        //newPosX.newVar = 420f;
        int testInput = Convert.ToInt32("000000000101000101101100", 2); //24 bits long, see sketchpad.\
        print("this should be 1: " + (testInput>>14 & Convert.ToInt32("1111111111", 2)));
        byte[] ass = new byte[3];
        ass[0] = 0;
        ass[1] = 0;
        ass[2] = 1;
        header1 = BitConverter.GetBytes(Convert.ToInt32("000000000101000101100110", 2)); //char+char+char into string to send
                                                                                                //predict byte array beforehand, set bytearray size, actively add onto it with each variable
                                                                                                //can predict with the method used to create the payload, for now hard code it

        //string payload1String = "";
        //foreach (char a in payload1String.ToCharArray()) {
        //connvert.toByte or something
        //}

        //FUCK I FORGOT BYTES ARE READ RIGHT TO LEFT FUCK ME...

        payload1 = new byte[17];
        payload1[0] = BitConverter.GetBytes(10f)[0];
        payload1[1] = BitConverter.GetBytes(10f)[1];
        payload1[2] = BitConverter.GetBytes(10f)[2]; // y pos
        payload1[3] = BitConverter.GetBytes(10f)[3];

        payload1[4] = BitConverter.GetBytes(2f)[0];
        payload1[5] = BitConverter.GetBytes(2f)[1];
        payload1[6] = BitConverter.GetBytes(2f)[2]; // z vel
        payload1[7] = BitConverter.GetBytes(2f)[3];

        payload1[8] = BitConverter.GetBytes(200f)[0];
        payload1[9] = BitConverter.GetBytes(200f)[1];
        payload1[10] = BitConverter.GetBytes(200f)[2]; //y rot
        payload1[11] = BitConverter.GetBytes(200f)[3];

        payload1[12] = BitConverter.GetBytes(90f)[0];
        payload1[13] = BitConverter.GetBytes(90f)[1]; //z rot
        payload1[14] = BitConverter.GetBytes(90f)[2];
        payload1[15] = BitConverter.GetBytes(90f)[3];

        payload1[16] = Convert.ToByte('6'); //2^4 =, although only possible positions are: 00000,0001,0010,0100,1000,1001,1010,01010,0110 so 9 positions, 3^2 or 3x3
        //hardcoded packet paylod of Y pos change, Z vel change,  YZ rotation change, and right+up keys are being pressed.

        //Encoding.Unicode.GetBytes("aa"); //BitConverter.GetBytes('a' + 'b'); //c# has each char as 2 bytes
        //print(payload1.Length);
        //BReadPacketID(header1, payload1);
        ReadNetworkPacket(header1, payload1);

        /*
        print("TESTING CONVERSION:");
        print(Convert.ToInt32("000000000010000000000000", 2)<<10);
        print((Convert.ToInt32("000000000010000000000000", 2)<<10)>>23);
        print((Convert.ToUInt32("000000000010000000000000", 2) << 18) >> 31);
        print((Convert.ToUInt32("000000000101000101100110", 2) << 18) >> 31);
        print((Convert.ToUInt32("000000000101000101100110", 2) << 19) >> 31);
        print((Convert.ToUInt32("000000000101000101100110", 2) << 20) >> 31);
        print((Convert.ToUInt32("000000000101000101100110", 2) << 21) >> 31);*/





        //print("this should be 1: " + (Convert.ToInt32("0000000000100000000000000", 2) & Convert.ToInt32("111111111100000000000000", 2)));

    }

    void ReadPacketID(int packet) {
        int packetID = (packet>>14 & Convert.ToInt16("1111111111", 2)); //first 10 bits, shift the other bits away.
        if(packetID>=0 && packetID <= 15) { //if its a player
            ReadNetworkedPlayer(packet<<10, packetID);
        }
        //if ((packet & Convert.ToInt16("1111111111", 2)) =) {

        //}
    }

    /*
    void BReadPacketID(byte[] header, byte[] payload) {
        int packetID = BitConverter.ToInt32(header,0) >> 14; //header to int, then bitshift?
        //print("pakid: " + packetID);
        if (packetID >= 0 && packetID <= 15) { //if its a player
            print("Processing packet for player: " + packetID);
            BReadNetworkedPlayer(header, payload, packetID);
        }
        //var test = new Vector3();
        //print(packetID);
        //if ((packet & Convert.ToInt16("1111111111", 2)) =) {

        //}
        Vector3? meme = null;
        if (meme.Equals(null)) {

        }
    }

    //is there a way to have a generic encapsulate a vec3, rotation, a boolean, and a short, all in one go? basically just say vec3.x=newVar, vec3.y=nmewVar
    //basically iterate through the values of each parameter, and assign those values to the new vector or new rotation.
    //use vars? that var thing worked... idfk

    
    void BReadNetworkedPlayer(byte[] header, byte[] payload, int playerID) { //packet now has size 14 bits
        int headerParams = (BitConverter.ToInt32(header, 0) << 18) >> 18; //clear packet id from byte -- dont have to clear past 10 bits left because the padding (overflow?), no need to push it away
        // 10 for size + 8 bits padding
        for(int i=0; i<=13; i++) { //14 is header size
            int temp = (headerParams << (18 + i))>>31; //isolate bit
            //print(temp);
            if (temp == -1) { //cuz its signed its negative, whatever lol
                newVars[i].isNew = true;
                //SET NEWVAR VALUE BASED ON PAYLOAD, AND KEEP TRACK OF ALREADY READ BYTES. OR DO IT AFTER THE LOOP, BUT THIS IS MORE EFFICIENT?
            }
        }
        AssignNewNetworkVars();
        //we know packetsize is 14, so lets iterate through every bit and have a unique result?
        //use % to check what number? like 111 is 7
    }

    void AssignNewNetworkVars() {
        for(int i = 0; i < newVars.Count; i++) {
            if (newVars[i].isNew) {
                print("new value of " + newVars[i].name + " is: " + newVars[i].newVar);
                //assign values now
            }
        }
        //newVars[index].newVar = 
    }
    */

    void ReadNetworkedPlayer(int packet, int playerID) { //packet now has size 14 bits
        /*
        int posData = packet >> 11;
        int velData = (packet << 3) >> 11;
        int rotData = (packet << 6) >> 10;
        int hInputData = (packet << 10) >> 12;
        int vInputData = (packet << 12) >> 12; //could also do & with its bit position? thats easier to understand lol ok redo
        */
        int posData = packet & posPos;
        int velData = packet & velPos;
        int rotData = packet & rotPos;
        int hInputData = packet & hInputPos;
        int vInputData = packet & vInputPos;

        

    }
    public bool enablePacketSpam = false;

    public byte[] incomingPacket;
    // Update is called once per frame
    void Update() {
        if (enablePacketSpam) {
            byte[] theHeader = new byte[4];
            byte[] thePayload = new byte[incomingPacket.Length - 3];
            for(int i = 0; i < 3; i++) {
                theHeader[i] = incomingPacket[incomingPacket.Length - 3 + i]; //-1 cuz length
            }
            for(int i = 0; i < incomingPacket.Length - 3; i++) { //if size 20, i goes from 0 to 16.
                thePayload[i] = incomingPacket[i]; //skip the ends cuz thats the header
            }
            theHeader[3] = 0; //pad it, tried doing conversions but it fucked
            //print("theheader!: " + theHeader.Length);
            //print("theheader: " + theHeader[0] + " " + theHeader[1] + " " + theHeader[2]);
            ReadNetworkPacket(theHeader, thePayload);
        }
    }

    //MAKE DICTIONARY OF OBJECTS THAT ARE READ IN ORDER IN THE PACKET
    //ASSIGN START INDEXES ACCORDINGLY. PINDEX IS ASSIGNED USING THE ONE BEFORE IT (INDEX 0 STARTS AT 0), TO ALLOW FOR CUSTOM XYZ VALUES.


     public Dictionary<int, BinaryPacketDecoder> playerPacket = new Dictionary<int, BinaryPacketDecoder> {
        { 0, new BinaryPacketDecoder(0,"Vector3")}, //Position
        { 1, new BinaryPacketDecoder(3,"Vector3")} //Velocity
        //{ 2, new BinaryPacketDecoder(6,"Quaternion")}, //Rotation
        //{ 3, new BinaryPacketDecoder(10,"float")} //input, goes to 14.

    };

    public void ReadNetworkPacket(byte[] header, byte[] payload) { //header is 4 bytes, payload is X bytes
        int packetID = BitConverter.ToInt32(header, 0) >> 14; //header to int, then bitshift?
        if (packetID >= 0 && packetID <= 15) { //if its a player
            //print("Processing packet for player: " + packetID);
            DecodeNetworkPlayer(header, payload, packetID);
        }
    }

    //yep, seems to work! vector is properly modified and everything.
    public void DecodeNetworkPlayer(byte[] header, byte[] payload, int playerID) {
        int headerParams = (BitConverter.ToInt32(header, 0) << 18) >> 18; // 10 to remove id, 8 for extra byte.
        //int headerParams = (BitConverter.ToInt16(header, 0) << 2) >> 2;
        for (int i=0; i < playerPacket.Count; i++) {
            if (i == 0) {
                //print("B$!!!!!!!!!!!!!!!!!!!!!!!!!" +playerPacket[i].aVector);
                playerPacket[i].CheckAndAssignValues(headerParams, payload);
                //print("AFTR!!!!!!!!!!!!!!!!!!!!!!" + playerPacket[i].aVector);
                //print("Final vector pos!: " + playerPacket[i].finalVector.y); //make print statement, toString, which prints the result depending on what datatype.
                //print(playerPacket[i].errorLog); //cont here, fag. 7-8-18
            } else {
                playerPacket[i].PIndex = playerPacket[i - 1].PIndex;
                playerPacket[i].CheckAndAssignValues(headerParams,payload);
                //print("Final vector vel!: " + playerPacket[i].finalVector); //make print statement, toString, which prints the result depending on what datatype.
                //print("Final rotation!: " + playerPacket[i].finalQuaternion);
                //print(playerPacket[i].errorLog);
            }
        }
        for (int i = 0; i < playerPacket.Count; i++) {
            playerPacket[i].Reset();
        }
    }
}

// will be intialized as a static vector inside a dictionary here. the main code will iterate through each part of that dictionary (for transforms only) and ask this class to do the dirty work.
public class BinaryPacketDecoder {

    public BinaryPacketDecoder(int theHIndex, string input) {
        //PIndex = thePIndex; //WHICH BIT THE PAYLOAD IS ON! USE %8
        //endPIndex = theEndPIndex;

        startIndex = theHIndex; //startIndex = 0 is origin. =1 is 1 after origin, and so on until 13, the final bit of the 14 bit header. subject to change.
        //endIndex = theEndIndex;
        //resultPIndex = PIndex;
        type = input;
        EstablishType(input);
    }
    public string type;

    public int PIndex = 0; public int startIndex = 0;//, endIndex; //payload start index, start and end of header.
    //  -> -> -> -> index goes LEFT TO RIGHT! -> -> -> -> 
    //WARNING: ALWAYS ADD PADDED SHIT AT THE END. DO NOT FUCK WITH VECTORS OR ANY CONSISTENT BYTE UNITS UNTIL THE END OF THE PACKET.

    //public int resultPIndex;

    const float UNCHANGED = 999999; //999999 aka 6 9s will denote NONE


    public Vector3? aVector = null;
    public Vector3 finalVector = new Vector3(UNCHANGED,UNCHANGED,UNCHANGED);

    Quaternion? aQuaternion = null;
    public Quaternion finalQuaternion = new Quaternion();

    float? aFloat = null;
    float finalFloat = new float();

    public string errorLog = "ErrorLog: ";
    //by expanding this list, you scale the capabilities of packet/payload decoding in one place. so long as you properly assign index.

    public void EstablishType(string input) { //how to do generic shit so i dont have to copy paste if statements fuck
        if (input.Equals("Vector3")) {
            aVector = new Vector3();
            //finalVector.x = 420f;
        }
        else if (input.Equals("Quaternion")) {
            aQuaternion = new Quaternion();
        }
        else if (input.Equals("float")) {
            aFloat = new float();
        }
        else {
            Debug.Log("Critical Network Error: EstablishType failed! Check your grammar, idiot!");
        }
    }

    public void Reset() {
        aVector = null;
        aQuaternion = null;
        aFloat = null;
        finalVector = new Vector3(UNCHANGED, UNCHANGED, UNCHANGED);
        finalQuaternion = new Quaternion();
        PIndex = 0;
        //startIndex = 0;//, endIndex; //payload start index, start and end of header.
        errorLog = "ErrorLog: ";
        EstablishType(type);
    }

    //checkbits: usaed for header diagnosis, should be already placed all the way right.
    public void CheckAndAssignValues(int header, byte[] payload) {
        if (aVector != null) { //use the null values as checks to see what object we are working with
            //errorLog += "StartingBitshift! pindex is: " + PIndex + " ";
            //ushort shortHeader = BitConverter.ToUInt16(header, 0);
            //ushort temp1 = Convert.ToUInt16("0000000000000001", 2);
            //ushort temp2 = Convert.ToUInt16("0000000000000010", 2);
            //errorLog += "quicktest - b4: " + (shortHeader) + " aftr: " + ((shortHeader <<2)>>15) + " header length: " + header.Length;
            //errorLog += "quicktest2 - bitmask 1: " + (shortHeader & temp1) + " bitmask 2: " + (shortHeader &  temp2) + ". ";
            //errorLog += "first shiort: " + ((header << (16+2 + startIndex + 0)) >> (15+16)) + " ";
            //errorLog += "2nd shiort: " + ((header << (16 + 2 + startIndex + 1)) >> (15 + 16)) + " ";
            //errorLog += "3rd shiort: " + ((header << (16 + 2 + startIndex + 2)) >> (15 + 16)) + " ";

            //header size should be 2 bytes. its basically the 14 of the header, and 2 padding making it 16 bits
            if (((header << (16 + 2 + startIndex + 0)) >> (15 + 16)) == -1) { //shift it to the end, and then more depending on the index to wipe out extra numbers. then, shift all the way to the right to isolate the 1 bit.
                                                                      //basically, if the resulting first value (x) is enabled, then we set it.
                //int leftoverBits; //the bits to shift after the byte array. almost always will be 0, but ya never know what else ill throw in.
                int index = PIndex / 8;//Math.DivRem(PIndex,8,out leftoverBits); //
                float payloadVal = BitConverter.ToSingle(payload, index); //convert payload at index to float
                //errorLog += "payloadVal is: " + payloadVal + " ";
                //WARNING: ALWAYS ADD PADDED SHIT AT THE END. DO NOT FUCK WITH VECTORS OR ANY CONSISTENT BYTE UNITS UNTIL THE END OF THE PACKET.
                finalVector.x = payloadVal;
                PIndex += 32;
                //ushort shortPayload = Convert.ToUInt16(payload[index]) << (8+leftoverBits);
                //aVector.Value.x = //calculate which byte to start on based on PIndex, which is taken from previous iterations of usage of the payload.
                //BitConverter.ToSingle(payload[PIndex % 8], 0);
            }
            if (((header << (16 + 3 + startIndex + 0)) >> (15 + 16)) == -1) {
                int index = PIndex / 8;
                float payloadVal = BitConverter.ToSingle(payload, index); // 
                //errorLog += "payloadVal is: " + payloadVal + " ";
                finalVector.y = payloadVal;
                //errorLog += "finalvector.y is: " + finalVector.y + " ";
                PIndex += 32;
            }
            if (((header << (16 + 4 + startIndex + 0)) >> (15 + 16)) == -1) {
                int index = PIndex / 8; // todo maybe: Math.DivRem(PIndex,8,out leftoverBits);
                //errorLog += "index for payload is: " + index + " ";
                float payloadVal = BitConverter.ToSingle(payload, index);
                //errorLog += "payloadVal is: " + payloadVal + " ";
                finalVector.z = payloadVal;
                PIndex += 32;
            }
        }
        if (aQuaternion != null) {

        }
    }

}

/*
 * (code seems efficient enough, doesnt seem to cause any hiccups.)
 * ok so TODO: add rotations and inputs. and add a check for after reading player info, it reads other info in order
 * like spell information, stats, if enemy was hit, etc
 * but wouldnt i have to increase the size of the header?
 * no, i could just add to the payload at will.
 * but how would i differentiate?
 * 
 * shit
 * 
 * maybe add a header onto the payload?
 * like right after the movement info is done, if there is an extra byte (or X amount of bytes depending on header), 
 * then we know there is some extra data to handle along with the movement. since we always need to sync playerpos, we have
 * that header always included, which works totally fine.
 * 
 * yeah no this is fine, i just have to add another 'reading' function to read and decode extra payload data.
 * OR instead of having something like that as a 'player' packet, i have it as its own set of variables
 * (as seen in the header, 0-15,16-x,x-y whatever. i can throw in 2^10, so i can fit extra information like spellcast, enemy hit, whatever)
 * (no need for id in header, connectionID is already given. whatever lol forget this line)
 * 
 * to do that id need to create a spell listing ID system and shit, wew networking games is some hard fucking shit
 * but im pretty content with what ive made so far. ill drop this for now (just wanted to PoC it), and start working on
 * the enemy fighting stuff. maybe some modeling and animating? or music making?
*/