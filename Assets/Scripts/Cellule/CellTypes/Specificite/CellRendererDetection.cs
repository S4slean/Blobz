using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellRendererDetection : MonoBehaviour
{
    public CellMain cellParent;

    private void OnBecameInvisible()
    {
        
        cellParent.isVisible = false;

    }
    private void OnBecameVisible()
    {
        cellParent.isVisible = true;
    }

}
