using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance;
    public bool isWin;
    public bool isMyTurn;
    public string myColor;
    public string oppoColor;
    public int colorNum;

    public int[][] gameBoard; 

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
        gameBoard = new int[19][];

        for (int i = 0; i < 19; i++)
        {
            int[] row = new int[19];
            for (int j = 0; j < 19; j++)
            {
                row[j] = -1;
            }
            gameBoard[i] = row;
        }
    }

    void ColorSetting()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            myColor = "Black";
            oppoColor = "White";
            colorNum = 1;
            isMyTurn = true;
        }
        else
        {
            myColor = "White";
            oppoColor = "Black";
            colorNum = 0;
            isMyTurn = false;
        }
    }

    public void PrintBoardArray()
    {
        string board = "";

        for (int i = 0; i < gameBoard.Length; i++)
        {
            string line = "";
            for (int j = 0; j < gameBoard[0].Length; j++)
            {
                line += gameBoard[i][j] + " ";
            }
            board += line + "\n";
        }

        Debug.Log(board);
    }
}
