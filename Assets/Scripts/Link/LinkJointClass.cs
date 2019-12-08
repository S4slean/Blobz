using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkJointClass : PoolableObjects
{
    public LinkClass link;
    public bool isOutput;
    public Color outputColor;
    public Color InputColor;

    public MeshRenderer mF;


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

    private void GraphUpdate()
    {
        if (isOutput)
        {
            mF.material.SetColor("_Color", outputColor);
        }
        else
        {
            mF.material.SetColor("_Color", outputColor);
        }
    }
}
