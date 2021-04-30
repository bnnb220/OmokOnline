using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance;
    public bool isMyTurn;
    public string myColor;
    public string oppoColor;

    public int[,] gameBoard; 

    private PhotonView pv;

    public GameObject[] blocks;

    void Awake()
    {
        instance = this;
        pv = photonView;
    }
    // Start is called before the first frame update
    void Start()
    {
        CreateBoardArray();
        ColorSetting();
    }

    void CreateBoardArray()
    {
        gameBoard = new int[19,19];

        for (int i = 0; i < 19; i++)
        {
            for (int j = 0; j < 19; j++)
            {
                gameBoard[i, j] = -1;
            }
        }
    }

    void ColorSetting()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            myColor = "Black";
            oppoColor = "White";
            isMyTurn = true;
        }
        else
        {
            myColor = "White";
            oppoColor = "Black";
            isMyTurn = false;
        }
    }

}
