using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tooltipable : MonoBehaviour, PlayerAction
{

    public TooltipScriptable tooltipData;

    public void OnDeselect()
    {

    }

    public void OnDragEnd(RaycastHit hit)
    {

    }

    public void OnDragStart(RaycastHit hit)
    {

    }

    public void OnLeftClickDown(RaycastHit hit)
    {

    }

    public void OnLeftClickHolding(RaycastHit hit)
    {

    }

    public void OnLeftDrag(RaycastHit hit)
    {

    }

    public void OnLongLeftClickUp(RaycastHit hit)
    {

    }

    public void OnmouseIn(RaycastHit hit)
    {
        UIManager.Instance.DisplayPropsTooltip(transform.position, tooltipData);
    }

    public void OnMouseOut(RaycastHit hit)
    {
        UIManager.Instance.HidePropsTooltip();
    }

    public void OnRightClickWhileDragging(RaycastHit hit)
    {

    }

    public void OnRightClickWhileHolding(RaycastHit hit)
    {

    }

    public void OnSelect()
    {

    }

    public void OnShortLeftClickUp(RaycastHit hit)
    {

    }

    public void OnShortRightClick(RaycastHit hit)
    {

    }

    public void StopAction()
    {

    }
}
