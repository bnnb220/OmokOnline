using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class GameManager : MonoBehaviour
{
    
    public static GameManager instance;
    public bool isMyTurn;
    public Color myColor;

    /*
    -2 : Empty but can't put.
    -1 : Empty.
     0 : White.
     1 : Black.
    */
    public int[][] gameBoard; 

    public enum Color{White, Black}

    private PhotonView pv;


    void Awake()
    {
        instance = this;
        pv = GetComponent<PhotonView>();
    }
    // Start is called before the first frame update
    void Start()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            myColor = Color.Black;
        }
        else
        {
            myColor = Color.White;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
