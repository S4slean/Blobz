using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellRendererDetection : MonoBehaviour
{
    public CellMain cellParent;

    private void OnBecameInvisible()
    {
        Debug.Log("Invisible ", cellParent);
        cellParent.isVisible = false;

    }
    private void OnBecameVisible()
    {
        Debug.Log("Visible" , cellParent);
        cellParent.isVisible = true;
    }

}
