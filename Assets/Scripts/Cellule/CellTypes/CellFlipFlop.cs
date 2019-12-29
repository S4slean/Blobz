using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellFlipFlop : CellMain
{
    private LinkClass currentOutputLink;
    private bool isToggle;
    private int switchCount;


    public override void BlobsTick()
    {
        BlobNumberVariation(myCellTemplate.prodPerTickBase);
        if (currentOutputLink != null)
        {
            return;
        }

        //ANIM
        haveExpulse = false;
        if (blobNumber > 0)
        {
            currentTick++;
            if (currentTick == currentTickForActivation)
            {
                for (int i = 0; i < myCellTemplate.rejectPowerBase; i++)
                {
                    if (blobNumber <= 0)
                    {
                        break;
                    }
                    currentOutputLink.Transmitt(1);
                    haveExpulse = true;
                }
                currentTick = 0;
            }
        }
        else
        {
            currentTick = 0;
        }

        if (haveExpulse)
        {
            anim.Play("BlobExpulsion");
        }

    }

    #region PLAYER ACTION INTERFACE 

    public override void OnShortLeftClickUp(RaycastHit hit)
    {
        // base.OnShortLeftClickUp(hit);
        SwitchLink();
    }
    #endregion

    private void SwitchLink()
    {
        if (outputLinks.Count == 0)
        {
            currentOutputLink = null;
            return;
        }

        if (outputLinks.Count == 1)
        {
            if (!isToggle)
            {
                currentOutputLink = outputLinks[0];
                isToggle = true;
            }
            else
            {
                currentOutputLink = null;
                isToggle = false;
            }
            return;
        }


        if (switchCount == outputLinks.Count)
        {
            switchCount = 0;
        }

        for (int i = 0; i < outputLinks.Count; i++)
        {
            //METTRE un truc qui close
        }
        // outputLinks[switchCount] //un close celui là 
        currentOutputLink = outputLinks[switchCount];


        switchCount++;

    }


}
