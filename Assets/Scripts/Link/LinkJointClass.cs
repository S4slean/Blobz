using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkJointClass : PoolableObjects
{
    public Color outputColor;
    public Color inputColor;
    public Color flexColor;


    //   public MeshRenderer mF;
    public SpriteRenderer sR;

    public linkJointType typeOfJoint;

    public bool disponible = true;


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
        switch (typeOfJoint)
        {
            case linkJointType.flex:
                // mF.material.SetColor("_Color", flexColor);
                sR.color = flexColor;
                break;
            case linkJointType.output:
                //  mF.material.SetColor("_Color", outputColor);
                sR.color = outputColor;
                break;
            case linkJointType.input:
                //  mF.material.SetColor("_Color", InputColor);
                sR.color = inputColor;
                break;

        }
    }

    public override void Inpool()
    {
        base.Inpool();
        disponible = true;
    }

    public override void Outpool()
    {
        base.Outpool();
    }
}



public enum linkJointType
{
    flex,
    output,
    input

}