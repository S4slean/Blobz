using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellProximityDectection : PoolableObjects
{
    public int proximityLevel;
    public SphereCollider myCollider;
    public CellMain parent;
    public MeshRenderer mR;
    public Color[] proximityColor;



    public void Init(int proxLevel, Transform targetTransform)
    {

        Color matColor = proximityColor[proxLevel - 1];
        mR.material.SetColor("_Color", matColor);
        transform.position = targetTransform.position + new Vector3(0f, 0.01f * (float)proxLevel, 0f);
        proximityLevel = proxLevel;
    }


    private void OnTriggerEnter(Collider other)
    {
        CellMain cell = other.GetComponent<CellMain>();
        if (cell != null && cell != parent)
        {
            cell.inThoseCellProximity.Add(this);
            //parent.AddToCellAtPromity(cell);
            cell.AddProximityInfluence(this);
        }
    }




    private void OnTriggerExit(Collider other)
    {
        Debug.Log("ça sort");
        CellMain cell = other.GetComponent<CellMain>();
        if (cell != null && cell != parent)
        {
            cell.RemoveProximityInfluence(this);
        }

    }
}

