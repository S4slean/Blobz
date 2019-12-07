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
    private List<CellMain> cellDetected = new List<CellMain>();

    public void Init(int proximityLevel , Transform targetTransform)
    {

        Color matColor = proximityColor[proximityLevel - 1];
        mR.material.SetColor("_Color", matColor);
        transform.position = targetTransform.position + new Vector3(0f, 0.01f * (float)proximityLevel, 0f);
    }


    private void OnTriggerEnter(Collider other)
    {
        CellMain cell = other.GetComponent<CellMain>();
        if (cell != null )
        {
            //parent.AddToCellAtPromity(cell);
            cellDetected.Add(cell);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        CellMain cell = other.GetComponent<CellMain>();
        if (cell != null)
        {
            cellDetected.Remove(cell);
        }

    }
}

