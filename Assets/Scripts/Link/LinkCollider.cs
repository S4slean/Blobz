using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkCollider : MonoBehaviour, PlayerAction
{
    public bool canDestroyLink;
    public BoxCollider boxCollider;
    public LinkClass parent;

    private Transform myTransform;

    public void Awake()
    {
        if (myTransform == null)
        {
            myTransform = transform;
        }
    }

    public void UpdatePosAndScale(Vector3 pos, float angle, float scale)
    {

        boxCollider.enabled = true;
        myTransform.position = pos;
        myTransform.rotation = Quaternion.Euler(0, angle, 0);
        myTransform.localScale = new Vector3(0.6f, 0.2f, scale);

    }

    #region Interaction

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
    }

    public void OnMouseOut(RaycastHit hit)
    {
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
        //if (canDestroyLink)
        //{
        //    parent.Break();
        //}
    }

    public void StopAction()
    {
    }
    #endregion

}
