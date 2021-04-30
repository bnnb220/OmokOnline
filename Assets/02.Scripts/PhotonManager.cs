using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    private readonly string gameVersion = "v1.0";
    private string userId = "Yongjung";

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        PhotonNetwork.GameVersion = gameVersion;

        PhotonNetwork.NickName = userId;

        PhotonNetwork.ConnectUsingSettings();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("1. Connected Server");

        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("2. connected to Lobby");

        PhotonNetwork.JoinRandomRoom();
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
}
