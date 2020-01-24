using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellFusee : CellMain
{
    private int clickforLauchn;
    public ProgressBar progressBar;
    private int Life;


    public override void BlobsTick()
    {
        if (stuckBlobs.Count <= 0 && Life < myCellTemplate.maxLifeProd)
        {
            StockageCapabilityVariation(1);
        }

        if (blobNumber > 0)
        {
            BlobNumberVariation(myCellTemplate.blobLostPerTick, BlobCheck(), false);
            //CellManager.Instance.EnergyVariation(currentEnergyPerClick);
            RessourceTracker.instance.EnergyVariation(currentEnergyPerClick);
        }
    }

    public override void OnShortLeftClickUp(RaycastHit hit)
    {
        if (overLoad)
        {
            clickforLauchn++;
            //Play FX 
            if (clickforLauchn > myCellTemplate.clickBeforeLaunch)
            {
                // C'est WIN
            }
        }
    }

}
