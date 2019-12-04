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

                        BlobNumberVariation(-1);
                        CellManager.Instance.EnergyVariation(myCellTemplate.energyPerblop);
                        haveExpulse = true;
                    }
                }

                for (int i = 0; i < outputLinks.Count; i++)
                {
                    if (blobNumber <= 0)
                    {
                        break;
                    }
                    //Pour l'instant il y a moyen que si une cellule creve la prochaine 
                    //soit sauté mai squand il y aura les anim , ce sera plus possible
                    outputLinks[i].Transmitt(1);
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
    public override void TickInscription()
    {
        TickManager.doTick2 += BlobsTick;
    }
    public override void TickDesinscription()
    {
        TickManager.doTick2 -= BlobsTick;
    }
}
