using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tooltipable : MonoBehaviour, PlayerAction
{

    public TooltipScriptable tooltipData;

    public void OnDeselect()
    {
        throw new System.NotImplementedException();
    }

    public void OnDragEnd(RaycastHit hit)
    {
        throw new System.NotImplementedException();
    }

    public void OnDragStart(RaycastHit hit)
    {
        throw new System.NotImplementedException();
    }

    public void OnLeftClickDown(RaycastHit hit)
    {
        throw new System.NotImplementedException();
    }

    public void OnLeftClickHolding(RaycastHit hit)
    {
        throw new System.NotImplementedException();
    }

    public void OnLeftDrag(RaycastHit hit)
    {
        throw new System.NotImplementedException();
    }

    public void OnLongLeftClickUp(RaycastHit hit)
    {
        throw new System.NotImplementedException();
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
        throw new System.NotImplementedException();
    }

    public void OnRightClickWhileHolding(RaycastHit hit)
    {
        throw new System.NotImplementedException();
    }

    public void OnSelect()
    {
        throw new System.NotImplementedException();
    }

    public void OnShortLeftClickUp(RaycastHit hit)
    {
        throw new System.NotImplementedException();
    }

    public void OnShortRightClick(RaycastHit hit)
    {
        throw new System.NotImplementedException();
    }

    public void StopAction()
    {
        throw new System.NotImplementedException();
    }
}
