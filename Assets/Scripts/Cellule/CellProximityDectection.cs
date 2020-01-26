using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellProximityDectection : PoolableObjects
{
    public int proximityLevel;
    public int productionBonusRatio;
    public SphereCollider myCollider;
    public CellMain parent;
    public MeshRenderer mR;
    public Color[] proximityColor;


    public void Init(int proxLevel, Transform targetTransform)
    {

        Color matColor = proximityColor[proxLevel - 1];
        mR.material.SetColor("_Color", matColor);
        transform.position = targetTransform.position + new Vector3(0f, 0.01f * (float)proxLevel + 0.3f, 0f);
        proximityLevel = proxLevel;
    }


    //private void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log("ça rentre" + parent.transform);
    //    CellMain cell = other.GetComponent<CellMain>();
    //    if (cell != null && cell != parent)
    //    {
    //        cell.inThoseCellProximity.Add(this);
    //        //parent.AddToCellAtPromity(cell);
    //        cell.AddProximityInfluence(this);
    //        if (parent.myCellTemplate.type == CellType.Productrice)
    //        {
    //            CellProductrice _parent = parent as CellProductrice;
    //            _parent.ProductriceProximityGestion(this, true);
    //        }
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    Debug.Log("sa sort", parent);
    //    CellMain cell = other.GetComponent<CellMain>();
    //    if (cell != null && cell != parent)
    //    {
    //        cell.RemoveProximityInfluence(this);
    //        if (parent.myCellTemplate.type == CellType.Productrice)
    //        {
    //            CellProductrice _parent = parent as CellProductrice;
    //            _parent.ProductriceProximityGestion(this, false);
    //        }
    //    }

    //}
}

