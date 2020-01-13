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

        UIManager.Instance.DisplayCostToolTip(transform.position, cellAssociated);
        mousOnMe = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.HideCostTooltip();
        mousOnMe = false;

    }

    public void Update()
    {
        if(mousOnMe && Input.GetMouseButtonUp(0))
        {
            Debug.Log("allo");
            CellSelectionShop.instance.CellConstruction(cellAssociated);
        }
    }


}
