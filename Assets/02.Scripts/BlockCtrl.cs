using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BlockCtrl : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    private Image image;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0.5f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0.0f);
    }

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
