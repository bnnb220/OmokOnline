using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class BoardCtrl : MonoBehaviourPunCallbacks
{
    public static BoardCtrl instance;

    public bool isGameOver;

    public GameObject block;

    public Transform boardPanelTr;
    
    private int[][] _gameBoard;
    private int _colorNum;

    private PhotonView pv;

    public GameObject[] blocks = new GameObject[361];

    private int[] pieceIds;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        pv = photonView;
    }

    void Start()
    {
        BlocksSetting();
    }


    void BlocksSetting()
    {
        GameObject newBlock;
        for (int i = 0; i < 361; i++)
        {
            newBlock = Instantiate(block, Vector3.zero, Quaternion.identity, boardPanelTr);
            newBlock.GetComponent<BlockCtrl>().id = i;

            blocks[i] = newBlock;
        }
    }

    public void BlocksReset()
    {
        foreach (GameObject block in blocks)
        {
            block.GetComponent<BlockCtrl>().BlockReset();
        }
    }

    public void BlockClicked(int blockId)
    {
        pv.RPC("PutPiece", RpcTarget.Others, blockId);
        CurrBoardCheck();
    }


#region PUNRPC

    [PunRPC]
    void PutPiece(int blockId)
    {
        blocks[blockId].GetComponent<BlockCtrl>().PutPiece(GameManager.instance.oppoColor, false);
    }

#endregion

#region GAME_END_CHECK

    void CurrBoardCheck()
    {
        GameManager.instance.PrintBoardArray();
        _gameBoard = GameManager.instance.gameBoard;
        _colorNum = GameManager.instance.colorNum;

        if(HorizontalCheck() || VerticalCheck() || LSlashCheck() || RSlashCheck())
        {
            GameManager.instance.GameEnd(pieceIds);
            this.isGameOver = false;
        }
        else
        {
            pv.RPC("ForbiddenPosSearch", RpcTarget.All);
        }
    }

    bool HorizontalCheck()
    {
        if(isGameOver) return true;
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
                    isGameOver = true;
                    return true;
                }
            }
        }
        return false;   
    }

    bool VerticalCheck()
    {
        if(isGameOver) return true;
        pieceIds = new int[] {-1,-1,-1,-1,-1};
        int numOfPiece = 0;

        for (int i = 0; i < 19; i++)
        {
            for(int j = 0; j < 19; j++)
            {
                if(_gameBoard[j][i] == _colorNum)
                {
                    pieceIds[numOfPiece] = GetPos(i, j);
                    numOfPiece++;
                }
                else
                {
                    pieceIds = new int[] {-1,-1,-1,-1,-1};
                    numOfPiece = 0;
                }
                
                if(numOfPiece >= 5)
                {
                    isGameOver = true;
                    return true;
                }
            }

        }
        return false;  
    }

    bool LSlashCheck()
    {
        if(isGameOver) return true;
        pieceIds = new int[] {-1,-1,-1,-1,-1};
        int numOfPiece = 0;
        int offset = 19;

        for (int i = 0; i < 15; i++)
        {
            for(int j = 0; j < offset; j++)
            {
                if(_gameBoard[i + j][j] == _colorNum)
                {
                    pieceIds[numOfPiece] = GetPos(j, i + j);
                    numOfPiece++;
                }
                else
                {
                    pieceIds = new int[] {-1,-1,-1,-1,-1};
                    numOfPiece = 0;
                }
                
                if(numOfPiece >= 5)
                {
                    isGameOver = true;
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
                    pieceIds[numOfPiece] = GetPos(i + j, j);
                    numOfPiece++;
                }
                else
                {
                    pieceIds = new int[] {-1,-1,-1,-1,-1};
                    numOfPiece = 0;
                }
                
                if(numOfPiece >= 5)
                {
                    isGameOver = true;
                    return true;
                }
            }
            offset--;
        }

        return false; 
    }

    bool RSlashCheck()
    {
        if(isGameOver) return true;
        pieceIds = new int[] {-1,-1,-1,-1,-1};
        int numOfPiece = 0;
        int offset = 19;

        for (int i = 18; i >= 4; i--)
        {
            for(int j = 0; j < offset; j++)
            {
                if(_gameBoard[i - j][j] == _colorNum)
                {
                    pieceIds[numOfPiece] = GetPos(j, i - j);
                    numOfPiece++;
                }
                else
                {
                    pieceIds = new int[] {-1,-1,-1,-1,-1};
                    numOfPiece = 0;
                }
                
                if(numOfPiece >= 5)
                {
                    isGameOver = true;
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

                if(_gameBoard[j][i] == _colorNum)
                {
                    pieceIds[numOfPiece] = GetPos(i, j);
                    numOfPiece++;    
                }
                else
                {
                    pieceIds = new int[] {-1,-1,-1,-1,-1};
                    numOfPiece = 0;
                }
                if(numOfPiece >= 5)
                {
                    isGameOver = true;
                    return true;
                }

                i++;
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


#region 33_44_CHECK

    // L, RU, U, LU, L, LD, D, RD
    private int[] dx = new int[8] {1, 1, 0,-1,-1,-1,0,1}; 
    private int[] dy = new int[8] {0,-1,-1,-1, 0, 1,1,1};

    // [3X3] : OX@XO, O@XXO, OXXO@O, OXO@XO
    private int[] dStart_33 = new int[5] {-2, -1, -4, -3, -1};
    private int[] dEnd_33 = new int[5] {2, 3, 1, 2, 4};

    // [4X4] : OXXX@, OX@XX, OXX@X, XXXO@, XXO@X
    private int[] dStart_44 = new int[5] {-3, -2, -4, -2, -3};
    private int[] dEnd_44 = new int[5] {2, 3, 0, 2, 1};

    private string[] pieceLine_33 = new string[5] {"EB@BE", "E@BBE", "EBBE@E", "EBE@BE", "E@BEBE"};

    private string[] pieceLine_44 = new string[8] {"EBB@BE","EB@BBE", "EBBB@", "EB@BB", "EBB@B", "BBBE@", "BBE@B", "BEB@B"};

    private int[][] boardCache33 = new int[19][];

    private int[][] boardCache44 = new int[19][];

    void ResetBoardCache()
    {
        boardCache33 = new int[19][];
        boardCache44 = new int[19][];

        for (int i = 0; i < 19; i++)
        {
            boardCache33[i] = new int[19];
            boardCache44[i] = new int[19];
        }
    }

    [PunRPC]
    void ForbiddenPosSearch()
    {
        if(GameManager.instance.myColor != "Black")
        {
            return;
        }

        ResetBoardCache();

        for (int y = 0; y < 19; y++)
        {
            for (int x = 0; x < 19; x++)
            {
                //현재 좌표가 Empty or ImposBlock 일 경우만 검사.
                if(this._gameBoard[y][x] < 0)
                {
                    if(BoardSearch33(x, y) || BoardSearch44(x, y))
                    {
                        this._gameBoard[y][x] = -2; // impossible position
                        this.blocks[PosToId(x,y)].GetComponent<BlockCtrl>().ForbiddenEvent(true);
                    }
                    else
                    {
                        this._gameBoard[y][x] = -1; // 만약 보드가 update되어 더이상 ImposBlock이 아닐 경우 empty로 변경.
                        this.blocks[PosToId(x,y)].GetComponent<BlockCtrl>().ForbiddenEvent(false);
                    }
                }
            }
        }
        UpdateGameBoard();
    }

    bool BoardSearch33(int x, int y)
    {
        int sum = 0;
        
        int skipNum = -1;

        for (int i = 0; i < 8; i++) //방향
        {
            if(i == skipNum)
            {
                continue;
            }
            for(int j = 0; j < dStart_33.Length; j++) //조합 개수
            {
                string curPieceLine = "";
                for(int k = dStart_33[j]; k <= dEnd_33[j]; k++)
                {
                    int newX = x + (this.dx[i]*k);
                    int newY = y + (this.dy[i]*k);
                    if(IsPieceInBoard(newX, newY))
                    {
                        if(k == 0)
                        {
                            curPieceLine += "@";
                        }
                        else{
                            curPieceLine += PieceValueToString(newX, newY);
                        }
                    }
                }

                if(IsPieceLineInList_33(curPieceLine))
                {
                    Debug.Log( $"x: {x}, y : {y} {curPieceLine} / j: {j}");
                    // 만약 조합이 OX@XO 이면 반대 방향도 같은 위치에 동일하게 체크되기 때문에 반대 방향 스킵.
                    curPieceLine = "";
                    if(j == 0)
                    {
                        string l = (PieceValueToString(x + (this.dx[i]*3), y + (this.dy[i]*3)));
                        string r = (PieceValueToString(x - (this.dx[i]*3), y - (this.dy[i]*3)));

                        if(l == "B" || r == "B")
                        {
                            Debug.Log("0x0x0  checekd");
                            break;
                        }   
                        skipNum = i + 4;
                    }
                    if(++sum >= 2)
                    {
                        Debug.Log("33 ");
                        return true; 
                    }
                    break; 
                }
                curPieceLine = "";
            }
        }
        return false;
    }

    bool BoardSearch44(int x, int y)
    {
        int sum = 0;
        
        int skipNum = -1;
        for (int i = 0; i < 8; i++) //방향
        {
            if(i == skipNum)
            {
                continue;
            }
            for(int j = 0; j < dStart_44.Length; j++) //조합개수
            {
                string curPieceLine = "";
                for(int k = dStart_44[j]; k <= dEnd_44[j]; k++)
                {
                    int newX = x + (this.dx[i]*k);
                    int newY = y + (this.dy[i]*k);
                    
                    //해당 좌표 valid 체크
                    if(IsPieceInBoard(newX, newY))
                    {
                        if(k == 0)
                        {
                            curPieceLine += "@";
                        }
                        else
                        {
                            curPieceLine += PieceValueToString(newX, newY);
                        }
                    }
                }

                if(IsPieceLineInList_44(curPieceLine))
                {
                    if(j <= 1)
                    {
                        skipNum = i + 4;
                    }
                    if(++sum >= 2) return true;   
                    curPieceLine = "";
                    break; 
                }
                curPieceLine = "";
            }
        }
        return false;
    }

    bool IsPieceInBoard(int x, int y)
    {
        if(x < 0 || x > 18 || y < 0 || y > 18)
        {
            return false;
        }
        return true;
    }

    string PieceValueToString(int x, int y)
    {
        if(!IsPieceInBoard(x,y))
        {
            return "F";
        }
        if(this._gameBoard[y][x] == 0)
        {
            return "W"; // White
        }
        else if(this._gameBoard[y][x] == 1)
        {
            return "B"; // Black
        }
        else
        {
            return "E";  // Empty
        }
    }

    bool IsPieceLineInList_33(string pieceLine)
    {
        for(int i = 0; i < this.pieceLine_33.Length; i++)
        {
            if(this.pieceLine_33[i] == pieceLine)
            {
                return true;
            }
        }
        return false;
    }

    bool IsPieceLineInList_44(string pieceLine)
    {
        for(int i = 0; i < this.pieceLine_44.Length; i++)
        {
            if(this.pieceLine_44[i] == pieceLine)
            {
                return true;
            }
        }
        return false;
    }

    void UpdateGameBoard()
    {
        GameManager.instance.gameBoard = this._gameBoard;
    }

    int PosToId(int x, int y)
    {
        return (y * 19) + x;
    }
#endregion
}
