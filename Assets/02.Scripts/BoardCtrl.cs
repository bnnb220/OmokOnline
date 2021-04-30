using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class BoardCtrl : MonoBehaviourPunCallbacks
{
    public static BoardCtrl instance;

    public GameObject block;
    
    private int[,] _gameBoard;

    private PhotonView pv;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        pv = photonView;
    }

    void Start()
    {
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


    // void BoardSetting()
    // {
    //     GameObject newBlock;
    //     for (int i = 0; i < 361; i++)
    //     {
    //         newBlock = Instantiate(block, Vector3.zero, Quaternion.identity, boardPanelTr);
    //         newBlock.GetComponent<BlockCtrl>().id = i;
    //     }
    // }

    public void BlockClicked(BlockCtrl block)
    {
        pv.RPC("PutPiece", RpcTarget.All, block);
    }

    [PunRPC]
    void PutPiece(BlockCtrl block)
    {
        block.PutPiece();
    }
}
