using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class BoardCtrl : MonoBehaviourPunCallbacks
{
    public static BoardCtrl instance;

    public GameObject block;

    public Transform boardPanelTr;
    
    private int[][] _gameBoard;
    private int _colorNum;

    private PhotonView pv;

    private GameObject[] blocks = new GameObject[361];

    private int[] pieceIds;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        pv = photonView;
    }

    void Start()
    {
        BoardSetting();
    }


    void BoardSetting()
    {
        GameObject newBlock;
        for (int i = 0; i < 361; i++)
        {
            newBlock = Instantiate(block, Vector3.zero, Quaternion.identity, boardPanelTr);
            newBlock.GetComponent<BlockCtrl>().id = i;

            blocks[i] = newBlock;
        }
    }

    public void BlockClicked(int blockId)
    {
        pv.RPC("PutPiece", RpcTarget.Others, blockId);
        CurrBoardCheck();
    }

    [PunRPC]
    void PutPiece(int blockId)
    {
        blocks[blockId].GetComponent<BlockCtrl>().PutPiece(GameManager.instance.oppoColor, false);
    }

#region BOARD_CHECK

    void CurrBoardCheck()
    {
        GameManager.instance.PrintBoardArray();
        _gameBoard = GameManager.instance.gameBoard;
        _colorNum = GameManager.instance.colorNum;
        // board check event (only check my color piece) 가로 세로 RLSlash LRSlash

        if(HorizontalCheck() || VerticalCheck() || LRSlashCheck() || RLSlashCheck())
        {
            WinEvent();
        }
    }

    bool HorizontalCheck()
    {
        pieceIds = new int[] {-1,-1,-1,-1,-1};
        int numOfPiece = 0;

        for (int i = 0; i < 19; i++)
        {
            for(int j = 0; j < 19; j++)
            {
                if(_gameBoard[i][j] == _colorNum)
                {
                    pieceIds[numOfPiece] = GetPos(j, i);
                    numOfPiece++;
                }
                else
                {
                    pieceIds = new int[] {-1,-1,-1,-1,-1};
                    numOfPiece = 0;
                }
                
                if(numOfPiece >= 5)
                {
                    return true;
                }
            }
        }
        return false;   
    }

    bool VerticalCheck()
    {
        pieceIds = new int[] {-1,-1,-1,-1,-1};
        int numOfPiece = 0;

        for (int i = 0; i < 19; i++)
        {
            for(int j = 0; j < 19; j++)
            {
                if(_gameBoard[j][i] == _colorNum)
                {
                    pieceIds[numOfPiece] = GetPos(j, i);
                    numOfPiece++;
                }
                else
                {
                    pieceIds = new int[] {-1,-1,-1,-1,-1};
                    numOfPiece = 0;
                }
                
                if(numOfPiece >= 5)
                {
                    return true;
                }
            }

        }
        return false;  
    }

    bool LRSlashCheck()
    {
        pieceIds = new int[] {-1,-1,-1,-1,-1};
        int numOfPiece = 0;
        int offset = 19;

        for (int i = 0; i < 15; i++)
        {
            for(int j = 0; j < offset; j++)
            {
                if(_gameBoard[i + j][j] == _colorNum)
                {
                    pieceIds[numOfPiece] = GetPos(j, i);
                    numOfPiece++;
                }
                else
                {
                    pieceIds = new int[] {-1,-1,-1,-1,-1};
                    numOfPiece = 0;
                }
                
                if(numOfPiece >= 5)
                {
                    return true;
                }
            }
            offset--;
        }
        pieceIds = new int[] {-1,-1,-1,-1,-1};
        numOfPiece = 0;
        offset = 19;


        for (int i = 0; i < 15; i++)
        {
            for(int j = 0; j < offset; j++)
            {
                if(_gameBoard[j][i + j] == _colorNum)
                {
                    pieceIds[numOfPiece] = GetPos(j, i);
                    numOfPiece++;
                }
                else
                {
                    pieceIds = new int[] {-1,-1,-1,-1,-1};
                    numOfPiece = 0;
                }
                
                if(numOfPiece >= 5)
                {
                    return true;
                }
            }
            offset--;
        }

        return false; 
    }

    bool RLSlashCheck()
    {
        pieceIds = new int[] {-1,-1,-1,-1,-1};
        int numOfPiece = 0;
        int offset = 19;

        for (int i = 18; i >= 4; i--)
        {
            for(int j = 0; j < offset; j++)
            {
                if(_gameBoard[i - j][j] == _colorNum)
                {
                    pieceIds[numOfPiece] = GetPos(j, i);
                    numOfPiece++;
                }
                else
                {
                    pieceIds = new int[] {-1,-1,-1,-1,-1};
                    numOfPiece = 0;
                }
                
                if(numOfPiece >= 5)
                {
                    return true;
                }
            }
            offset--;
        }
        pieceIds = new int[] {-1,-1,-1,-1,-1};
        numOfPiece = 0;
        offset = 0;


        for (int i = 0; i < 15; i++)
        {
            int tempI = i;
            for(int j = 18; j >= offset; j--)
            {
                if(_gameBoard[j][i++] == _colorNum)
                {
                    pieceIds[numOfPiece] = GetPos(j, i);
                    numOfPiece++;
                }
                else
                {
                    pieceIds = new int[] {-1,-1,-1,-1,-1};
                    numOfPiece = 0;
                }
                
                if(numOfPiece >= 5)
                {
                    return true;
                }
            }
            i = tempI;
            offset++;
        }


        return false;
    }

    int GetPos(int x, int y)
    {
        return y * 19 + x;
    }
#endregion

    void WinEvent()
    {
        GameManager.instance.isWin = true;
        ShowWinPieces((GameManager.instance.myColor));
    }

    void ShowWinPieces(string color)
    {
        for (int i = 0; i < pieceIds.Length; i++)
        {
            blocks[pieceIds[i]].GetComponent<Image>().sprite = Resources.Load<Sprite>($"{color}Lined");
        }
    }
}
