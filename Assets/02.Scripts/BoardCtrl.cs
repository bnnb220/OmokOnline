using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class BoardCtrl : MonoBehaviourPunCallbacks
{
    public static BoardCtrl instance;

    public GameObject block;

    public Transform boardPanelTr;
    
    private int[,] _gameBoard;

    private PhotonView pv;

    private GameObject[] blocks = new GameObject[361];

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

    // Update is called once per frame
    void Update()
    {
        
    }

    void CurrBoardCheck()
    {
        _gameBoard = GameManager.instance.gameBoard;

        // board check event (only check my color piece) 가로 세로 RLSlash LRSlash
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
    }

    [PunRPC]
    void PutPiece(int blockId)
    {
        blocks[blockId].GetComponent<BlockCtrl>().PutPiece(GameManager.instance.oppoColor, false);
    }
}
