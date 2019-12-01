using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellBroyeur : CellMain
{

    public override void BlobsTick()
    {
        haveExpulse = false;
        if (BlobNumber > 0 )
        {
            for (int i = 0; i < currentRejectPower; i++)
            {
                RemoveBlob(1);
                CellManager.Instance.EnergyVariation(myCellTemplate.energyPerblop);
                haveExpulse = true;
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
