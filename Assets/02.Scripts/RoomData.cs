using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class RoomData : MonoBehaviour
{
    public TMP_Text roomInfoText;
    public RoomInfo _roomInfo;
    private bool readyToJoin;

    public RoomInfo RoomInfo

    {
        get
        {
            return _roomInfo;
        }
        set
        {
            this._roomInfo = value;
            roomInfoText.text = $"{this._roomInfo.Name}   ({this._roomInfo.PlayerCount} / {this._roomInfo.MaxPlayers})";

            GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => OnEnterRoom(this._roomInfo.Name));

        }
    }

    void OnEnterRoom(string roomName) 
    {
        if(PhotonManager.instance.readyToJoin)
        {
            PhotonNetwork.JoinRoom(roomName);
        }
    }
}
