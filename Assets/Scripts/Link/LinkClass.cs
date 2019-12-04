using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LinkClass : PoolableObjects
{
    #region REFS
    public Animator anim;

    #endregion

    #region INFO
    public float angle;
    public CellMain originalCell, receivingCell;
    public LineRenderer line;

    public Vector3 startPos, endPos;

    //Gestion du graph 
    private int range;
    private float bendRatio;
    //direction non normaliser
    private Vector3 trajectoir;
    
    #endregion

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
        anim.Play("Transfer");
    }
    public bool CheckLength(Vector3 posToTest)
    {
        //distance entre la position testé et le point de début et de fin ( donc entre cellule d'origine  et de fin
        float length1 = Vector3.Distance(startPos, posToTest);
        float length2 = Vector3.Distance(endPos, posToTest);
        //check la distance en fonction de la range des 2 cellules
        if (length1 <= originalCell.myCellTemplate.rangeBase / 2 && length2 <= receivingCell.myCellTemplate.rangeBase / 2)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CheckNewLinkLength (Vector3 posToTest , CellMain startCell)
    {
        float length1 = Vector3.Distance(startPos, posToTest);
        //a modifier par rappport à la proximité
        if (length1 <= startCell.myCellTemplate.rangeBase/2)
        {
            Debug.Log(" Lien ok");
            endPos = posToTest;
            return true; 
        }
        else
        {
            Debug.Log(" l'un des lien est trop court");
            return false;
        }

    }

    public void UpdateLinks(CellMain cellInDeplacement, Vector3 posToTest)
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
            line.SetPosition(line.positionCount - 1, endPos);
        }

    }
    public void Break()
    {
        receivingCell.RemoveLink(this);
        originalCell.RemoveLink(this);
        gameObject.SetActive(false);
    }

    public void FirstSetup(Vector3 firstPos, Vector3 lastPos , int cellRange)
    {
        range = cellRange;
        line.positionCount = range;
        startPos = firstPos;
        endPos = lastPos;
        UpdatePoint();
    }

    private void UpdatePoint()
    {
        trajectoir = endPos - startPos;
        bendRatio = trajectoir.magnitude / range;

        Vector3 posFrag = trajectoir / range; 
        for (int i = 0; i < range; i++)
        {
            line.SetPosition(i, startPos + i * posFrag);
        }
        
    }
    public void UpdatePoint(Vector3 lastPos)
    {
        endPos = lastPos;

        trajectoir = endPos - startPos;
        bendRatio = trajectoir.magnitude / range;

        Vector3 posFrag = trajectoir / range;
        for (int i = 0; i < range; i++)
        {
            line.SetPosition(i, startPos + i * posFrag);
        }

    }


}
