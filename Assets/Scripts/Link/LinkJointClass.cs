using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkJointClass : PoolableObjects
{
    public LinkClass link;
    public List<CellMain> cellsAttach = new List<CellMain>() ; 
    public Color outputColor;
    public Color InputColor;


    public MeshRenderer mF;


    public bool nextToACell;
    public bool isOutput;
    public bool isLock;
    public bool canBeDropEveryWhere;


    public void Init(bool _isOuput)
    {
        isOutput = _isOuput;
        GraphUpdate();
    }

    public void PlayerInteraction()
    {
        if (!isLock)
        {
            link.SensSwitch();
        }
    }

    public void GraphUpdate()
    {
        if (isOutput)
        {
            mF.material.SetColor("_Color", outputColor);
        }
        else
        {
            mF.material.SetColor("_Color", InputColor);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        CellMain cell = other.GetComponent<CellMain>();
        if (cell)
        {
            nextToACell = true;
            cellsAttach.Add(cell);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        CellMain cell = other.GetComponent<CellMain>();
        if (cell)
        {
            nextToACell = false;
            cellsAttach.Remove(cell);
        }
    }
}
