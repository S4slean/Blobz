using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface PlayerAction 
{
    void OnLeftClickDown();

    void OnShortLeftClickUp();

    void OnLongLeftClickUp();

    void OnLeftClickHolding();

    void OnShortRightClick();

}
