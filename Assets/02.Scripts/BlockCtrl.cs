using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class BlockCtrl : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image image;
    private PhotonView pv;

    public int id;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(GameManager.instance.isMyTurn)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0.5f);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {   
        if(GameManager.instance.isMyTurn)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0.0f);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        GetComponent<Button>().onClick.AddListener(() => OnBlockClick());
    }

    
    void OnBlockClick()
    {
        if(GameManager.instance.isMyTurn)
        {   
            PutPiece(GameManager.instance.myColor, true);
            BoardCtrl.instance.BlockClicked(id);
        }
    }

    public void PutPiece(string color, bool isCalledFromMine)
    {
        Sprite img = Resources.Load<Sprite>(color);

        image.sprite = img;

        image.color = Color.white;

        if(isCalledFromMine)
        {
            GameManager.instance.isMyTurn = false;
        }
        else
        {
            GameManager.instance.isMyTurn = true;
        }
        BoardArrayUpdate(color);

        GetComponent<BlockCtrl>().enabled = false;
        GetComponent<Button>().enabled = false;
    }

    void BoardArrayUpdate(string color)
    {
        int colorNum = -1;
        if(color == "White")
        {
            colorNum = 0;
        }
        else if(color == "Black")
        {
            colorNum = 1;
        }
        else
        {
            Debug.Log("ERROR");
        }
        int x = id % 19;
        int y = (int)Mathf.Floor(id / 19);

        GameManager.instance.gameBoard[y][x] = colorNum;
    }
}
