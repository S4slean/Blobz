using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CellButtonsShop : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public  bool detectMouse = true;
    public int index;

    Image img;
    Animator anim;

    private void Start()
    {
        img = GetComponent<Image>();
        anim = GetComponent<Animator>();

        //rect.pivot = img.sprite.pivot;
        img.alphaHitTestMinimumThreshold = 0.5f;
    }

    //private void OnEnable()
    //{
    //    anim.Play("DisplaySection");
    //}


    public void DetectMouse()
    {
        detectMouse = true;
    }

    public void UndetectMouse()
    {
        detectMouse = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //if (!detectMouse)
        //    return;

        //CellSelectionShop.instance.DisplaySubMenu(index);
    }



    public void OnPointerExit(PointerEventData eventData)
    {
    }

    //private void Update()
    //{
    //    pointer = new PointerEventData(EventSystem.current);

    //    if(mouseOnMe && Input.GetMouseButtonUp(0))
    //    {
    //        Debug.Log("create Cell");
    //        trigger.OnPointerUp(pointer);
    //    }
    //}
}
