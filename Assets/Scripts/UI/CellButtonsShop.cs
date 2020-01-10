using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CellButtonsShop : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    bool mouseOnMe = false;
    EventTrigger trigger;
    PointerEventData pointer;
    Image img;
    RectTransform rect;
    Animator anim;

    private void Start()
    {
        trigger = GetComponent<EventTrigger>();
        img = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
        anim = GetComponent<Animator>();

        //rect.pivot = img.sprite.pivot;
        img.alphaHitTestMinimumThreshold = 0.9f;
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
        mouseOnMe = true;
        anim.Play("DisplaySubMenu");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOnMe = false;
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
