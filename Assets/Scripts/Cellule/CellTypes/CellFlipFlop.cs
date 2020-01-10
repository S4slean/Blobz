﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellFlipFlop : CellMain
{
    private LinkClass currentOutputLink;
    private bool isToggle;
    private int switchCount;


    public override void BlobsTick()
    {
        //ANIM
        haveExpulse = false;
        if (blobNumber > 0 && currentOutputLink != null)
        {
            currentTick++;
            if (currentTick == currentTickForActivation)
            {
                for (int i = 0; i < currentRejectPower; i++)
                {

                    if (blobNumber <= 0)
                    {
                        break;
                    }
                    currentOutputLink.Transmitt(1, BlobCheck());
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
        else if (outputLinks.Count == 1)
        {
            if (!isToggle)
            {
                currentOutputLink = outputLinks[0];
                outputLinks[0].isCLosed(false);
                isToggle = true;
            }
            else
            {
                outputLinks[0].isCLosed(true);
                currentOutputLink = null;
                isToggle = false;
            }
            return;
        }


        isToggle = false;

        if (switchCount >= outputLinks.Count)
        {
            switchCount = 0;
        }

        for (int i = 0; i < outputLinks.Count; i++)
        {
            outputLinks[i].isCLosed(true);
        }
        outputLinks[switchCount].isCLosed(false); //Unclose celui là 
        currentOutputLink = outputLinks[switchCount];

        switchCount++;
    }
    private void SwitchLink(LinkClass link)
    {
        for (int i = 0; i < outputLinks.Count; i++)
        {
            outputLinks[i].isCLosed(true);
        }
        link.isCLosed(false); //Unclose celui là 
        currentOutputLink = link;

        switchCount++;
    }

    public override void SetupVariable()
    {
        currentOutputLink = null;
        isToggle = false;
        switchCount = 0;
        base.SetupVariable();
    }

    public override void AddLinkReferenceToCell(LinkClass linkToAdd, bool output)
    {
        base.AddLinkReferenceToCell(linkToAdd, output);
        if (output)
        {
            SwitchLink(linkToAdd);
        }
    }

    public override void RemoveLink(LinkClass linkToRemove, bool isOutput)
    {
        base.RemoveLink(linkToRemove, isOutput);
        isToggle = false;
        SwitchLink();
    }
}
