using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SubMenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    bool mousOnMe = false;
    Image img;
    public CellMain cellAssociated;


    private void Start()
    {
        img = GetComponent<Image>();
        img.alphaHitTestMinimumThreshold = .5f;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UIManager.Instance.LoadToolTip(transform.position, cellAssociated, true);
        mousOnMe = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.UnloadToolTip();

        mousOnMe = false;

    }

    public void Update()
    {
        if(mousOnMe && Input.GetMouseButtonUp(0))
        {
            CellSelectionShop.instance.CellConstruction(cellAssociated);
        }
    }


}
