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

    public Image masterImage;
    public Image otherImage;
    public TMP_Text masterText;
    public TMP_Text otherText;

    public Button restartButton;

    private PhotonView pv;

    public static UIManager instance;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        pv = photonView;
    }
    void Start()
    {
        otherImage = client.transform.GetChild(0).gameObject.GetComponent<Image>();
        otherText = client.transform.GetChild(1).gameObject.GetComponent<TMP_Text>();
        masterImage = masterClient.transform.GetChild(0).gameObject.GetComponent<Image>();
        masterText = masterClient.transform.GetChild(1).gameObject.GetComponent<TMP_Text>();

        restartButton.GetComponent<Image>().color = Color.gray;
        InfoSetting();
    }


    [PunRPC]
    void SetPlayersInfo(string otherName)
    {
        otherImage.sprite = Resources.Load<Sprite>(GameManager.instance.oppoColor);
        otherImage.color = Color.white;
        otherText.text = otherName;
    }

    void MyInfo()
    {
        masterImage.sprite = Resources.Load<Sprite>(GameManager.instance.myColor);
        masterImage.color = Color.white;
        masterText.text = PhotonNetwork.NickName;
    }

    void InfoSetting()
    {
        MyInfo();
        pv.RPC("SetPlayersInfo", RpcTarget.OthersBuffered, PhotonNetwork.NickName);
    }

    public void OtherPlayerExit()
    {
        otherImage.sprite = null;
        otherImage.color = new Color(0,0,0,0);
        otherText.text = "";
        Restart();
        MyInfo();
    }

    public void OnRestartButtonClick()
    {
        if(GameManager.instance.isGameEnd)
        {
            pv.RPC("Restart", RpcTarget.All);
        }
    }

    [PunRPC]
    void Restart()
    {
        restartButton.GetComponent<Image>().color = Color.gray;
        GameManager.instance.GameRestart();
    }
}
