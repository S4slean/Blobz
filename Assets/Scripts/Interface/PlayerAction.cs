using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface PlayerAction
{
    void OnLeftClickDown(RaycastHit hit);
    void OnShortLeftClickUp(RaycastHit hit);



    void OnLeftClickHolding(RaycastHit hit);
    void OnLongLeftClickUp(RaycastHit hit);



    void OnDragStart(RaycastHit hit);
    void OnLeftDrag(RaycastHit hit);
    void OnDragEnd(RaycastHit hit);


    void OnShortRightClick(RaycastHit hit);
    void OnRightClickWhileHolding(RaycastHit hit);
    void OnRightClickWhileDragging(RaycastHit hit);

    void OnmouseIn(RaycastHit hit);
    void OnMouseOut(RaycastHit hit);

    void OnSelect();
    void OnDeselect();

    void StopAction(); 


}
