using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LinkClass : PoolableObjects
{
    #region VARIABLES

    #region REFS
    public Animator anim;

    #endregion

    #region INFO
    public float angle;
    public CellMain originalCell, receivingCell;
    public LinkJointClass[] joints = new LinkJointClass[2];
    public LineRenderer line;

    //public Vector3 extremityPos[1], extremityPos[1];
    //public Vector3 startPos, endPos;
    public Vector3[] extremityPos = new Vector3[2];

    private bool canSwitch;
    private bool WantToSwitch;
    private bool InTransmission;

    //Gestion du graph 
    private int range;
    private float bendRatio;
    //direction non normaliser
    private Vector3 trajectoir;

    #endregion

    #endregion

    #region COMMUN
    public void Break()
    {
        receivingCell.RemoveLink(this);
        originalCell.RemoveLink(this);
        gameObject.SetActive(false);
        //for (int i = 0; i < joints.Length; i++)
        //{
        //    if (!joints[i].isLock)
        //    {
        //        joints[i].Inpool();
        //    }
        //}
    }
    public void FirstSetup(Vector3 firstPos, Vector3 lastPos, int cellRange)
    {
        range = cellRange * 2;
        line.positionCount = range;
        extremityPos[0] = firstPos;
        extremityPos[1] = lastPos;
        for (int i = 0; i < joints.Length; i++)
        {
            joints[i] = ObjectPooler.poolingSystem.GetPooledObject<LinkJointClass>() as LinkJointClass;
            joints[i].transform.position = extremityPos[i];
            //joints[i].isOutput  = (bool)i;
            //joints[i].isOutput = i > 0 ? false : true;
            joints[i].GraphUpdate();
            joints[i].Outpool();
        }
        UpdatePoint();
    }
    public void FirstSetupWithSlot(Vector3 firstPos, Vector3 lastPos, int cellRange, LinkJointClass baseJoint/*, bool isOutput*/)
    {
        Debug.Log("yop");
        range = cellRange * 2;
        line.positionCount = range;
        extremityPos[0] = firstPos;
        extremityPos[1] = lastPos;

        LinkJointClass createdJoint = ObjectPooler.poolingSystem.GetPooledObject<LinkJointClass>() as LinkJointClass; ;
        //if (isOutput)
        //{
        baseJoint.transform.position = extremityPos[0];
        joints[0] = baseJoint;
        createdJoint.transform.position = extremityPos[1];
        createdJoint.typeOfJoint = linkJointType.input;
        joints[1] = createdJoint;
        //}
        //else
        //{
        //    baseJoint.transform.position = extremityPos[1];
        //    joints[1] = baseJoint;
        //    createdJoint.transform.position = extremityPos[0];
        //    createdJoint.isOutput = true;
        //}

        createdJoint.GraphUpdate();
        createdJoint.Outpool();

        UpdatePoint();

    }

    //A remplacée surement 
    public void UpdateLinks(CellMain cellInDeplacement, Vector3 posToTest)
    {
        //Updata la position du lien en fonction de la cellules déplacée
        if (cellInDeplacement == originalCell)
        {
            extremityPos[0] = posToTest;
            line.SetPosition(0, extremityPos[0]);
        }
        else
        {
            extremityPos[1] = posToTest;
            line.SetPosition(line.positionCount - 1, extremityPos[1]);
        }

    }
    private void UpdatePoint()
    {
        trajectoir = extremityPos[1] - extremityPos[0];
        bendRatio = (range - trajectoir.magnitude * 2) / range;


        line.material.SetFloat("_bendRatio", bendRatio);

        Vector3 posFrag = trajectoir / range;
        for (int i = 0; i < range - 1; i++)
        {
            line.SetPosition(i, extremityPos[0] + i * posFrag);
        }
        line.SetPosition(range - 1, extremityPos[1]);
    }
    public void UpdatePoint(Vector3 firstPos ,Vector3 lastPos)
    {
        extremityPos[0] = firstPos;
        extremityPos[1] = lastPos;
        joints[1].transform.position = extremityPos[1];
        joints[0].transform.position = extremityPos[0];

        trajectoir = extremityPos[1] - extremityPos[0];
        bendRatio = (range - trajectoir.magnitude * 2) / range;

        line.material.SetFloat("_bendRatio", bendRatio);


        Vector3 posFrag = trajectoir / range;
        for (int i = 0; i < range ; i++)
        {
            line.SetPosition(i, extremityPos[0] + i * posFrag);
        }
        line.SetPosition(range - 1, extremityPos[1]);
    }
    #endregion

    #region CHECKLENGTH
    public bool CheckNewLinkLength(Vector3 posToTest, CellMain startCell)
    {
        float length1 = Vector3.Distance(startCell.transform.position, posToTest);
        //a modifier par rappport à la proximité
        if (length1 <= startCell.GetCurrentRange())
        {
            Debug.Log(" Lien ok");
            extremityPos[1] = posToTest;
            return true;
        }
        else
        {
            Debug.Log(" l'un des lien est trop court");
            return false;
        }

        
    }
    public bool CheckLength(Vector3 posToTest)
    {
        //distance entre la position testé et le point de début et de fin ( donc entre cellule d'origine  et de fin
        float length1 = Vector3.Distance(extremityPos[0], posToTest);
        float length2 = Vector3.Distance(extremityPos[1], posToTest);
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

    #endregion

    #region CELL RELATIVE
    public void Transmitt(int blobAmount)
    {
        //Ancienne Version 
        originalCell.BlobNumberVariation(-blobAmount);
        receivingCell.BlobNumberVariation(blobAmount);
        //New Version
        //joints[0].cellsAttach[0].BlobNumberVariation(-blobAmount);
        //joints[1].cellsAttach[0].BlobNumberVariation(blobAmount);
        // on pourra lancer une anim ici 
        anim.Play("Transfer");
    }
    public void AngleFromCell(CellMain OutputCell)
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


    #endregion

    #region LINK JOINT

    //public void UpdatePointFromJoint(Vector3 targetPos, bool isOutpul)
    //{
    //    if (isOutpul)
    //    {
    //        extremityPos[1] = targetPos;
    //        //joints[1].transform.position = extremityPos[1];
    //    }
    //    else
    //    {
    //        extremityPos[0] = targetPos;
    //    }

    //    trajectoir = extremityPos[1] - extremityPos[0];
    //    bendRatio = trajectoir.magnitude / range;

    //    Vector3 posFrag = trajectoir / range;
    //    for (int i = 0; i < range - 1; i++)
    //    {
    //        line.SetPosition(i, extremityPos[0] + i * posFrag);
    //    }
    //    line.SetPosition(range - 1, extremityPos[1]);
    //}
    //public void SensSwitch()
    //{

    //    for (int i = 0; i < joints.Length; i++)
    //    {
    //        if (joints[i].isLock)
    //        {
    //            return;
    //        }
    //    }
    //    if (InTransmission)
    //    {
    //        WantToSwitch = true;
    //    }
    //    else
    //    {
    //        LinkJointClass tJoint = joints[0];
    //        joints[0] = joints[1];
    //        joints[1] = tJoint;

    //    }

    //}

    #endregion

}
