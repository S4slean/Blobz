using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CellButtonsShop : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    bool mouseOnMe = false;
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

    private void OnDisable()
    {
        mouseOnMe = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!detectMouse)
            return;

        mouseOnMe = true;
        anim.SetInteger("opening", 2);
        CellSelectionShop.instance.HideOtherSubMenus(index);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOnMe = false;
    }

    public void DetectMouse()
    {
        detectMouse = true;
    }

    public void UndetectMouse()
    {
        detectMouse = false;
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
