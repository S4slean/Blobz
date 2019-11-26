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

    private void Start()
    {
        trigger = GetComponent<EventTrigger>();
    }

    private void OnDisable()
    {
        mouseOnMe = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOnMe = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOnMe = false;
    }

    private void Update()
    {
        pointer = new PointerEventData(EventSystem.current);

        if(mouseOnMe && Input.GetMouseButtonUp(0))
        {
            trigger.OnPointerUp(pointer);
        }
    }
}
