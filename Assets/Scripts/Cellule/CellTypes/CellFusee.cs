using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellFusee : CellMain
{
    public override void BlobsTick()
    {
        if (blobNumber > 0 )
        {
            BlobNumberVariation(-1, BlobCheck(), false);
            //CellManager.Instance.EnergyVariation(currentEnergyPerClick);
            RessourceTracker.instance.EnergyVariation(currentEnergyPerClick);

        }
        base.BlobsTick();
    }



}
