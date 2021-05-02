using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviourPunCallbacks
{
    public GameObject masterClient;
    public GameObject client;

    private PhotonView pv;
    // Start is called before the first frame update
    void Awake()
    {
        pv = photonView;
    }
    void Start()
    {
        pv.RPC("SetPlayersInfo", RpcTarget.OthersBuffered, PhotonNetwork.NickName);
    }


    [PunRPC]
    void SetPlayersInfo(string otherName)
    {
        Image masterImage = masterClient.transform.GetChild(0).gameObject.GetComponent<Image>();
        Image otherImage = client.transform.GetChild(0).gameObject.GetComponent<Image>();
        TMP_Text masterText = masterClient.transform.GetChild(1).gameObject.GetComponent<TMP_Text>();
        TMP_Text otherText = client.transform.GetChild(1).gameObject.GetComponent<TMP_Text>();

        if(PhotonNetwork.IsMasterClient)
        {
            masterImage.sprite = Resources.Load<Sprite>(GameManager.instance.myColor);
            otherImage.sprite = Resources.Load<Sprite>(GameManager.instance.oppoColor);

            masterText.text =  PhotonNetwork.NickName;
            otherText.text = otherName;
        }
        else
        {
            masterImage.sprite = Resources.Load<Sprite>(GameManager.instance.oppoColor); 
            otherImage.sprite = Resources.Load<Sprite>(GameManager.instance.myColor);

            masterText.text = otherName;
            otherText.text = PhotonNetwork.NickName;
        }
    }
}
