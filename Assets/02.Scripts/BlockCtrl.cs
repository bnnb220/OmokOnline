using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class BlockCtrl : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool enable = true;
    private Image image;
    private PhotonView pv;

    public int id;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(enable)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0.5f);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {   
        if(enable)
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

        enable = false;

        if(isCalledFromMine)
        {
            GameManager.instance.isMyTurn = false;
        }
        else
        {
            GameManager.instance.isMyTurn = true;
        }
    }
}
