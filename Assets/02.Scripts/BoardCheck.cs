using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardCheck : MonoBehaviour
{

    /*
    -2 : Empty but can't put.
    -1 : Empty.
     0 : White.
     1 : Black.
    */
    private int[,] _gameBoard;

    // Start is called before the first frame update
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
}
