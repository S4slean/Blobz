using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellBroyeur : CellMain
{

    public override void BlobsTick()
    {
        haveExpulse = false;

        if (blobNumber > 0)
        {
            currentTick++;
            if (currentTick == currentTickForActivation)
            {
                for (int i = 0; i < currentRejectPower; i++)
                {
                    if (blobNumber > 0)
                    {

                        RemoveBlob(1);
                        CellManager.Instance.EnergyVariation(myCellTemplate.energyPerblop);
                        haveExpulse = true;
                    }
                }
                currentTick = 0;
            }
        }
        if (haveExpulse)
        {
            anim.Play("BlobExpulsion");
        }
    }
    public override void TickInscription()
    {
        TickManager.doTick2 += BlobsTick;
    }
    public override void TickDesinscription()
    {
        TickManager.doTick2 -= BlobsTick;
    }
}
