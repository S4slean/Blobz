using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LinkClass : PoolableObjects
{
    public float angle;
    public CellMain originalCell, receivingCell;
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
        receivingCell.AddBlob(1); 
        // on pourra lancer une anim ici 
    }

    public  bool CheckLength(Vector3 posToTest)
    {
        //distance entre la position testé et le point de début et de fin ( donc entre cellule d'origine  et de fin
        float length1 = Vector3.Distance(startPos, posToTest);
        float length2 = Vector3.Distance(endPos, posToTest);

        //check la distance en fonction de la range des 2 cellules
        if (length1 <= originalCell.myCellTemplate.range && length2 <= receivingCell.myCellTemplate.range)
        {
            return true;
        }
        else
        {
            Debug.Log(" l'un des lien est trop court");
            return false;
        }
    }
    public void UpdateLinks(CellMain cellInDeplacement , Vector3 posToTest)
    {
        //Updata la position du lien en fonction de la cellules déplacée
        if (cellInDeplacement == originalCell)
        {
            startPos = posToTest;
            line.SetPosition(0, startPos);
        }
        else
        {
            endPos = posToTest;
            line.SetPosition(1, endPos);
        }

    }

    public void Break()
    {
        receivingCell.RemoveLink(this);
        originalCell.RemoveLink(this);
        gameObject.SetActive(false);
    }

}
