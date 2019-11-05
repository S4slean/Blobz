using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LinkClass : PoolableObjects
{
    public float angle;
    public CellMain originalCell, receveingCell;
    public LineRenderer line;

    public Vector3 startPos, endPos;

    public void Init(CellMain OutputCell)
    {
        originalCell = OutputCell;
        Vector3 dir = (line.GetPosition(1) - OutputCell.transform.position).normalized;
        if (line.GetPosition(1).x <= OutputCell.transform.position.x)
        {
            angle = 360 - Vector3.Angle(originalCell.transform.forward, dir);
        }
        else if (line.GetPosition(1).x > OutputCell.transform.position.x)
        {
            angle = Vector3.Angle(originalCell.transform.forward, dir);
        }
        //angle = 180 -( 180 * Vector3.Dot(OutputCell.transform.forward, dir) );
        //Vector3 relative = outputCell.transform.InverseTransformDirection(line.GetPosition(1));
        //angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;
    }

    public void Transmitt()
    {
        originalCell.RemoveBlob(1);
        receveingCell.AddBlob(1); 
        // on pourra lancer une anim ici 
    }

    public void Break()
    {
        receveingCell.RemoveLink(this);
        originalCell.RemoveLink(this);
        gameObject.SetActive(false);
    }
}
