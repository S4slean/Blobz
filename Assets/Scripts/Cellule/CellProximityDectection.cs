using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellProximityDectection : PoolableObjects
{
    public SphereCollider myCollider;
    public CellMain parent;
    public MeshRenderer mR;
    public Color[] proximityColor;

    private int proximityLevel; 

    public void Init(int proximityLevel)
    {

        Color matColor = proximityColor[proximityLevel - 1];
        mR.material.SetColor("_Color", matColor);
    }


    private void OnTriggerEnter(Collider other)
    {
        CellMain cell = other.GetComponent<CellMain>();
        if (cell != null)
        {
            parent.AddToCellAtPromity(cell);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        CellMain cell = other.GetComponent<CellMain>();
        if (cell != null)
        {
            parent.RemoveToCellAtPromity(cell);
        }

    }
}

