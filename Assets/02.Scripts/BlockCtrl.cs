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

    public int x;
    public int y;

    private bool isForbidden;

    private Color originColor;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        GetComponent<Button>().onClick.AddListener(() => OnBlockClick());

        x = id % 19;
        y = (int)Mathf.Floor(id / 19);

        originColor = image.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(PosValidCheck())
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0.5f);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {   
        if(PosValidCheck())
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0.0f);
        }
    }

    bool PosValidCheck()
    {
        if(GameManager.instance.gameBoard[this.y][this.x] == -2)
        {
            return false;
        }
        if(!GameManager.instance.isMyTurn)
        {
            return false;
        }
        return true;
    }
    
    public void ForbiddenEvent(bool isForbidden)
    {
        if(isForbidden)
        {
            Sprite img = Resources.Load<Sprite>("RedPiece");
            this.image.sprite = img;
            this.image.color = Color.white;
        }
        else
        {
            this.image.sprite = null;
            this.image.color = this.originColor;
        }
    }
    void OnBlockClick()
    {
        if(PosValidCheck())
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
        GameManager.instance.gameBoard[this.y][this.x] = colorNum;
    }
}
