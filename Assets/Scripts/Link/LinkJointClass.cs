using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkJointClass : PoolableObjects
{
    public LinkClass link;
    public Color outputColor;
    public Color InputColor;


    public MeshRenderer mF;

    public bool isOutput;
    public bool isLock;


    public void Init(bool _isOuput)
    {
        isOutput = _isOuput;
        GraphUpdate();
    }

    public void PlayerInteraction()
    {
        Switch();
    }

    private void Switch()
    {
        if (!isLock)
        {
            if (isOutput)
            {
                isOutput = false;
            }
            else
            {
                isOutput = true;
            }
            GraphUpdate();
        }
        else
        {
            Debug.Log("Le joint est lock");
        }
    }

    private void GraphUpdate()
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
}
