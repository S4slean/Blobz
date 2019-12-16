using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkJointClass : PoolableObjects
{
    //public LinkClass link;
    //public List<CellMain> cellsAttach = new List<CellMain>() ; 
    public Color outputColor;
    public Color InputColor;
    public Color flexColor;


    public MeshRenderer mF;

    public linkJointType typeOfJoint;

    //public bool nextToACell;
    public bool disponible = true;
    //public bool isOutput;
    //public bool isLock;
    //public bool canBeDropEveryWhere;


    public void Init(linkJointType type)
    {
        typeOfJoint = type;
        GraphUpdate();
    }

    //public void PlayerInteraction()
    //{
    //    if (!isLock)
    //    {
    //        link.SensSwitch();
    //    }
    //}

    public void GraphUpdate()
    {
        //if (isOutput)
        //{
        //    mF.material.SetColor("_Color", outputColor);
        //}
        //else
        //{
        //    mF.material.SetColor("_Color", InputColor);
        //}

        switch (typeOfJoint)
        {
            case linkJointType.flex:
                mF.material.SetColor("_Color", flexColor);
                break;
            case linkJointType.output:
                mF.material.SetColor("_Color", outputColor);
                break;
            case linkJointType.input:
                mF.material.SetColor("_Color", InputColor);
                break;

        }
    }

    public override void Inpool()
    {
        base.Inpool();
        disponible = true;
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    CellMain cell = other.GetComponent<CellMain>();
    //    if (cell)
    //    {
    //        nextToACell = true;
    //        cellsAttach.Add(cell);
    //    }
    //}
    //private void OnTriggerExit(Collider other)
    //{
    //    CellMain cell = other.GetComponent<CellMain>();
    //    if (cell)
    //    {
    //        nextToACell = false;
    //        cellsAttach.Remove(cell);
    //    }
    //}
}



public enum linkJointType
{
    flex,
    output,
    input

}