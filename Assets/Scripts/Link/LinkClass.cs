﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(LineRenderer))]
public class LinkClass : PoolableObjects
{
    #region VARIABLES

    #region REFS
    public Animator anim;
    public GameObject[] lockIcon = new GameObject[2];
    private BlobCoach blobCoachInTransition;

    public LinkCollider myCollider;
    public TextMeshPro numberOfBlob;

    public LinkTransmissionNumber LinkTransmissionNumber;

    //private int transmissionAmount;
    private int blobTransmited;

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

    #region AnimVariable
    //private int transMitAmount;
    private BlobManager.BlobType transmitType;



    #endregion

    #endregion

    #region COMMUN

    public void Tick()
    {
        //numberOfBlob.text = transmissionAmount.ToString();
        blobTransmited = 0;
    }

    public void Break()
    {
        receivingCell.RemoveLink(this, false);
        originalCell.RemoveLink(this, true);
        myCollider.boxCollider.enabled = false;
        TickManager.doTick -= Tick;

        Inpool();
    }
    public void FirstSetup(Vector3 firstPos, Vector3 lastPos, int cellRange)
    {
        range = cellRange * 2;
        line.positionCount = range;
        extremityPos[0] = firstPos;
        extremityPos[1] = lastPos;
        //line.material.SetFloat("_rangeDivision", range);
        float rangeTransmit = (float)range / 2;
        line.material.SetFloat("_rangeDivision", rangeTransmit);
        Debug.Log("OUIII");

        for (int i = 0; i < joints.Length; i++)
        {
            joints[i] = ObjectPooler.poolingSystem.GetPooledObject<LinkJointClass>() as LinkJointClass;
            joints[i].transform.position = extremityPos[i];
            joints[i].GraphUpdate();
            joints[i].Outpool();
        }
        UpdatePoint();
    }
    public void FirstSetupWithSlot(Vector3 firstPos, Vector3 lastPos, int cellRange, LinkJointClass baseJoint/*, bool isOutput*/)
    {
        range = cellRange * 2;
        line.positionCount = range;
        extremityPos[0] = firstPos;
        extremityPos[1] = lastPos;
        float rangeTransmit = (float)range / 2;
        line.material.SetFloat("_rangeDivision", rangeTransmit);

        LinkJointClass createdJoint = ObjectPooler.poolingSystem.GetPooledObject<LinkJointClass>() as LinkJointClass; ;

        baseJoint.transform.position = extremityPos[0];
        joints[0] = baseJoint;
        joints[0].disponible = false;
        createdJoint.transform.position = extremityPos[1];
        createdJoint.typeOfJoint = linkJointType.input;
        joints[1] = createdJoint;


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
    public void UpdatePoint(Vector3 firstPos, Vector3 lastPos)
    {
        extremityPos[0] = firstPos;
        extremityPos[1] = lastPos;
        joints[1].transform.position = extremityPos[1];
        joints[0].transform.position = extremityPos[0];

        trajectoir = extremityPos[1] - extremityPos[0];
        bendRatio = (range - trajectoir.magnitude * 2) / range;

        line.material.SetFloat("_bendRatio", bendRatio);


        Vector3 posFrag = trajectoir / range;
        for (int i = 0; i < range; i++)
        {
            line.SetPosition(i, extremityPos[0] + i * posFrag);
        }
        line.SetPosition(range - 1, extremityPos[1]);
    }
    #endregion

    #region CHECKLENGTH
    public bool CheckNewLinkLength(Vector3 posToTest, CellMain startCell, CellMain newCell)
    {
        float length1 = Vector3.Distance(startCell.transform.position, posToTest);
        //a modifier par rappport à la proximité
        if (length1 - 0.01f <= (startCell.GetCurrentRange() + startCell.myCellTemplate.slotDistance + newCell.myCellTemplate.slotDistance))
        {

            Vector3 _dir = (posToTest - startCell.transform.position).normalized;
            extremityPos[1] = posToTest - _dir * newCell.myCellTemplate.slotDistance;
            return true;
        }
        else
        {
            return false;
        }


    }
    public bool CheckLength(Vector3 posToTest)
    {
        //distance entre la position testé et le point de début et de fin ( donc entre cellule d'origine  et de fin
        float length1 = Vector3.Distance(extremityPos[0], posToTest);
        float length2 = Vector3.Distance(extremityPos[1], posToTest);
        //check la distance en fonction de la range des 2 cellules
        if (length1 <= originalCell.GetCurrentRange()
            &&
            length2 <= receivingCell.GetCurrentRange()
            )
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


    public void Transmitt(int blobAmount, BlobManager.BlobType _blobType)
    {

        if (anim == null || originalCell == null || receivingCell == null)
        {
            return;
        }

        transmitType = _blobType;
        blobTransmited += blobAmount;

        if (_blobType == BlobManager.BlobType.coach)
        {
            blobCoachInTransition = originalCell.blobCoaches[originalCell.blobCoaches.Count - 1];
            blobCoachInTransition.ChangeCellOut(receivingCell);
        }
        else
        {
            originalCell.BlobNumberVariation(-blobAmount, _blobType, true);
        }

        float speed = 1 / (TickManager.instance.tickDuration - TickManager.instance.tickDuration / 1.4f);

        anim.speed = speed;
        numberOfBlob.text = blobTransmited.ToString();
        line.material.SetFloat("_blobNumber", blobTransmited);

        anim.Play("Transfer");
        StartCoroutine(Transmission((TickManager.instance.tickDuration - TickManager.instance.tickDuration / 1.4f), blobAmount));
    }

    private IEnumerator Transmission(float delay, int blobAmount)
    {
        yield return new WaitForSeconds(delay);
        if (transmitType == BlobManager.BlobType.coach)
        {
            blobCoachInTransition.ChangeCellArrive();
        }
        else
        {
            receivingCell.BlobNumberVariation(blobAmount, transmitType, true);
        }
        //transmissionAmount += transmissionAmount;

    }

    public void EndTransmit()
    {

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
        Vector3 _pos = extremityPos[0] + trajectoir / 2;
        float scale = trajectoir.magnitude;

        TickManager.doTick += Tick;
        myCollider.UpdatePosAndScale(_pos, angle, scale);
        LinkTransmissionNumber.UpdatePosAndScale(_pos, angle);

    }

    public void GetInputSlot(CellMain _receivingCell)
    {
        joints[1].Outpool();
        joints[1].transform.position = extremityPos[1];
        joints[1].typeOfJoint = linkJointType.input;
        joints[1].disponible = false;
        joints[1].GraphUpdate();

        UpdatePoint(extremityPos[0], extremityPos[1]);
        receivingCell = _receivingCell;
    }

    public void isCLosed(bool toggle)
    {
        if (toggle)
        {
            for (int i = 0; i < lockIcon.Length; i++)
            {
                lockIcon[i].SetActive(true);
                lockIcon[i].transform.position = extremityPos[i] + new Vector3(0, 0.05f, 0);
            }
        }
        else
        {
            for (int i = 0; i < lockIcon.Length; i++)
            {
                lockIcon[i].SetActive(false);
            }
        }
    }

    #endregion

}
