using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellProximityDectection : MonoBehaviour
{
    public SphereCollider myCollider;
    public CellMain parent; 


    private void OnTriggerEnter(Collider other)
    {
        CellMain cell = other.GetComponent<CellMain>();
        if (cell != null)
        {
            parent.AddToCellAtPromity(cell);
            Debug.Log("new cell Detected");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        CellMain cell = other.GetComponent<CellMain>();
        if (cell != null)
        {
            parent.RemoveToCellAtPromity(cell);
            Debug.Log("Lost a cell Detected");
        }

    }
}

