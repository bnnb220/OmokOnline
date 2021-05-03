using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PhotonManager : MonoBehaviourPunCallbacks
{

    private readonly string gameVersion = "v1.0";
    private string userId = "Yongjung";

    public TMP_InputField userIdText;

    public TMP_InputField roomNameText;
    

    public GameObject roomPref;
    public GameObject roomContents;

    private Dictionary<string, GameObject> roomDict = new Dictionary<string, GameObject>();


    public bool readyToJoin;
    

    public static PhotonManager instance;

    void Awake()
    {
        instance = this;

        PhotonNetwork.AutomaticallySyncScene = true;

        PhotonNetwork.GameVersion = gameVersion;

        PhotonNetwork.NickName = userId;

       // PhotonNetwork.Disconnect();

        PhotonNetwork.ConnectUsingSettings();

        

        Debug.Log("Awake");
    }
    // Start is called before the first frame update

    
    void Start()
    {
        userId = PlayerPrefs.GetString("userId", $"USER_{Random.Range(0, 100):00}");
        userIdText.text = userId;
        PhotonNetwork.NickName = userId;

    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("1. Connected Server");

        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("2. connected to Lobby");
        
        readyToJoin = true;
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("3. join random room fail");

        PhotonNetwork.CreateRoom("NewRoom");
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("4. New Room Created.");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("5. Room joined.");

        if(PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("InGame");
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        GameObject tempRoom;

        foreach(var room in roomList)
        {
            if(room.RemovedFromList)
            {
                roomDict.TryGetValue(room.Name, out tempRoom);

                Destroy(tempRoom);

                roomDict.Remove(room.Name);
            }

            else
            {
                if(!roomDict.ContainsKey(room.Name))
                {
                    GameObject _room = Instantiate(roomPref, roomContents.transform);

                    _room.GetComponent<RoomData>().RoomInfo = room;
                    roomDict.Add(room.Name, _room);
                }

                else
                {
                    roomDict.TryGetValue(room.Name, out tempRoom);
                    tempRoom.GetComponent<RoomData>().RoomInfo = room;

                }
            }
        }
    }

    public void OnRoomCreateBtnClk()
    {
        if(!readyToJoin)
        {
            return;
        }
        PlayerPrefs.SetString("userId", this.userIdText.text);
        PhotonNetwork.NickName = this.userIdText.text;

        RoomOptions ro = new RoomOptions();
        ro.IsOpen = true;
        ro.IsVisible = true;
        ro.MaxPlayers = 2;

        if(string.IsNullOrEmpty(roomNameText.text))
        {
            roomNameText.text = $"ROOM_{Random.Range(0,100):00}";
        }
        PhotonNetwork.CreateRoom(roomNameText.text, ro);
    }
}
