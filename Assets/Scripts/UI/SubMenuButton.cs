using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SubMenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    bool mousOnMe = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        mousOnMe = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mousOnMe = false;

    }

    private void Update()
    {
        if(mousOnMe && Input.GetMouseButtonDown(0))
        {
            Debug.Log("createCell");
        }
    }
}
